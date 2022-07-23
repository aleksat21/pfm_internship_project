import { TransactionView } from "./TransactionView";

export interface IGetTransactionsResponse {
    pageSize : number,
    page : number,
    totalCount : number,
    sortBy? : string,
    sortOrder? : string,
    items : [TransactionView]
}