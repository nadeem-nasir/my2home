import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators, AbstractControl, ValidatorFn, FormArray } from "@angular/forms";
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/delay';
import "rxjs/add/operator/takeUntil";
import { Subject } from 'rxjs/Subject';
//import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { NotificationService, TenantService, HostelService, BedService } from '../../../services'
import { ITenantModels, IPagedResult, SelectItem } from '../../../models'

@Component({
  selector: 'tenant-create-edit',
  templateUrl: './tenant-create-edit.component.html',
  styles: [`
    nb-card {
      transform: translate3d(0, 0, 0);
    }
  `],
})
export class TenantCreateEditComponent implements OnInit, OnDestroy {
  private ngUnsubscribe: Subject<void> = new Subject<void>();
  createForm: FormGroup;
  public tenantyModel: ITenantModels;
  errorMessage: string;
  public hostelDropDownList: IPagedResult<SelectItem>;
  public bedDropDownList: SelectItem[];
  public tenantStatus: SelectItem[];
  //source: LocalDataSource = new LocalDataSource();
  constructor(private fb: FormBuilder, private location: Location,
    private toastrService: NotificationService,
    private tenantService: TenantService,
    private hostelService: HostelService,
    private bedService: BedService,
    private route: ActivatedRoute) {
        //get hostel dropdown list

    this.createForm = this.fb.group(
      {
        tenantId: 0,
        tenantName: ['', [Validators.required]],
        tenantMobile: '',
        tenantEmail: '',
        tenantIDType: '',
        tenantHomeAddress: '',
        tenantEmergencyPerson: '',
        tenantEmergencyContact: '',
        tenantBedId: ['', [Validators.required]],
        tenantProfession: '',
        tenantWorkInstitutionAddress: '',
        tenantStatus: '',
        tenantExtraNote: '',
        
        tenantAdvanceDepositSecurity: 0,
        tenantVehicleNumber: '',
        
        tenantIdentityUserId: 0,
        tenantHostelId: 0,
      });


    hostelService.getHostelDropDownByOrganizationIdAsync().subscribe(data => {
      this.hostelDropDownList = data;
      console.log(this.hostelDropDownList)
      this.getBedDropDownByHostelIdAsync(this.hostelDropDownList.results[0].value);

      this.createForm.patchValue
        ({
          tenantHostelId: this.hostelDropDownList.results[0].value,
        });

    }, error => {
      this.toastrService.showDanger("Hostel", <any>error)
    })

    this.tenantStatus = this.tenantService.getTenantStatus();
    this.createForm.patchValue
      ({
        tenantStatus: this.tenantStatus[0].value
      });
  }

  private patchDefaultValue()
  {
    this.createForm.patchValue
      ({
        tenantId: 0,      
        tenantIdentityUserId: 0,
        tenantHostelId: 0,
      });

  }
  ngOnInit() {

    



    this.route.queryParams.subscribe(params => {
      var tenantId = Number.parseInt(params['Id']);
      if (tenantId != NaN && tenantId > 0) {
        this.tenantService.getById(tenantId).subscribe(data => {
          this.createForm.patchValue
            ({
              tenantId: data.tenantId,
              tenantName: data.tenantName,
              tenantMobile: data.tenantMobile,
              tenantEmail: data.tenantEmail,
              tenantIDType: data.tenantIDType,
              tenantHomeAddress: data.tenantHomeAddress,
              tenantEmergencyPerson: data.tenantEmergencyPerson,
              tenantEmergencyContact: data.tenantEmergencyContact,
              tenantBedId: data.tenantBedId,
              tenantProfession: data.tenantProfession,
              tenantWorkInstitutionAddress: data.tenantWorkInstitutionAddress,
              tenantStatus: data.tenantStatus,
              tenantExtraNote: data.tenantExtraNote,
              
              tenantAdvanceDepositSecurity: data.tenantAdvanceDepositSecurity,
              tenantVehicleNumber: data.tenantVehicleNumber,
              
              tenantIdentityUserId: data.tenantIdentityUserId,
              tenantHostelId: data.tenantHostelId,
            });
        }, error => {
          this.toastrService.showDanger("Tenant", <any>error)
        });
      }
    });
  }
  ngOnDestroy() {
    // If subscribed, we must unsubscribe before Angular destroys the component.
    // Failure to do so could create a memory leak.
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  getBedDropDownByHostelIdAsync(hostelId: any) {
    this.bedService.getBedDropDownByHostelIdAsync(hostelId).subscribe(data => {
      this.bedDropDownList = data;
      //console.log(this.hostelDropDownList)
    }, error => {
      this.toastrService.showDanger("Bed", <any>error)
    })
  }
  create(): void {

    this.tenantyModel = this.createForm.value as ITenantModels;
    console.log(this.tenantyModel);
    if (this.tenantyModel.tenantId <= 0)
    {
      this.tenantService.Create(this.tenantyModel).subscribe(data => {
        this.toastrService.showSuccess('Tenant', 'Your data has been successfully saved');
        this.createForm.reset();//reset forms
        this.bedDropDownList = this.bedDropDownList.filter(item => item.value !== this.tenantyModel.tenantBedId);
        this.patchDefaultValue();
      }, error => {
        this.toastrService.showDanger("Tenant", <any>error)
      })
    }
    else {
      this.tenantService.updateById(this.tenantyModel).subscribe(data => {
        this.toastrService.showSuccess('Tenant', 'Your data has been successfully saved');

        //this.createForm.reset();//reset forms

      }, error => {
        this.toastrService.showDanger("Tenant", <any>error)
      })
    }
  }

  onHostelChange(event) {
    this.getBedDropDownByHostelIdAsync(event.value);
  }

  goBack(): void {
    this.location.back();
  }

  
}

