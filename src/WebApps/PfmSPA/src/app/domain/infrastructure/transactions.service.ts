import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { SortDirection } from '@angular/material/sort';
import { IGetTransactionsRequest } from '../models/IGetTransactionRequst';
import { Observable } from 'rxjs';
import { IGetTransactionsResponse } from '../models/IGetTransactionsResponse';

@Injectable({
  providedIn: 'root'
})
export class TransactionsService {

  constructor(private http : HttpClient) { }

  public getTransactions(request : IGetTransactionsRequest) : Observable<IGetTransactionsResponse>
  {
    let url = "http://localhost:8001/api/v1/PersonalFinanceManagement/transactions"

    let queryParams = new HttpParams();

    // queryParams = queryParams.append("transactionKind", transactionKind!)

    if (request.startDate != undefined){
      queryParams = queryParams.append("startDate", request.startDate.toDateString())
    }
    if (request.endDate != undefined){
      queryParams = queryParams.append("endDate", request.endDate.toDateString())
    }
    if (request.kind != 'all'){
      queryParams = queryParams.append("transactionKind", request.kind)
    }
    queryParams = queryParams.append("page", request.page)
    queryParams = queryParams.append("pageSize", request.pageSize)

    return this.http.get<IGetTransactionsResponse>(url, {
      params : queryParams
    })  
  }

}
