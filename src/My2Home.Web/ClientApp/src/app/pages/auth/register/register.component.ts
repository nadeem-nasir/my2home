import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Inject, OnDestroy, OnInit } from '@angular/core';

import { Router, ActivatedRoute } from '@angular/router';

import { OAuthService } from 'angular-oauth2-oidc';
import { AuthenticationService, countryService, NotificationService} from '../../../services'
import { RegisterModel, IPagedResult, ICountryModels } from '../../../models';
import { FormGroup, FormBuilder, Validators, AbstractControl, ValidatorFn, FormArray } from "@angular/forms";
import { Location } from '@angular/common';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/delay';
import "rxjs/add/operator/takeUntil";
import { Subject } from 'rxjs/Subject';

@Component({
  selector: 'nb-register',
  styleUrls: ['./register.component.scss'],
  templateUrl: './register.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NbRegisterComponent implements OnInit, OnDestroy {

  private ngUnsubscribe: Subject<void> = new Subject<void>();
  //createForm: FormGroup;
  errorMessage: string;
  public countryList: IPagedResult<ICountryModels>;

  redirectDelay: number = 0;
  showMessages: any = {};
  strategy: string = '';

  submitted = false;
  errors: string[] = [];
  messages: string[] = [];
  user: any = {};
  loading:boolean = false;
 // socialLinks: NbAuthSocialLink[] = [];
  ngOnInit() {
    this.user.countryId = 162;

    //this.toastrService.showSuccess('hi', 'toaster');
    //this.toastrService.showDanger("hi" , "Danger")
    //this.createForm = this.fb.group(
    //  {
    //    email: ['', [Validators.required, Validators.minLength(3)]],
    //    password: 0,
    //    confirmPassword:0,
    //    fullName: 162,
    //    countryId:162
    //  });
  }
  ngOnDestroy() {
    // If subscribed, we must unsubscribe before Angular destroys the component.
    // Failure to do so could create a memory leak.
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  constructor(private authService: OAuthService,
    private authenticationService: AuthenticationService,
    private fb: FormBuilder, private location: Location,
    private countryService: countryService,
    private toastrService: NotificationService,
    public router: Router,
    public route: ActivatedRoute)
  {

    this.countryService.getCountryList().subscribe(data =>
    {
      this.countryList = data;
      //console.log(this.countryList)
    }, error => {
        this.toastrService.showDanger("City", <any>error)
      })
  }

  register(): void
  {
    this.loading = true;
    var model = this.user as RegisterModel;       
    this.authenticationService.register(this.user).subscribe((res: Response) =>
    {
        this.router.navigate(['../register-confirmation'], { relativeTo: this.route });
        //auth/register-confirmation
       // this.router.navigate(['../registerconfirmation'], { relativeTo: this.route });        
      this.loading = false;
    }, error =>
    {
      this.loading = false;
      this.toastrService.showDanger("User", <any>error)
    });;   
  }
}

//auth/register-confirmation
    //this.router.navigate(['/auth/register-confirmation'], { relativeTo: this.route, queryParams: { emailConfirmed: true } });      
    ///auth/register-confirmation
