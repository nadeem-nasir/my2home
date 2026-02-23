import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';
import {
  AuthenticationService, countryService, NotificationService,
  organizationStaffService} from '../../../services'
import { RegisterModel, IPagedResult, ICountryModels } from '../../../models';
import { FormGroup, FormBuilder, Validators, AbstractControl, ValidatorFn, FormArray } from "@angular/forms";
import { Location } from '@angular/common';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/delay';
import "rxjs/add/operator/takeUntil";
import { Subject } from 'rxjs/Subject';

@Component({
  selector: 'register-staff', 
  templateUrl: './register.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RegisterStaffComponent implements OnInit, OnDestroy {

 private ngUnsubscribe: Subject<void> = new Subject<void>();

  //createForm: FormGroup;
  errorMessage: string; 
  redirectDelay: number = 0;
  showMessages: any = {};
  strategy: string = '';
  submitted = false;
  errors: string[] = [];
  messages: string[] = [];
  user: any = {};

  constructor(private organizationStaffService: organizationStaffService,
    private location: Location,
    private router: Router,
    private route: ActivatedRoute,
    private toastrService: NotificationService)
  {

  }
  ngOnInit() { }
  ngOnDestroy() {
    // If subscribed, we must unsubscribe before Angular destroys the component.
    // Failure to do so could create a memory leak.
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  register(): void {
    var model = this.user as RegisterModel;
    //console.log(model);
    if (model == null)
    {
      return;
    }

    this.organizationStaffService.registerStaff(model).subscribe((res: Response) => {
      this.toastrService.showSuccess('Staff', 'Your data has been successfully Save');
      this.router.navigate(['../staff-list'], { relativeTo: this.route });
    }, error =>
      { 
        this.toastrService.showDanger("User", <any>error)
      });;
  }

  goBack(): void {
    this.location.back();
  }
}
