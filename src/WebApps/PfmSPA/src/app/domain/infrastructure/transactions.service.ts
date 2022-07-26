import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { SortDirection } from '@angular/material/sort';
import { Observable } from 'rxjs';
import { IGetTransactionsResponse } from '../models/GetTransactionsModels/IGetTransactionsResponse';
import { IGetCategoriesRequest } from '../models/GetTransactionsModels/IGetCategoriesRequest';
import { IGetCategoriesResponse } from '../models/GetTransactionsModels/IGetCategoriesResponse';
import { IGetTransactionsRequest } from '../models/GetTransactionsModels/IGetTransactionRequest';
import { ICategorizeRequest } from '../models/PostCategorizeModels/ICategorizeRequest';

@Injectable({
  providedIn: 'root'
})
export class TransactionsService {

  constructor(private http : HttpClient) { }

  public categorize(request : ICategorizeRequest) : Observable<Object>{
    let url = "http://localhost:8001/api/v1/PersonalFinanceManagement/transactions/" + request.id + "/categorize"

    return this.http.post(url, request.category)
  }

  public getCategories(request : IGetCategoriesRequest) : Observable<IGetCategoriesResponse>{
    let url = "http://localhost:8001/api/v1/PersonalFinanceManagement/categories"

    let queryParams = new HttpParams();

    if (request.parentCode != undefined){
      queryParams = queryParams.append("parentId", request.parentCode)
    }

    return this.http.get<IGetCategoriesResponse>(url, {
      params : queryParams
    })
  }

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
    queryParams = queryParams.append("sortBy", request.sortBy);
    queryParams = queryParams.append("sortOrder", request.orderByDirection);

    queryParams = queryParams.append("page", request.page)
    queryParams = queryParams.append("pageSize", request.pageSize)

    return this.http.get<IGetTransactionsResponse>(url, {
      params : queryParams
    })  
  }

}
