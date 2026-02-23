import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { BaseDataService } from './BaseDataService'
import { RegisterModel } from '../models';
import { OAuthService } from 'angular-oauth2-oidc';
import { JwtHelper } from 'angular2-jwt';
import { Router, ActivatedRoute } from '@angular/router';
import { catchError } from 'rxjs/operators';
@Injectable({ providedIn: 'root' })
export class AuthenticationService extends BaseDataService 
{
  public jwtHelper: JwtHelper = new JwtHelper();
  constructor(private http: HttpClient, private oAuthService: OAuthService,
    public router: Router, public route: ActivatedRoute)
  {
    super(); 
  }

  public get isLoggedIn(): boolean
  {
    return this.oAuthService.hasValidAccessToken();
  }
  public logout() {
    this.http.post('api/account/logout', null).subscribe((res) =>
    {
      this.oAuthService.logOut();
      //this.utilityService.navigateToSignIn();
    });
  }
  public get accessToken(): string
  {
    return this.oAuthService.getAccessToken();
  }
  // Used to access user information
  public get idToken(): string
  {
    return this.oAuthService.getIdToken();
  }

  resetPasswordInit(email: string)
  {
    return this.http.post<any>("/api/account/forgotpassword", { "email": email });
  }

  resetpassword(model:any)
  {
    return this.http.post<any>("/api/account/resetpassword", model);
  }
  //others method

  public register(model: RegisterModel): Observable<any> 
  {
      return this.http.post('api/account/register', model).pipe(
        catchError(this.handleError));          
  }


}

//.subscribe((res: Response) =>
      //{
      //  //auth/register-confirmation
      //  this.router.navigate(['../registerconfirmation'], { relativeTo: this.route });        
      //});
