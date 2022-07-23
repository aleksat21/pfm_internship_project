import { Injectable } from '@angular/core';
import { catchError, map, Observable, of } from 'rxjs';
import { TransactionsService } from '../infrastructure/transactions.service';
import { IGetTransactionsResponse } from '../models/IGetTransactionsResponse';
import { TransactionView } from '../models/TransactionView';

@Injectable({
  providedIn: 'root'
})
export class TransactionsFacadeService {

  constructor(private transactionsService : TransactionsService) { }

  public getTransactions(page : number, pageSize : number, startDate? : Date , endDate? : Date) : Observable<IGetTransactionsResponse>{
    const requst = {page , pageSize, startDate, endDate}

    return this.transactionsService.getTransactions(requst).pipe(
      map((response : IGetTransactionsResponse) => {
        return response;
      }),
      catchError((err) => {
        console.log(err);
        return of(err)
      })
    )
  }
}
