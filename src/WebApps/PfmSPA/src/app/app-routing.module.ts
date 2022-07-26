import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TransactionsComponent } from './transactions-feature/transactions-component/transactions-component';
import { TransactionDetailsComponent } from './transactions-feature/transaction-detail-component/transaction-detail-component';
const routes: Routes = [
  {
    path: 'transactions',
    component: TransactionsComponent
  },
  {
    path: 'transactions/categorize/:transactionId',
    component: TransactionDetailsComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
