import { AfterViewInit, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Route } from '@angular/router';
import { formatDate, Location } from '@angular/common';
import { TransactionsFacadeService } from 'src/app/domain/application-services/transactions-facade.service';
import { SingleCategoryAnalyticsView } from 'src/app/domain/models/GetAnalyticsModel/SingleCategoryAnalyticsView';
import { IGetCategoriesResponse } from 'src/app/domain/models/GetCategoriesModels/IGetCategoriesResponse';
import { CategorizeView } from 'src/app/domain/models/PostCategorizeModels/CategorizeView';
import { CategoryView } from 'src/app/domain/models/GetCategoriesModels/CategoryView';
import { MatTableDataSource } from '@angular/material/table';
import { FormControl } from '@angular/forms';
import {catchError, map, startWith, switchMap} from 'rxjs/operators';




@Component({
  selector: 'app-analytics',
  templateUrl: './analytics.component.html',
  styleUrls: ['./analytics.component.css']
})
export class AnalyticsComponent implements OnInit, AfterViewInit {

  public displayedColumns : string[] = ['catcode', 'count', 'amount']
  public dataSource = new MatTableDataSource<SingleCategoryAnalyticsView>()
  public dataSourceMap = new Map<string, MatTableDataSource<SingleCategoryAnalyticsView>>()

  public categoriesViewMap : Map<string, string> = new Map<string, string>()
  directionControl = new FormControl('d');

  startDateQuery : Date
  endDateQuery : Date

  startDateString : string = ""
  endDateString : string = "-"

  topTiesCategoriesData : SingleCategoryAnalyticsView[] = []


  constructor(private location : Location,
              private transcationService : TransactionsFacadeService
  )
  {}
  ngAfterViewInit(): void {
    var data  = this.location.getState() as State
    this.startDateQuery = data.startDate
    this.endDateQuery = data.endDate

    if (this.startDateQuery != undefined){
      this.startDateString = formatDate(this.startDateQuery, 'dd-MM-yyyy', 'en-US')
    }

    if (this.endDateQuery != undefined){
      this.endDateString = formatDate(this.endDateQuery, 'dd-MM-yyyy', 'en-US')
    }

    this.directionControl.valueChanges.pipe(
      startWith({}),
      switchMap(() => {
       return this.transcationService.getAnalyticsData(this.startDateQuery, this.endDateQuery, this.directionControl.value!)
      }),
      map(analyticsData => {
        this.dataSourceMap.clear()
        if (analyticsData === null){
          return []
        }
        return analyticsData
      }))
      .subscribe(analyticsData => {
          this.topTiesCategoriesData = analyticsData.filter(x => x.catcode >= 'A' && x.catcode <= 'Z')

          this.topTiesCategoriesData.forEach(topCategoryData => {
            this.transcationService.getCategories(topCategoryData.catcode).subscribe((lowerCategoriesData : CategoryView[]) => {
              var matTableData =  new MatTableDataSource<SingleCategoryAnalyticsView>();
              matTableData.data = analyticsData.filter(ac => {
                var haslowerCategoriesData : (CategoryView | undefined) = lowerCategoriesData.find(lc => lc.code == ac.catcode)
                return haslowerCategoriesData != undefined
              })

              // add other row
              if (matTableData.data.length > 0){
                var total_amount = topCategoryData.amount
                var total_count = topCategoryData.count
  
                var subcat_count = matTableData.data.map((x : SingleCategoryAnalyticsView) => x.count).reduce((accumulator, currentValue) => accumulator + currentValue)
                var subcat_amount = matTableData.data.map((x : SingleCategoryAnalyticsView) => x.amount).reduce((accumulator, currentValue) => accumulator + currentValue)

                var other : SingleCategoryAnalyticsView = {catcode : topCategoryData.catcode, amount : (total_amount - subcat_amount), count : (total_count - subcat_count)}
                if (subcat_count != topCategoryData.count){
                  matTableData.data.push(other)
                  matTableData._updateChangeSubscription()
                }
              }

              matTableData.data.sort((x1, x2) => {
                if (x1.amount > x2.amount){
                  return -1;
                } else {
                  return 1;
                }
              })
              this.dataSourceMap.set(topCategoryData.catcode, matTableData )
            })
          });
  
          this.topTiesCategoriesData.sort((x1, x2) => {
            if (x1.amount > x2.amount){
              return -1;
            } else{
              return 1;
            }       
          }) 
      })
      
  } 

  back(){
    this.location.back();
  }

  ngOnInit(): void {
    this.transcationService.getCategories().subscribe((categoriesData : CategoryView[]) => {
      categoriesData.forEach(category => {
        this.categoriesViewMap.set(category.code, category.name)
      })
    })
  }
}



interface State {
  startDate : Date,
  endDate : Date,
  navigationId : number
}
