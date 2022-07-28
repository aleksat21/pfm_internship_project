import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Route } from '@angular/router';
import { formatDate, Location } from '@angular/common';
import { TransactionsFacadeService } from 'src/app/domain/application-services/transactions-facade.service';
import { SingleCategoryAnalyticsView } from 'src/app/domain/models/GetAnalyticsModel/SingleCategoryAnalyticsView';
import { IGetCategoriesResponse } from 'src/app/domain/models/GetCategoriesModels/IGetCategoriesResponse';
import { CategorizeView } from 'src/app/domain/models/PostCategorizeModels/CategorizeView';
import { CategoryView } from 'src/app/domain/models/GetCategoriesModels/CategoryView';
import { MatTableDataSource } from '@angular/material/table';


@Component({
  selector: 'app-analytics',
  templateUrl: './analytics.component.html',
  styleUrls: ['./analytics.component.css']
})
export class AnalyticsComponent implements OnInit {

  public displayedColumns : string[] = ['catcode', 'count', 'amount']
  public dataSource = new MatTableDataSource<SingleCategoryAnalyticsView>()
  public dataSourceMap = new Map<string, MatTableDataSource<SingleCategoryAnalyticsView>>()

  public categoriesViewMap : Map<string, string> = new Map<string, string>()

  startDateQuery : Date
  endDateQuery : Date

  startDateString : string = ""
  endDateString : string = "-"

  topTiesCategoriesData : SingleCategoryAnalyticsView[] = []


  constructor(private location : Location,
              private transcationService : TransactionsFacadeService
  )
  {}

  ngOnInit(): void {
    var data  = this.location.getState() as State
    this.startDateQuery = data.startDate
    this.endDateQuery = data.endDate

    if (this.startDateQuery != undefined){
      this.startDateString = formatDate(this.startDateQuery, 'dd-MM-yyyy', 'en-US')
    }

    if (this.endDateQuery != undefined){
      this.endDateString = formatDate(this.endDateQuery, 'dd-MM-yyyy', 'en-US')
    }

    this.transcationService.getCategories().subscribe((categoriesData : CategoryView[]) => {
      
      categoriesData.forEach(category => {
        this.categoriesViewMap.set(category.code, category.name)
      })

      this.transcationService.getAnalyticsData().subscribe(analyticsData => {
        this.topTiesCategoriesData = analyticsData.filter(x => x.catcode >= 'A' && x.catcode <= 'Z')

        this.topTiesCategoriesData.forEach(topCategoryData => {
          this.transcationService.getCategories(topCategoryData.catcode).subscribe((lowerCategoriesData : CategoryView[]) => {
            var matTableData =  new MatTableDataSource<SingleCategoryAnalyticsView>();
            matTableData.data = analyticsData.filter(ac => {
              var haslowerCategoriesData : (CategoryView | undefined) = lowerCategoriesData.find(lc => lc.code == ac.catcode)
              return haslowerCategoriesData != undefined
            })
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
    })
  }

}

interface State {
  startDate : Date,
  endDate : Date,
  navigationId : number
}
