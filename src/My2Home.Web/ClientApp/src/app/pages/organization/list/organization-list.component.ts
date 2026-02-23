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
import { OrganizationService, NotificationService } from '../../../services'
import { IOrganizationModels, IPagedResult } from '../../../models'

@Component({
  selector: 'organization-list',
  templateUrl: './organization-list.component.html',
  styles: [`
    nb-card {
      transform: translate3d(0, 0, 0);
    }
  `],
})
export class OrganizationListComponent implements OnInit, OnDestroy {
  private ngUnsubscribe: Subject<void> = new Subject<void>();
  createForm: FormGroup;
  public model: IOrganizationModels;
  public tableList: IPagedResult<IOrganizationModels>;
  errorMessage: string;

  
  //source: LocalDataSource = new LocalDataSource();

  constructor(private fb: FormBuilder, private location: Location,
    private organizationService: OrganizationService, private toastrService: NotificationService) {
    //const data = this.service.getData();
    //this.source.load(data);
  }

  ngOnInit() {
    this.organizationService.getByIdentityUserId().subscribe(data => {
      this.model = data;
      //console.log(this.model);
      this.buildForm();    
    }, error => {
      this.toastrService.showDanger("Organization", <any>error)
    });
  }

  buildForm(): void {
    this.createForm = this.fb.group(
      {
        organizationId: this.model.organizationId,
        organizationName: [this.model.organizationName, [Validators.required, Validators.minLength(3)]],
        organizationAddress: this.model.organizationAddress,
        organizationPhoneNumber: this.model.organizationPhoneNumber
      });
  }
  ngOnDestroy() {
    // If subscribed, we must unsubscribe before Angular destroys the component.
    // Failure to do so could create a memory leak.
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
  create(): void
  {
    this.model = this.createForm.value as IOrganizationModels;
    this.organizationService.update(this.model).subscribe(data => {
      this.toastrService.showSuccess('Organization', 'Your data has been successfully saved');
      //this.createForm.reset();//reset forms
    }, error => {
      this.toastrService.showDanger("Organization", <any>error)
    })
  }
}


