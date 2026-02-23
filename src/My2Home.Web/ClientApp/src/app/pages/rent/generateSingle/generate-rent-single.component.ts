import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { FormGroup, FormBuilder, Validators} from "@angular/forms";
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/delay';
import "rxjs/add/operator/takeUntil";
import { Subject } from 'rxjs/Subject';
import { RentService, NotificationService, HostelService, TenantService} from '../../../services'
import { IRentModels, IPagedResult, SelectItem } from '../../../models'
import * as Moment from "moment";

@Component({
  selector: 'generate-rent-single',
  templateUrl: './generate-rent-single.component.html',
  styles: [`
    nb-card {
      transform: translate3d(0, 0, 0);
    }
  `],
})
export class GenerateRentSingleComponent implements OnInit, OnDestroy {
  private ngUnsubscribe: Subject<void> = new Subject<void>();
  createForm: FormGroup;

  public rentModel: IRentModels;
  public tableList: IPagedResult<IRentModels>;
  errorMessage: string;
  public hostelDropDownList: IPagedResult<SelectItem>;
  public tenantDropDownList: IPagedResult<SelectItem>;
  public rentStatusList: SelectItem[];
  //selectedRoomHostelId: number = 0;
  constructor(private fb: FormBuilder,
    private location: Location,
    private toastrService: NotificationService,
    private rentService: RentService,
    private hostelService: HostelService,
    private tenantService: TenantService)
  {
    //get hostel dropdown list
    this.createForm = this.fb.group(
      {
        rentId: 0,
        rentMonth:'',
        rentYear: 0,
        rentStatus: ['', [Validators.required]],
        rentAmount: 0,
        rentBedNumber: 0,
        rentCreationDate: new Date(),
        rentDueDateTime: new Date(),
        rentDateTime: ['', [Validators.required]], 
        rentHostelId: ['', [Validators.required]], 
        rentTenantId: 0,        
      });

    hostelService.getHostelDropDownByOrganizationIdAsync().subscribe(data =>
    {
      this.hostelDropDownList = data;
      
    }, error =>
      {
        this.toastrService.showDanger("Hostel", <any>error)
      })

    this.rentStatusList = rentService.getRentStatus();

   

    

  }
  onTenantChange(event)
  {
    this.createForm.patchValue
      ({
        rentTenantId: event.value
      });
  }

  onHostelChangeSingle(event)
  {
    //console.log(event);
    this.createForm.patchValue
      ({
        rentHostelId: event.value
      });

    //Get tenant list
    this.tenantService.getTenantDropDownListAsync(event.value).subscribe(data => {
      this.tenantDropDownList = data;
      if (this.tenantDropDownList != null)
      {
        //console.log(this.tenantDropDownList.results[0]);
        //this.createForm.patchValue({
        //  rentTenantId: this.tenantDropDownList.results[0].value
        //});
      }       
    }, error => {
      this.toastrService.showDanger("tenant", <any>error)
      })
  }

  //onHostelChangeMultiple(event)
  //{
  //  this.createForm.patchValue
  //    ({
  //    rentHostelId: event.value
  //     });
  //}

  ngOnInit() {


    if (this.rentStatusList != null)
    {
      this.createForm.patchValue({
        rentStatus: this.rentStatusList[0].value
      });
    }
  }
  ngOnDestroy() {
    // If subscribed, we must unsubscribe before Angular destroys the component.
    // Failure to do so could create a memory leak.
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  } 

  createSingleRent(): void {

    this.rentModel = this.createForm.value as IRentModels;
    //console.log(this.rentModel);
    
    //console.log(this.rentModel.rentTenantId <= 0);
    if (this.rentModel.rentTenantId <= 0)
    {
      this.rentModel.rentTenantId = this.tenantDropDownList.results[1].value
    }
    
      this.rentModel.rentCreationDate = Moment(new Date()).format("YYYY-MM-DDTHH:mm:ss.SSS[Z]");
    
    if (this.rentModel.rentDueDateTime != null)
    {
      this.rentModel.rentDueDateTime = Moment(this.rentModel.rentDueDateTime).format("YYYY-MM-DDTHH:mm:ss.SSS[Z]");
    }    
    //console.log(this.createForm.value)
    //console.log(this.rentModel);
    if (this.rentModel.rentDateTime != null) {
      this.rentModel.rentDateTime = Moment(this.rentModel.rentDateTime).format("YYYY-MM-DDTHH:mm:ss.SSS[Z]");
    }
    //return;
    this.rentService.Create(this.rentModel).subscribe(data => {
      this.toastrService.showSuccess('Rent', 'Your data has been successfully saved');
      this.createForm.reset();//reset forms
      this.createForm.patchValue({
        rentId: 0,
        rentMonth: '',
        rentYear: 0,
        rentStatus: this.rentStatusList[0].value,
        rentAmount: 0,
        rentBedNumber: 0,
        rentCreationDate: new Date(),
        rentDueDateTime: new Date(),
        rentDateTime: new Date() ,
        rentHostelId: new Date(),
        rentTenantId: 0,    
      })
    }, error => {
      this.toastrService.showDanger("Rent", <any>error)
    })


  }

  //createMultiple(): void {
  //  this.rentModel = this.createForm.value as IRentModels;
  //  //console.log(this.createForm.value)
  //  //console.log(this.rentModel);
  //  this.rentModel.rentCreationDate = Moment(new Date()).format("YYYY-MM-DDTHH:mm:ss.SSS[Z]");

  //  if (this.rentModel.rentMonth != null)
  //  {
  //    this.rentModel.rentMonth = Moment(this.rentModel.rentMonth).format("YYYY-MM-DDTHH:mm:ss.SSS[Z]");
  //  }
  //  //return;
  //  this.rentService.Create(this.rentModel).subscribe(data => {
  //    this.toastrService.showSuccess('Room', 'Your data has been successfully saved');
  //    this.createForm.reset();//reset forms
  //  }, error => {
  //    this.toastrService.showDanger("Room", <any>error)
  //  })

  //}

  


  goBack(): void {
    this.location.back();
  }

  onHostelChange(event) {
    setTimeout(() => {
      this.rentService.getPageList(event.value, 1,5).takeUntil(this.ngUnsubscribe)
        .subscribe(data => {
          this.tableList = data;
         // console.log(data);
        },
          error => this.errorMessage = <any>error);
    }, 250);   
  }
  
}
