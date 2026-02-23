import { throwError as observableThrowError, Observable, BehaviorSubject } from 'rxjs';
import { _throw } from "rxjs/observable/throw";
import { take, filter, catchError, switchMap, finalize } from 'rxjs/operators';
import { Injectable, Injector } from "@angular/core";
import { HttpInterceptor, HttpRequest, HttpHandler, HttpSentEvent, HttpHeaderResponse, HttpProgressEvent, HttpResponse, HttpUserEvent, HttpErrorResponse } from "@angular/common/http";
import { OAuthService } from 'angular-oauth2-oidc';
import { Router, ActivatedRoute } from '@angular/router';

@Injectable()
export class RefreshTokenInterceptor implements HttpInterceptor {

  isRefreshingToken: boolean = false;
  tokenSubject: BehaviorSubject<string> = new BehaviorSubject<string>(null);

  constructor(private injector: Injector,
    private router: Router,
    private route: ActivatedRoute) { }

  addToken(req: HttpRequest<any>, token: string): HttpRequest<any> {
    return req.clone({ setHeaders: { Authorization: 'Bearer ' + token } })
  }

  protected get authService(): OAuthService
  {
    return this.injector.get(OAuthService);
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpSentEvent | HttpHeaderResponse | HttpProgressEvent | HttpResponse<any> | HttpUserEvent<any>> {
    //const authService = this.injector.get(AuthService);
    return next.handle(this.addToken(req, this.authService.getAccessToken())).pipe(
      catchError(error => {
        console.log(error);
        if (error instanceof HttpErrorResponse) {
          console.log((<HttpErrorResponse>error).status)
          switch ((<HttpErrorResponse>error).status)
          {
            case 400:
              return this.handle400Error(error);
            case 401:
              return this.handle401Error(req, next);
            default:
              return _throw(error);
          }
        } else {
          return _throw(error);
        }
      }));
  }

  handle400Error(error) {
    if (error && error.status === 400 && error.error && error.error.error === 'invalid_grant') {
      // If we get a 400 and the error message is 'invalid_grant', the token is no longer valid so logout.
      return this.logoutUser();
    }
    return _throw(error);
  }

  handle401Error(req: HttpRequest<any>, next: HttpHandler) {
    if (!this.isRefreshingToken ) {
      this.isRefreshingToken = true;

      // Reset here so that the following requests wait until the token
      // comes back from the refreshToken call.
      this.tokenSubject.next(null);

      //const authService = this.injector.get(AuthService);

      this.authService.refreshToken().then((token :any) =>
      {
        console.log(token);
        switchMap((newToken: string) => {
          if (newToken) {
            this.tokenSubject.next(this.authService.getAccessToken());
            return next.handle(this.addToken(req, this.authService.getAccessToken()));
          }
          //  // If we don't get a new token, we are in trouble so logout.
          return this.logoutUser();
        })
        }),
        catchError(error =>
       {
         console.log(error);
          // If there is an exception calling 'refreshToken', bad news so logout.
          this.authService.logOut();
          return this.logoutUser();
        //return observableThrowError( error);
        }),
        finalize(() => {
          this.isRefreshingToken = false;
        });
    } else {
      return this.tokenSubject.pipe(
        filter(token => token != null),
        take(1),
        switchMap(token => {
          return next.handle(this.addToken(req, token));
        }));
    }

    
  }
  logoutUser() {
    // Route to the login page (implementation up to you)
    this.router.navigate(['../auth/login'], { relativeTo: this.route });
    return  _throw("error");
  }
}

/*
import { Injectable, Injector } from "@angular/core";
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from "@angular/common/http";
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable } from "rxjs/Observable";
import { BehaviorSubject } from "rxjs/BehaviorSubject";
import {catchError, switchMap, filter, take, } from 'rxjs/operators';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import { throwError } from 'rxjs';
@Injectable()
export class RefreshTokenInterceptor implements HttpInterceptor {

  protected get authService(): OAuthService{
    return this.injector.get(OAuthService);
  }

  private refreshTokenInProgress = false;
  // Refresh Token Subject tracks the current token, or is null if no token is currently
  // available (e.g. refresh pending).
  private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(    null  );
  constructor(private injector: Injector) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>>
  {
    return next.handle(request).pipe(catchError(error => {
      // We don't want to refresh token for some requests like login or refresh token itself
      // So we verify url and we throw an error if it's the case
      if (request.url.includes("/connect/token") || request.url.includes("login"))
      {
        // We do another check to see if refresh token failed
        // In this case we want to logout user and to redirect it to login page
        if (request.url.includes("/connect/token"))
        {
          this.authService.logOut();
          //return to login page
        }
        return throwError(error);
      }

      // If error status is different than 401 we want to skip refresh token
      // So we check that and throw the error if it's the case
      if (error.status !== 401) { return throwError(error);      }

      if (this.refreshTokenInProgress)
      {
        // If refreshTokenInProgress is true, we will wait until refreshTokenSubject has a non-null value
        // – which means the new token is ready and we can retry the request again
        return this.refreshTokenSubject
          .pipe(filter(result => result !== null))
          .pipe(take(1))
          .pipe(switchMap(() => next.handle(this.addAuthenticationToken(request))));
      }
      else {
        this.refreshTokenInProgress = true;
        // Set the refreshTokenSubject to null so that subsequent API calls will wait until the new token has been retrieved
        this.refreshTokenSubject.next(null);
        // Call auth.refreshAccessToken(this is an Observable that will be returned)
        this.authService.refreshToken().then((token: any) =>
        {
          console.log(token);
        //return this.authService.refreshToken().then
        //  .pipe(switchMap((token: any) => {
        //    //When the call to refreshToken completes we reset the refreshTokenInProgress to false
        //    // for the next time the token needs to be refreshed
           this.refreshTokenInProgress = false;
          this.refreshTokenSubject.next(this.authService.getAccessToken());
            return next.handle(this.addAuthenticationToken(request));
          })
          .catch((err: any) =>
          {
            this.refreshTokenInProgress = false;
            this.authService.logOut();
            return throwError(err);
          });
      }          
    }))
  }

  addAuthenticationToken(request) {
    // Get access token from Local Storage
    const accessToken = this.authService.getAccessToken();

    // If access token is null this means that user is not logged in
    // And we return the original request
    if (!accessToken) {
      return request;
    }

    // We clone the request, because the original request is immutable
    return request.clone({
      setHeaders: {
        Authorization: `Bearer ${this.authService.getAccessToken()}`
      }
    });
  }
}
*/
