import { Component } from '@angular/core';
//import { NbLoginComponent } from '@nebular/auth';
import { OAuthService } from 'angular-oauth2-oidc';
import { JwksValidationHandler } from 'angular-oauth2-oidc';
import { authConfig} from '../../../../environments/environment'
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'ngx-login',
  templateUrl: './login.component.html',
})
export class NgxLoginComponent  {

  redirectDelay: number = 0;
  showMessages: any = {};
  //strategy: string = '';

  errors: string[] = [];
  messages: string[] = [];
  user: any = {};
  submitted: boolean = false;
 // socialLinks: NbAuthSocialLink[] = [];
  rememberMe = false;
  loading: boolean = false;
  constructor(public oauthService: OAuthService,
    private route: ActivatedRoute, private router: Router,)
  {
    this.oauthService.setStorage(localStorage);
    this.oauthService.configure(authConfig);
    this.oauthService.tokenValidationHandler = new JwksValidationHandler();
    //console.log(authConfig);
  }

  login(): void
  {
    this.loading = true;
    //console.log(this.user);
    this.oauthService.fetchTokenUsingPasswordFlow(this.user.email, this.user.password)
      .then((x: any) => {
        //console.log(x);

        //localStorage.setItem('id_token', x.refresh_token);

       // this.oauthService.setupAutomaticSilentRefresh();
        //this.oauthService.
        //localStorage.setItem('id_token', x.id_token);

        //this.oauthService.setupAutomaticSilentRefresh();

        //this.us.navigate('/dashboard');

        //this.us.navigate('/dashboard');
        //this.us.navigate('');
      
        this.router.navigate(['../dashboard'], { relativeTo: this.route });
        this.loading = false;
      })
      .catch(err =>{
        this.loading = false;

      });
  }
}
