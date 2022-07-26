import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';

import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table';
import { MatSort, Sort, SortDirection } from '@angular/material/sort';
import { merge, Observable, of as observableOf, of } from 'rxjs';
import {catchError, map, startWith, switchMap} from 'rxjs/operators';
import { TransactionsFacadeService } from 'src/app/domain/application-services/transactions-facade.service';
import { MatDatepicker } from '@angular/material/datepicker';
import {FormGroup, FormControl} from '@angular/forms';
import {MatDialog, MatDialogRef} from '@angular/material/dialog';
import { TransactionView } from 'src/app/domain/models/GetTransactionsModels/TransactionView';
import { formatDate } from '@angular/common';
import { CategoryView } from 'src/app/domain/models/GetCategoriesModels/CategoryView';

@Component({
  selector: 'app-transactions-component',
  templateUrl: './transactions-component.html',
  styleUrls: ['./transactions-component.css']
})
export class TransactionsComponent implements AfterViewInit, OnInit{

  public displayedColumns : string[] = ['id', 'beneficiaryName', 'date', 'direction', 'amount', 'description', 'currency', 'mcc', 'kind', 'catcode', 'action' ]
  public dataSource = new MatTableDataSource<TransactionView>()

  categories : CategoryView[] = []

  resultsLength = 0;

  range = new FormGroup({
    start: new FormControl(),
    end: new FormControl()
  });

  fgKind : FormGroup
  selectedKindOption = "all";
  kinds: Kind[] = [
    {value: 'all', viewValue: 'All'},
    {value: 'dep', viewValue: 'Deposit'},
    {value: 'wdw', viewValue: 'Withdrawal'},
    {value: 'pmt', viewValue: 'Payment'},
    {value: 'fee', viewValue: 'Fee'},
    {value: 'inc', viewValue: 'Intereset credit'},
    {value: 'rev', viewValue: 'Reversal'},
    {value: 'adj', viewValue: 'Adjustment'},
    {value: 'lnd', viewValue: 'Loan disbursement'},
    {value: 'lnr', viewValue: 'Loan repayment'},
    {value: 'fcx', viewValue: 'Foreign currency exchange'},
    {value: 'aop', viewValue: 'Account openning'},
    {value: 'acl', viewValue: 'Account closing'},
    {value: 'spl', viewValue: 'Split Payment'},
    {value: 'sal', viewValue: 'Salary'}
  ];

  fgSortBy : FormGroup
  selectedSortByOption = "date"
  sortByValues : SortBy[] = [
    {viewValue : "Id", value : "id"},
    {viewValue : "Beneficiary name", value : "beneficiary-name"},
    {viewValue : "Date", value : "date"},
    {viewValue : "Direction", value : "direction"},
    {viewValue : "Description", value : "description"},
    {viewValue : "Currency", value : "currency"},
    {viewValue : "Kind", value : "kind"}
  ]

  fgOrderByDirection : FormGroup
  selectedOrderByOption = "asc"
  orderByDirectionValues : OrderByDirection[] = [
    {viewValue : "Ascending", value : "asc"},
    {viewValue : "Desceding", value : "desc"}
  ]

  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(
      public dialog: MatDialog,
      private transactionsService : TransactionsFacadeService ,
      private router : Router,      
  ) { }


  public showTransactionDetails(transactionId: string) {
    this.router.navigate(['transactions/categorize/' + transactionId]);
  }
  
  
  ngOnInit(): void {
    this.transactionsService.getCategories().subscribe(data => (this.categories = data))

    this.fgKind = new FormGroup({
      kindFormControl : new FormControl(this.selectedKindOption)
    })

    this.fgSortBy = new FormGroup({
      sortByFormControl: new FormControl(this.selectedSortByOption)
    })

    this.fgOrderByDirection = new FormGroup({
      orderByDirectionFormControl : new FormControl(this.selectedOrderByOption)
    })
  }

  ngAfterViewInit() {
    this.range.valueChanges.subscribe(() => this.paginator.pageIndex = 0)

    merge(this.fgKind.valueChanges, this.fgSortBy.valueChanges, this.fgOrderByDirection.valueChanges).subscribe(() => {this.paginator.pageIndex = 0})

    merge(this.range.valueChanges, this.paginator.page, this.fgKind.valueChanges, this.fgSortBy.valueChanges, this.fgOrderByDirection.valueChanges).pipe(
      startWith({}),
      switchMap(() => {
        return this.transactionsService.getTransactions(
          this.paginator.pageIndex + 1,
          this.paginator.pageSize,
          this.range.value['start'],
          this.range.value['end'],
          this.fgKind.value['kindFormControl'],
          this.fgSortBy.value['sortByFormControl'],
          this.fgOrderByDirection.value['orderByDirectionFormControl'])
      }),
      map(data => {
        if (data === null){
          return []
        }

        this.resultsLength = data.totalCount
        data.items.map(t => {
          let category : (CategoryView | undefined)  = this.categories.find(c => c.code == t.catcode)
          if (category != undefined){
              t.catcode = category.name
          }
          let kind_detail  = this.kinds.find(f => f.value == t.kind)!
          t.direction = t.direction == 'd' ? "Debit" : "Credit"
          
          if (t.kind != undefined)
            t.kind = kind_detail.viewValue
          t.date =  formatDate(t.date, 'dd-MM-yyyy', 'en-US')
        })
        return data.items
      })
    )
    .subscribe(data => (this.dataSource.data = data))
  }
}

interface Kind {
  value: string;
  viewValue: string;
}

interface SortBy {
  value : string,
  viewValue : string
}

interface OrderByDirection {
  value : string,
  viewValue : string
}
