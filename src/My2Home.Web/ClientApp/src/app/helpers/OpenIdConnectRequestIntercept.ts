import { Inject, Injectable, Injector } from '@angular/core';
//import { ErrorDialogService } from '../error-dialog/errordialog.service';
import {
  HttpInterceptor,
  HttpRequest,
  HttpResponse,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse,
  HttpHeaders, 
  HttpParams
} from '@angular/common/http';

import { Observable, throwError } from 'rxjs';
import { map, catchError, switchMap } from 'rxjs/operators';
//import { NbAuthJWTToken, NbAuthService, NbAuthOAuth2JWTToken } from '@nebular/auth';

@Injectable()
export class OpenIdConnectRequestIntercept implements HttpInterceptor
{
  //protected get authService(): NbAuthService {
  //  return this.injector.get(NbAuthService);
  //}

  constructor(private injector: Injector)
  {

  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    //console.log(clonedRequest.url);
    //console.log(this.authService.getToken());
    //first time login 
    return next.handle(req);
    //if (req.url.includes('/connect/token'))
    //{
      //console.log('to')
      // const payload = new HttpParams()
      //   .set('username', req.body.email)
      //   .set('password', req.body.password)
      //  .set('grant_type', 'password')
      //  .set('client_id', 'My2Homespaappcorecore')
      //  .set('scope', 'openid profile email offline_access  roles')
      //     //openid profile email offline_access client_id roles
      //req = req.clone({
      //  body: payload,
      //});      
      //return next.handle(req);
    //}
    //else {
      //console.log(this.authService.isAuthenticatedOrRefresh());
      //return this.authService.isAuthenticatedOrRefresh()
      //  .pipe(
      //    switchMap(authenticated => {
      //      if (authenticated) {
      //        return this.authService.getToken().pipe(
      //          switchMap((token: NbAuthOAuth2JWTToken) => {
      //            const JWT = `Bearer ${token.getValue()}`;
      //            req = req.clone({
      //              setHeaders: {
      //                Authorization: JWT,
      //              },
      //            });
      //            return next.handle(req);
      //          }),
      //        )
            //} else {
            //  // Request is sent to server without authentication so that the client code
            //  // receives the 401/403 error and can act as desired ('session expired', redirect to login, aso)
            //  return next.handle(req);
            //}
          
        
      //clonedRequest = clonedRequest.clone({
      //  headers: clonedRequest.headers.delete(
      //    'OpenIdConnectRequestIntercept'
      //  )
      //});
      //console.log('no token')
      //return next.handle(clonedRequest);
      // do some changes with the request as required
      //return next.handle(clonedRequest);
    }
  }


  //{
        //  username: clonedRequest.body.email,
        //  password: clonedRequest.body.password,
        //  grant_type: "password",
        //  client_id: "My2Homespaappcorecore",
        //  scope: "openid profile offline_access"
        //},
        //headers:{
        //  headers: clonedRequest.headers.delete('Content-Type').set("Content-Type", "application/x-www-form-urlencoded")
        //}

    //return next.handle(request).pipe(catchError(err => {
    //  if (err.status === 401) {
    //    // auto logout if 401 response returned from api
    //    this.authenticationService.logout();
    //    location.reload(true);
    //  }

    //  const error = err.error.message || err.statusText;
    //  return throwError(error);
    //}))
  

//clonedRequest = clonedRequest.clone({
      //  body: clonedRequest.body.append("grant_type", "password")
      //})
      //console.log('before')
      //console.log(clonedRequest.body.email);
      //console.log(clonedRequest);
      //const formData = new FormData();

      //formData.append('username', clonedRequest.body.email);
      //formData.append('password', clonedRequest.body.password);
      //formData.append('grant_type', 'password');
      //formData.append('client_id', 'My2Homespaappcorecore');
      //formData.append('scope', 'openid profile offline_access');

//clonedRequest = clonedRequest.clone({
      //  body: clonedRequest.body.append("", "")
      //});
      //var newBody=
      //{
      //  username : clonedRequest.body.email,
      //  password : clonedRequest.body.password,
      //  grant_type:"password",
      //  client_id: "My2Homespaappcorecore",
      //  scope:     "openid profile offline_access"
      //};

//clonedRequest = clonedRequest.clone({
      //  headers: new HttpHeaders({
      //    'Content-Type': 'application/x-www-form-urlencoded'
      //  })

      //});

      //console.log('after')
      //clonedRequest.body.append("grant_type", "password");
      //console.log(clonedRequest)
      //username
      //password
      //grant_type:password
      //Scope:openid profile myli offline_access
      //client_id : My2Homespaappcorecore
     // console.log(clonedRequest.body);
      //console.log('token');

      //Add parameters for open id connect
      //clonedRequest = clonedRequest.clone({
      //  headers: clonedRequest.headers.delete(
      //    'OpenIdConnectRequestIntercept'
      //  )
      //});
