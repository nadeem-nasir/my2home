import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators, AbstractControl, ValidatorFn, FormArray } from "@angular/forms";
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/delay';
import "rxjs/add/operator/takeUntil";
import { Subject } from 'rxjs/Subject';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { HostelService,dashboardService, NotificationService } from '../../services'
import { IaccountModels,  IDashboardModels, IPagedResult, SelectItem } from '../../models'
import * as Moment from "moment";
import { Location } from '@angular/common';

@Component({
  selector: 'ngx-dashboard',
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent implements OnInit, OnDestroy {
  private ngUnsubscribe: Subject<void> = new Subject<void>();

  errorMessage: string;
  hostelDropDownList: IPagedResult<SelectItem>;
  dashboardData: IDashboardModels;
  selectedHostelId: number = 0;
  searchCondition: Date = new Date();;
  currentMonthAccountData: IaccountModels;
  previousMonthAccountData: IaccountModels;
  constructor(private dashboardService: dashboardService,
    private toastrService: NotificationService,
    private hostelService: HostelService, private location: Location)
  {
    //get hostel dropdown list
    hostelService.getHostelDropDownByOrganizationIdAsync().subscribe(data => {
      this.hostelDropDownList = data;
      //console.log(this.hostelDropDownList)
      if (this.hostelDropDownList != null && this.hostelDropDownList != undefined) {
        this.selectedHostelId = this.hostelDropDownList.results[0].value;
        //let searchCondition = Moment(this.searchCondition).format("YYYY-MM-DDTHH:mm:ss.SSS[Z]");

        this.getDashboard();
        this.getCurrentMonthlyAccountPageListAsync();
        this.getPreviousMonthlyAccountPageListAsync();
      }
    }, error => {
      this.toastrService.showDanger("Hostel", <any>error)
      });


    
  }
  ngOnInit() {

    

  }
  ngOnDestroy() {
    // If subscribed, we must unsubscribe before Angular destroys the component.
    // Failure to do so could create a memory leak.
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  onShowDashboardChange(event)
  {
    this.searchCondition =  new Date();;
   // let searchCondition = Moment(this.searchCondition).format("YYYY-MM-DDTHH:mm:ss.SSS[Z]");
    this.getDashboard();
    this.getCurrentMonthlyAccountPageListAsync();
    this.getPreviousMonthlyAccountPageListAsync();
  }

  private getDashboard()
  {
    this.dashboardService.getDashboardAsync(this.selectedHostelId).subscribe(data => {
     // console.log(data);
      this.dashboardData = data;
      //console.log(this.countryList)
    }, error => {
      this.toastrService.showDanger("Dashboard", <any>error)
    })

  }

  private getCurrentMonthlyAccountPageListAsync()
  {
    let searchCondition = Moment(this.searchCondition).format("YYYY-MM-DDTHH:mm:ss.SSS[Z]");

    this.dashboardService.getMonthlyAccountPageListAsync(this.selectedHostelId, searchCondition).subscribe(data => {

      //console.log('currentMonthAccountData');
      //console.log(data);
      this.currentMonthAccountData = data;
      //console.log(this.countryList)
    }, error => {
      this.toastrService.showDanger("Dashboard", <any>error)
    })

  }


  private getPreviousMonthlyAccountPageListAsync()
  {
    this.searchCondition.setDate(0);
    let searchCondition = Moment(this.searchCondition).format("YYYY-MM-DDTHH:mm:ss.SSS[Z]");

    this.dashboardService.getMonthlyAccountPageListAsync(this.selectedHostelId, searchCondition).subscribe(data => {     
     // console.log('previousMonthAccountData');
     // console.log(data);

      this.previousMonthAccountData = data;
      //console.log(this.countryList)
    }, error => {
      this.toastrService.showDanger("Dashboard", <any>error)
    })
  }


  goBack(): void {
    this.location.back();
  }

}
