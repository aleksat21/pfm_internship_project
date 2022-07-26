import { SingleTransactionWithSplitView } from "./SingleTransactionWithSplitView"

export interface IGetTransactionWithSplitsResponse{
    id : string,
    beneficiaryName : string,
    date : string,
    direction : string,
    amount : number,
    description : string,
    currency : string,
    mcc : string,
    kind : string,
    catcode : string
    splits : [SingleTransactionWithSplitView]
}