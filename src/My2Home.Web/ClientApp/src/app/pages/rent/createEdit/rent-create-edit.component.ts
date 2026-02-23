import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators, AbstractControl, ValidatorFn, FormArray } from "@angular/forms";
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/delay';
import "rxjs/add/operator/takeUntil";
import { Subject } from 'rxjs/Subject';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { NotificationService, RentService , HostelService } from '../../../services'
import {IPagedResult , SelectItem,  IRentModels} from '../../../models'
import * as Moment from "moment";
@Component({
  selector: 'rent-create-edit',
  templateUrl: './rent-create-edit.component.html',
  styles: [`
    nb-card {
      transform: translate3d(0, 0, 0);
    }
  `],
})
export class RentCreateEditComponent implements OnInit, OnDestroy {
  private ngUnsubscribe: Subject<void> = new Subject<void>();
  createForm: FormGroup;
  public hostelDropDownList: IPagedResult<SelectItem>;
  public rentStatusList: SelectItem[];
  //public expenseCategoryDropDownList: IPagedResult<SelectItem>;
  //public selectedModel: IExpenseModels;
  //public tableList: IPagedResult<IExpenseModels>;
  //dialogToCreate: boolean;
  public createModel: IRentModels;
  selectedHostelId: number = 0;
  //selectedexpenseId: number = 0;
  errorMessage: string;
  
  constructor(private fb: FormBuilder, private location: Location,
   // private expenseCategoryService: ExpenseCategoryService,
    private hostelService: HostelService,
    private rentService: RentService,
    private toastrService: NotificationService,
    private route: ActivatedRoute)
  {
    //get hostel dropdown list

    //this.selectedexpenseId = Number.parseInt(this.route.snapshot.paramMap.get('Id'));

    //console.log(this.selectedexpenseId);

    this.hostelService.getHostelDropDownByOrganizationIdAsync().subscribe(data => {
      this.hostelDropDownList = data;
      if (this.hostelDropDownList != null && this.hostelDropDownList != undefined) {
        this.createForm.patchValue({
          rentHostelId: this.hostelDropDownList.results[0].value
        });

        this.selectedHostelId = this.hostelDropDownList.results[0].value;
      }
     // console.log(this.hostelDropDownList)
    }, error => {
      this.toastrService.showDanger("Hostel", <any>error)
      })

    this.rentStatusList = rentService.getRentStatus();
    
    
  }

  ngOnInit() {

    this.createForm = this.fb.group(
      {
        rentId: 0,
        rentMonth: ['', [Validators.required]],
        rentYear: 0,
        rentStatus: ['', [Validators.required]],
        rentAmount: 0,
        rentBedNumber: 0,
        rentCreationDate: '',
        rentDueDateTime: '',
        rentDateTime: ['', [Validators.required]], 
        rentHostelId: ['', [Validators.required]],
        rentTenantId: 0,
        rentTenantName:''
      });
         
    this.route.queryParams.subscribe(params => {
      var rentId = Number.parseInt(params['Id']);
      console.log(rentId);
      if (rentId > 0) {
        this.rentService.getById(rentId).subscribe(data =>
        {
         // console.log('rent');
         // console.log(data);
          this.createForm.patchValue
            ({
              rentId: data.rentId,
              rentMonth:data.rentMonth,
              rentYear: data.rentYear,
              rentStatus: data.rentStatus,
              rentAmount: data.rentAmount,
              rentBedNumber: data.rentBedNumber,
              rentCreationDate: data.rentCreationDate ? Moment(data.rentCreationDate).toDate() : '',
              rentDueDateTime: data.rentDueDateTime ? Moment(data.rentDueDateTime).toDate() : '',
              rentDateTime: data.rentDateTime ? Moment(data.rentDateTime).toDate() : '',
              rentTenantId: data.rentTenantId,
              rentTenantName: data.rentTenant.tenantName
            });
        }, error => {
            this.toastrService.showDanger("Rent", <any>error)
          })
      }
    });
     

  }
  ngOnDestroy() {
    // If subscribed, we must unsubscribe before Angular destroys the component.
    // Failure to do so could create a memory leak.
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
  
  goBack(): void {
    this.location.back();
  }


  create(): void
  {

    this.createModel = this.createForm.value as IRentModels;
   // console.log(this.createModel);

    if (this.createModel.rentDueDateTime != null) {
      this.createModel.rentDueDateTime = Moment(this.createModel.rentDueDateTime).format("YYYY-MM-DDTHH:mm:ss.SSS[Z]");
    }
    
    if (this.createModel.rentCreationDate != null) {
      this.createModel.rentCreationDate = Moment(this.createModel.rentCreationDate).format("YYYY-MM-DDTHH:mm:ss.SSS[Z]");
    }

    if (this.createModel.rentDateTime != null) {
      this.createModel.rentDateTime = Moment(this.createModel.rentDateTime).format("YYYY-MM-DDTHH:mm:ss.SSS[Z]");
    }

    this.rentService.updateById(this.createModel).subscribe(data =>
    {
      this.toastrService.showSuccess('Rent', 'Your data has been successfully saved');
      //this.createForm.reset();//reset forms
    }, error => {
      this.toastrService.showDanger("Rent", <any>error)
    })

 }

}


  
  

  

  




