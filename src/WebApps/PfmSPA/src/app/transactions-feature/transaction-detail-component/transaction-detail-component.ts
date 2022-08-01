import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common'
import { TransactionsFacadeService } from 'src/app/domain/application-services/transactions-facade.service';
import { map, startWith, switchMap } from 'rxjs';
import { TransactionView } from 'src/app/domain/models/GetTransactionsModels/TransactionView';
import { CategoryView } from 'src/app/domain/models/GetCategoriesModels/CategoryView';

@Component({
  selector: 'app-user-details',
  templateUrl: './transaction-detail.component.html',
  styleUrls: ['./transaction-detail.component.css']
})
export class TransactionDetailsComponent implements OnInit, AfterViewInit {
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

  transactionId: string;
  non_value : string = "none"

  amount : number
  beneficiaryName : string
  category : string 
  currency : string
  date : string
  description : string
  direction : string
  kind : string
  mcc : string

  public categoriesViewMap : Map<string, string> = new Map<string, string>();
  public directionViewMap : Map<string, string> = new Map<string, string>();
  public kindViewMap : Map<string, string> = new Map<string, string>();


  fgTopCategory : FormGroup
  selectedTopCategoryOption : "B"
  topLevelCategories : CategoryForm[] = []

  fgLowerCategory : FormGroup
  selectedLowerCategoryOption : "2"
  lowerLevelCategories : CategoryForm[] = []

  constructor(
    private activatedRoute: ActivatedRoute,
    private transactionsService: TransactionsFacadeService,
    private location: Location
  ) { }

  categorizeTransaction(){
    var topCategoryValue : (string | undefined) = this.fgTopCategory.value['topCategoryFormControl']
    if (topCategoryValue == undefined){
      window.alert("Please select a valid category input!")
      return;
    }

    var lowerCategoryValue : (string | undefined) = this.fgLowerCategory.value['lowerCategoryFormControl']

    var selectedValue : string
    if (lowerCategoryValue == undefined || lowerCategoryValue == this.non_value) {
      selectedValue = topCategoryValue!      
    } else {
      selectedValue = lowerCategoryValue!
    }
    
    this.transactionsService.categorize(this.transactionId, selectedValue).subscribe((success : boolean) => {
      window.alert('Categorization of transacation' + (success ? " is" : " is not") + ' successful!')

      if (success){
        this.location.back()
      }
    })
  }

  ngOnInit(): void {

    this.directionViewMap.set("d", "Debits")
    this.directionViewMap.set("c", "Credits")
    
    this.kinds.forEach((kind : Kind) => {
      this.kindViewMap.set(kind.value, kind.viewValue)
    })

    this.fgTopCategory = new FormGroup({
      topCategoryFormControl : new FormControl(this.selectedTopCategoryOption)
    })

    this.fgLowerCategory = new FormGroup({
      lowerCategoryFormControl : new FormControl(this.selectedLowerCategoryOption)
    })

    this.transactionsService.getCategories().subscribe((data : CategoryView[]) => {
      data.forEach(category => {
        this.categoriesViewMap.set(category.code, category.name)
      })

      data = data.filter(c => !/^-?\d+$/.test(c.code))
      data.forEach((cat : CategoryView)  => {
        this.topLevelCategories.push({value: cat.code, viewValue: cat.name})
      });
    })

    this.transactionsService.getCategories().subscribe((data : CategoryView[]) => {
      data = data.filter(c => /^-?\d+$/.test(c.code))
      data.forEach((cat : CategoryView)  => {
        this.lowerLevelCategories.push({value: cat.code, viewValue: cat.name})
      });
      this.lowerLevelCategories.push({value: this.non_value, viewValue:"None"})
    })

    this.activatedRoute.params.subscribe((params) => {
      this.transactionId = params['transactionId'];
    })
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
    
    this.fgTopCategory.valueChanges.pipe(
      startWith({}),
      switchMap(() => {
        return this.transactionsService.getCategories(this.fgTopCategory.value['topCategoryFormControl'])
      }),
      map((data : CategoryView[]) => {
        if (data === null)
        {
          return []
        }
        var parsedData : CategoryForm[] = []
        data.forEach((cat : CategoryView)  => {
          parsedData.push({value: cat.code , viewValue: cat.name})
        });
        parsedData.push({value: this.non_value , viewValue:"None"})
        return parsedData
      })
    ).subscribe(parsedData => (this.lowerLevelCategories = parsedData))
  }

  public back() {
    this.location.back();
  }
}

interface CategoryForm {
  value: string;
  viewValue: string;
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
  beneficiaryName : string
}

interface Kind {
  value: string;
  viewValue: string;
}


async function getCategoriesAsync() {
  let url = 'http://localhost:8001/api/v1/PersonalFinanceManagement/categories';
  try {
      let res = await fetch(url);
      return await res.json();
      
  } catch (error) {
      console.log(error);
  }
}



