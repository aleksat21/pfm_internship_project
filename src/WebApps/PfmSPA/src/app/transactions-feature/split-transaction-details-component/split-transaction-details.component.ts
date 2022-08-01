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
  public directionViewMap : Map<string, string> = new Map<string, string>();
  public kindViewMap : Map<string, string> = new Map<string, string>();

  constructor(    
    private activatedRoute: ActivatedRoute,
    private transactionsService: TransactionsFacadeService,
    private location: Location
    ) { }


  back(){
    this.location.back()
  }
  ngAfterViewInit(): void {
    var data  = this.location.getState() as State
    this.amount = data.amount
    this.beneficiaryName = data.beneficiaryName
    this.category  = data.catcode
    this.currency = data.currency
    this.date = data.date
    this.description = data.description
    this.direction = data.direction
    this.kind = data.kind
    this.mcc = data.mcc
    this.splits = data.splits
  }

  ngOnInit(): void {
    this.kinds.forEach((kind : Kind) => {
      this.kindViewMap.set(kind.value, kind.viewValue)
    })

    this.directionViewMap.set("d", "Debits")
    this.directionViewMap.set("c", "Credits")

    this.activatedRoute.params.subscribe((params) => {
      this.transactionId = params['transactionId']
    })

    this.transactionsService.getCategories().subscribe((data : CategoryView[]) => {
        data.forEach(cat => {
          this.categoriesViewMap.set(cat.code, cat.name)
        })    
    }) 
  }
}

interface State{
  date : string,
  direction : string,
  amount : number,
  currency : string,
  mcc : string,
  description : string,
  kind : string,
  catcode : string,
  beneficiaryName : string,
  splits : SingleTransactionWithSplitView[]
}

interface Kind {
  value: string;
  viewValue: string;
}