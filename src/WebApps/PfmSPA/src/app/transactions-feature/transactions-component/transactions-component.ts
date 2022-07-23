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
export class TransactionsComponent implements AfterViewInit{

  public displayedColumns : string[] = ['id', 'beneficiaryName', 'date', 'direction', 'amount', 'description', 'currency', 'mcc', 'kind', 'catcode']
  public dataSource = new MatTableDataSource<TransactionView>()

  resultsLength = 0;

  range = new FormGroup({
    start: new FormControl(),
    end: new FormControl()
  });

  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(
      private transactionsService : TransactionsFacadeService ,
      private router : Router
  ) { }

  // catchError(() => observableOf(null)));
  ngAfterViewInit() {
    this.range.valueChanges.subscribe(() => this.paginator.pageIndex = 0)
    merge(this.range.valueChanges, this.paginator.page).pipe(
      startWith({}),
      switchMap(() => {
        return this.transactionsService.getTransactions(this.paginator.pageIndex + 1, this.paginator.pageSize, this.range.value['start'], this.range.value['end'])
      }),
      map(data => {
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

export interface TransactionsAPI {
  pageSize : number,
  page : number,
  totalCount : number,
  sortBy : string,
  sortOrder : string
  items : TransactionView[]
}
