import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';

import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table';
import { MatSort, SortDirection } from '@angular/material/sort';
import { merge, Observable, of as observableOf, of } from 'rxjs';
import {catchError, map, startWith, switchMap} from 'rxjs/operators';
import { TransactionView } from '../../domain/models/TransactionView';
import { TransactionsFacadeService } from 'src/app/domain/application-services/transactions-facade.service';
import { MatDatepicker } from '@angular/material/datepicker';
import {FormGroup, FormControl} from '@angular/forms';

@Component({
  selector: 'app-transactions-component',
  templateUrl: './transactions-component.html',
  styleUrls: ['./transactions-component.css']
})
export class TransactionsComponent implements AfterViewInit, OnInit{

  public displayedColumns : string[] = ['id', 'beneficiaryName', 'date', 'direction', 'amount', 'description', 'currency', 'mcc', 'kind', 'catcode']
  public dataSource = new MatTableDataSource<TransactionView>()

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

  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(
      private transactionsService : TransactionsFacadeService ,
      private router : Router
  ) { }

  ngOnInit(): void {
    this.fgKind = new FormGroup({
      kinds : new FormControl(this.selectedKindOption)
    })
  }

  ngAfterViewInit() {
    this.range.valueChanges.subscribe(() => this.paginator.pageIndex = 0)
    merge(this.range.valueChanges, this.paginator.page, this.fgKind.valueChanges).pipe(
      startWith({}),
      switchMap(() => {
        return this.transactionsService.getTransactions(this.paginator.pageIndex + 1, this.paginator.pageSize, this.range.value['start'], this.range.value['end'], this.fgKind.value['kinds'])
      }),
      map(data => {
        console.log(this.fgKind.value['kinds'])
        if (data === null){
          return []
        }

        this.resultsLength = data.totalCount
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
