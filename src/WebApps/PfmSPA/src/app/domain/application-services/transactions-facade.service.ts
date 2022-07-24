import { Injectable } from '@angular/core';
import { catchError, map, Observable, of } from 'rxjs';
import { TransactionsService } from '../infrastructure/transactions.service';
import { IGetCategoriesResponse } from '../models/IGetCategoriesResponse';
import { IGetTransactionsResponse } from '../models/IGetTransactionsResponse';
import { TransactionView } from '../models/TransactionView';

@Injectable({
  providedIn: 'root'
})
export class TransactionsFacadeService {

  constructor(private transactionsService : TransactionsService) { }

  public getCategories(parentCode? : string){
    const request = {parentCode}

    return this.transactionsService.getCategories(request).pipe(
      map((response : IGetCategoriesResponse) => {
        return response.items;
      }),
      catchError((err) => {
        console.log(err)
        return of(err)
      })
    )
  }

  public getTransactions(page : number, pageSize : number, startDate? : Date , endDate? : Date, kind = "all", sortBy = "date", orderByDirection = "asc") : Observable<IGetTransactionsResponse>{
    const request = {page , pageSize, startDate, endDate, kind, sortBy, orderByDirection}

    return this.transactionsService.getTransactions(request).pipe(
      map((response : IGetTransactionsResponse) => {
        return response;
      }),
      catchError((err) => {
        return of(err)
      })
    )
  }
}
