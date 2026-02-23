import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { Location } from '@angular/common';
//import { Router } from '@angular/router';
//import { FormGroup, FormBuilder, Validators, AbstractControl, ValidatorFn, FormArray } from "@angular/forms";

import 'rxjs/add/operator/map';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/delay';
import "rxjs/add/operator/takeUntil";
import { Subject } from 'rxjs/Subject';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { dashboardService, NotificationService, HostelService } from '../../../services'
import { IaccountModels, IPagedResult, SelectItem } from '../../../models'
import * as Moment from "moment";
@Component({
  selector: 'account-list',
  templateUrl: './account-list.component.html',
  styles: [`
    nb-card {
      transform: translate3d(0, 0, 0);
    }
  `],
})
export class AccountListComponent implements OnInit, OnDestroy {
  private ngUnsubscribe: Subject<void> = new Subject<void>();


  public tableList: IPagedResult<IaccountModels>;
  
  errorMessage: string;
  searchCondition: Date = new Date();;
  selectedHostelId: number = 0;
  rowsPerPage: number;
  rowsPerPageOptions: number[] = [];

  public hostelDropDownList: IPagedResult<SelectItem>;
  ngOnInit()
  {
    
  }
  ngOnDestroy() {
    // If subscribed, we must unsubscribe before Angular destroys the component.
    // Failure to do so could create a memory leak.
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  constructor(private _dashboardService: dashboardService, private toastrService: NotificationService,
    private location: Location, private hostelService: HostelService)
  {
    this.rowsPerPage = _dashboardService.rowsPerPage;
    this.rowsPerPageOptions = _dashboardService.rowsPerPageOptions;


    //get hostel dropdown list
    hostelService.getHostelDropDownByOrganizationIdAsync().subscribe(data => {
      this.hostelDropDownList = data;
     // console.log(this.hostelDropDownList)
      if (this.hostelDropDownList != null && this.hostelDropDownList != undefined) {
        this.selectedHostelId = this.hostelDropDownList.results[0].value;
      }
    }, error => {
        this.toastrService.showDanger("Hostel", <any>error)
      })

  }



  OnloadListLazy(event: LazyLoadEvent) {
    let searchCondition = Moment(this.searchCondition).format("YYYY-MM-DDTHH:mm:ss.SSS[Z]");
    let pageIndex = event.first / event.rows + 1;

    setTimeout(() =>
    {
      this._dashboardService.getAccountPageListAsync(this.selectedHostelId, pageIndex, event.rows, searchCondition).takeUntil(this.ngUnsubscribe)
        .subscribe(data => {
          this.tableList = data;
          //console.log(data);
        },
          error => this.errorMessage = <any>error);
    }, 250);
  }

  onMonthSelect(event) {



  }

  goBack(): void
  {
    this.location.back();
  }

  onHostelChange(event) {
    let searchCondition = Moment(this.searchCondition).format("YYYY-MM-DDTHH:mm:ss.SSS[Z]");
    let pageIndex = event.first / event.rows + 1;

    setTimeout(() => {
      this._dashboardService.getAccountPageListAsync(this.selectedHostelId, pageIndex, event.rows, searchCondition).takeUntil(this.ngUnsubscribe)
        .subscribe(data => {
          this.tableList = data;
          //console.log(data);
        },
          error => this.errorMessage = <any>error);
    }, 250);
  }
}


