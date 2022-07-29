import { formatDate } from '@angular/common';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TransactionsFacadeService } from 'src/app/domain/application-services/transactions-facade.service';
import { CategoryView } from 'src/app/domain/models/GetCategoriesModels/CategoryView';
import { IGetTransactionWithSplitsResponse } from 'src/app/domain/models/GetTransactionWithSplitsModel/IGetTransactionWithSplitsResponse';
import { SingleTransactionWithSplitView } from 'src/app/domain/models/GetTransactionWithSplitsModel/SingleTransactionWithSplitView';
import { CategorizeView } from 'src/app/domain/models/PostCategorizeModels/CategorizeView';
import { Location } from '@angular/common'

@Component({
  selector: 'app-split-transaction-details',
  templateUrl: './split-transaction-details.component.html',
  styleUrls: ['./split-transaction-details.component.css']
})
export class SplitTransactionDetailsComponent implements OnInit, AfterViewInit {

  kinds: Kind[] = [
    {value: 'all', viewValue: 'All'},
    {value: 'dep', viewValue: 'Deposit'},
    {value: 'wdw', viewValue: 'Withdrawal'},
    {value: 'pmt', viewValue: 'Payment'},
    {value: 'fee', viewValue: 'Fee'},
    {value: 'inc', viewValue: 'Intereset credit'},
    {value: 'rev', viewValue: 'Reversal'},
    {value: 'adj', viewValue: 'Adjustment'},
    {value: 'lnd', viewValue: 'Loan disbursement'},
    {value: 'lnr', viewValue: 'Loan repayment'},
    {value: 'fcx', viewValue: 'Foreign currency exchange'},
    {value: 'aop', viewValue: 'Account openning'},
    {value: 'acl', viewValue: 'Account closing'},
    {value: 'spl', viewValue: 'Split Payment'},
    {value: 'sal', viewValue: 'Salary'}
  ];
  

  transactionId : string
  amount : number
  beneficiaryName : string
  category : string 
  currency : string
  date : string
  description : string
  direction : string
  kind : string
  mcc : string
  splits : SingleTransactionWithSplitView[] = []

  public categoriesViewMap : Map<string, string> = new Map<string, string>();

  constructor(    
    private activatedRoute: ActivatedRoute,
    private transactionsService: TransactionsFacadeService,
    private location: Location
    ) { }

  back(){
    this.location.back()
  }
  ngAfterViewInit(): void {

  }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe((params) => {
      this.transactionId = params['transactionId']
    })

    this.transactionsService.getCategories().subscribe((data : CategoryView[]) => {
        data.forEach(cat => {
          this.categoriesViewMap.set(cat.code, cat.name)
        })

        this.transactionsService.getTransactionDetails(this.transactionId).subscribe((data : IGetTransactionWithSplitsResponse) => {
        this.category = "Split"
      
        this.amount = data.amount
        this.beneficiaryName = data.beneficiaryName
        this.currency = data.currency
        this.description = data.description
        
        this.direction = data.direction == 'd' ? "Debits" : "Credits"
        this.date = formatDate(data.date, 'dd-MM-yyyy', 'en-us')
        
        let kind_detail  = this.kinds.find(f => f.value == data.kind)!
        this.kind = kind_detail.viewValue
  
        this.mcc = data.mcc
  
        this.splits = data.splits
      })
    
    }) 
  }
}

interface Kind {
  value: string;
  viewValue: string;
}