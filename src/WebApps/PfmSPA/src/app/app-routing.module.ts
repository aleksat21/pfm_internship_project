import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TransactionsComponent } from './transactions-feature/transactions-component/transactions-component';
import { TransactionDetailsComponent } from './transactions-feature/transaction-detail-component/transaction-detail-component';
import { SplitTransactionDetailsComponent } from './transactions-feature/split-transaction-details-component/split-transaction-details.component';
import { AnalyticsComponent } from './transactions-feature/analytics-component/analytics.component';
const routes: Routes = [
  {
    path: 'transactions',
    component: TransactionsComponent
  },
  {
    path: 'transactions/categorize/:transactionId',
    component: TransactionDetailsComponent
  },
  {
    path: 'transactions/:transactionId/splits',
    component: SplitTransactionDetailsComponent
  },
  {
    path: 'analytics',
    component: AnalyticsComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
