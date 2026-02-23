import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators, AbstractControl, ValidatorFn, FormArray } from "@angular/forms";
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/delay';
import "rxjs/add/operator/takeUntil";
import { Subject } from 'rxjs/Subject';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { RentService, NotificationService, HostelService} from '../../../services'
import { IRoomModels, IPagedResult, SelectItem, IRentModels } from '../../../models'
import * as Moment from "moment";

@Component({
  selector: 'rent-list-yearly',
  templateUrl: './rent-list-yearly.component.html',
  styles: [`
    nb-card {
      transform: translate3d(0, 0, 0);
    }
  `],
})
export class RentListYearlyComponent implements OnInit, OnDestroy {
  private ngUnsubscribe: Subject<void> = new Subject<void>();
  createForm: FormGroup;

  //public roomModel: IRoomModels;
  public tableList: IPagedResult<IRentModels>;
  errorMessage: string;
  public hostelDropDownList: IPagedResult<SelectItem>;
  selectedRoomHostelId: number = 0;
  dialogToCreate: boolean;
  searchCondition: Date = new Date();;
  rowsPerPage: number;
  rowsPerPageOptions: number[] = [];
  searchType: boolean = false;
  constructor(private fb: FormBuilder, private location: Location,
    private toastrService: NotificationService,
    private rentService: RentService,

    private hostelService: HostelService)
  {

    this.rowsPerPage = rentService.rowsPerPage;
    this.rowsPerPageOptions = rentService.rowsPerPageOptions;


    //get hostel dropdown list
    hostelService.getHostelDropDownByOrganizationIdAsync().subscribe(data =>
    {
      this.hostelDropDownList = data;
      console.log(this.hostelDropDownList)
      if (this.hostelDropDownList != null && this.hostelDropDownList != undefined)
      {
        this.selectedRoomHostelId = this.hostelDropDownList.results[0].value;
      }
    }, error =>
      {
        this.toastrService.showDanger("Hostel", <any>error)
      })
  }

  ngOnInit() {

    
  }
  ngOnDestroy() {
    // If subscribed, we must unsubscribe before Angular destroys the component.
    // Failure to do so could create a memory leak.
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }


  

  OnloadListLazy(event: LazyLoadEvent) {
    let pageIndex = event.first / event.rows + 1;
    let searchCondition = Moment(this.searchCondition).format("YYYY-MM-DDTHH:mm:ss.SSS[Z]");

    console.log(searchCondition);
    setTimeout(() => {
      this.rentService.getPageList(this.selectedRoomHostelId, pageIndex, event.rows, searchCondition, this.searchType).takeUntil(this.ngUnsubscribe)
        .subscribe(data => {
          this.tableList = data;
          console.log('rent list')
          console.log(data);
        },
          error => this.errorMessage = <any>error);
    }, 250);

  } 

  //onMonthSelect(event)
  //{

  //  let searchCondition = Moment(event).format("YYYY-MM-DDTHH:mm:ss.SSS[Z]");
  //  console.log(searchCondition);

  //  setTimeout(() => {
  //    this.rentService.getPageList(this.selectedRoomHostelId, 1, this.rowsPerPage, searchCondition).takeUntil(this.ngUnsubscribe)
  //      .subscribe(data => {
  //        this.tableList = data;
  //        console.log('rent list')
  //        console.log(data);
  //      },
  //        error => this.errorMessage = <any>error);
  //  }, 250);

  //}

  OnDelete(tableRow)
  {
    if (window.confirm('Are you sure you want to delete?')) {
      this.rentService.deleteById(tableRow.rentId).takeUntil(this.ngUnsubscribe)
        .subscribe(data => {
          this.tableList.results = this.tableList.results.filter(item => item.rentId != tableRow.rentId);
          this.toastrService.showSuccess('Rent', 'Your data has been successfully deleted');
        },
          error => {
            this.toastrService.showDanger("Rent", <any>error);
          });
    }
  }
  goBack(): void {
    this.location.back();
  }

  //onHostelChange(event) {
  //  setTimeout(() => {
  //    this.rentService.getPageList(this.selectedRoomHostelId, 1, this.rowsPerPage).takeUntil(this.ngUnsubscribe)
  //      .subscribe(data => {
  //        this.tableList = data;
  //        console.log(data);
  //      },
  //        error => this.errorMessage = <any>error);
  //  }, 250);   
  //}

  OnShowRentClick(event) {
    console.log(event);
    console.log('show click')
    console.log(this.searchCondition)
    console.log(this.selectedRoomHostelId);
    let searchCondition = Moment(this.searchCondition).format("YYYY-MM-DDTHH:mm:ss.SSS[Z]");
    setTimeout(() => {
      this.rentService.getPageList(this.selectedRoomHostelId, 1, this.rowsPerPage, searchCondition, this.searchType).takeUntil(this.ngUnsubscribe)
        .subscribe(data => {
          this.tableList = data;
          console.log('rent list')
          console.log(data);
        },
          error => this.errorMessage = <any>error);
    }, 250);
  }
}

