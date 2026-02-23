import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Inject } from '@angular/core';
import { Router,ActivatedRoute } from '@angular/router';
import { AuthenticationService } from '../../../services/authenticationService';
import { from } from 'rxjs';
import { OAuthService } from 'angular-oauth2-oidc';
import { JwksValidationHandler } from 'angular-oauth2-oidc';
import { authConfig } from '../../../../environments/environment'
//import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'nb-request-password-page',
  styleUrls: ['./request-password.component.scss'],
  templateUrl: './request-password.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NbRequestPasswordComponent {
   
  redirectDelay: number = 0;
  showMessages: any = {};
  strategy: string = '';
  submitted = false;
  errors: string[] = [];
  messages: string[] = [];
  user: any = {};
  loading: boolean = false;
  constructor(private authenticationService: AuthenticationService,
    public oauthService: OAuthService,
    private router: Router,
    private route: ActivatedRoute)
  {
    this.oauthService.setStorage(localStorage);
    this.oauthService.configure(authConfig);
    this.oauthService.tokenValidationHandler = new JwksValidationHandler();
   // console.log(authConfig);

  }

  requestPass(): void
  {
    //this.submitted = true;
    //this.showMessages.success = true;
    //this.messages = [];
    //this.messages.push("In a few moments, you will receive an email that contains a link to reset your password");

    //return;
    this.loading = true;

    //setTimeout(() => this.loading = false, 3000);

    //return;
    this.authenticationService.resetPasswordInit(this.user.email).subscribe(x =>
    {            
      //request-password-confirmation
      this.router.navigate(['../request-password-confirmation'], { relativeTo: this.route });        
      this.loading = false;
    });
  }
}
