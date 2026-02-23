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
import { NotificationService, TenantService, HostelService, BedService } from '../../../services'
import { ITenantModels, IPagedResult, SelectItem } from '../../../models'

@Component({
  selector: 'tenant-list',
  templateUrl: './tenant-list.component.html',
  styles: [`
    nb-card {
      transform: translate3d(0, 0, 0);
    }
  `],
})
export class TenantListComponent implements OnInit, OnDestroy {
  private ngUnsubscribe: Subject<void> = new Subject<void>();
  createForm: FormGroup;
  public tenantyModel: ITenantModels;
  public tableList: IPagedResult<ITenantModels>; 
  errorMessage: string;
  public hostelDropDownList: IPagedResult<SelectItem>;
  dialogToCreate: boolean;
  selectedTenantHostelId: number = 0;
  //source: LocalDataSource = new LocalDataSource();
  rowsPerPage: number;
  rowsPerPageOptions: number[] = [];

  constructor(private fb: FormBuilder, private location: Location,
    private toastrService: NotificationService,
    private tenantService: TenantService,
    private hostelService: HostelService,
    private bedService :BedService) {
    //const data = this.service.getData();
    //this.source.load(data);

    this.rowsPerPage = tenantService.rowsPerPage;
    this.rowsPerPageOptions = tenantService.rowsPerPageOptions;


    //get hostel dropdown list
    hostelService.getHostelDropDownByOrganizationIdAsync().subscribe(data => {
      this.hostelDropDownList = data;
      //console.log(this.hostelDropDownList)
      if (this.hostelDropDownList != null && this.hostelDropDownList != undefined) {
        this.selectedTenantHostelId = this.hostelDropDownList.results[0].value;
      }
    }, error => {
        this.toastrService.showDanger("Hostel", <any>error)
      })
  }

  ngOnInit() {
    //this.createForm = this.fb.group(
    //  {
    //    tenantId: 0,
    //    tenantName: ['', [Validators.required, Validators.minLength(3)]],
    //    tenantMobile: 0,
    //    tenantEmail: '',
    //    tenantIDType: '',
    //    tenantHomeAddress: '',
    //    tenantEmergencyPerson: '',
    //    tenantEmergencyContact: '',
    //    tenantBedId: '',
    //    tenantProfession: '',
    //    tenantWorkInstitutionAddress: '',
    //    tenantStatus: '',
    //    tenantExtraNote: '',
    //    tenantUpdatedOn: '',
    //    tenantAdvanceDepositSecurity: '',
    //    tenantVehicleNumber: '',
    //    activeInActive: 0,
    //    tenantIdentityUserId: '',
    //    tenantHostelId:'',
    //  });
  }
  ngOnDestroy() {
    // If subscribed, we must unsubscribe before Angular destroys the component.
    // Failure to do so could create a memory leak.
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
   

  OnloadListLazy(event: LazyLoadEvent) {
    let pageIndex = event.first / event.rows + 1;
    setTimeout(() => {
      this.tenantService.getPageList(this.selectedTenantHostelId, pageIndex, event.rows).takeUntil(this.ngUnsubscribe)
        .subscribe(data => {
          this.tableList = data;
          //console.log(data);
        },
          error => this.errorMessage = <any>error);
    }, 250);
  }

  onHostelChange(event) {
    setTimeout(() => {
      this.tenantService.getPageList(this.selectedTenantHostelId, 1, this.rowsPerPage).takeUntil(this.ngUnsubscribe)
        .subscribe(data => {
          this.tableList = data;
         // console.log(data);
        },
          error => this.errorMessage = <any>error);
    }, 250);
  }
  onRowSelect(event) {
    //this.newCar = false;
    //this.car = this.cloneCar(event.data);
    this.dialogToCreate = true;
    //console.log(event);
  }
  showDialogToCreate() {

    this.dialogToCreate = true;

  }
  OnDelete(tableRow) {
    if (window.confirm('Are you sure you want to delete?')) {
      this.tenantService.deleteById(tableRow.tenantId).takeUntil(this.ngUnsubscribe)
        .subscribe(data =>
        {
          this.tableList.results = this.tableList.results.filter(item => item.tenantId != tableRow.tenantId);
          this.toastrService.showSuccess('Tenant', 'Your data has been successfully deleted');
        },
        error =>
        {
            this.toastrService.showDanger("Tenant", <any>error);
          });
    }
  }
  goBack(): void {
    this.location.back();
  }

  //onDeleteConfirm(event): void {
  //  if (window.confirm('Are you sure you want to delete?')) {
  //    event.confirm.resolve();
  //  } else {
  //    event.confirm.reject();
  //  }
  //}
}

