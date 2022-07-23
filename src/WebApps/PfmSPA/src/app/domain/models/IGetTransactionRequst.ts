 export interface IGetTransactionsRequest {
    page : number,
    pageSize : number,
    startDate? : Date,
    endDate? : Date,
    kind : string
 }