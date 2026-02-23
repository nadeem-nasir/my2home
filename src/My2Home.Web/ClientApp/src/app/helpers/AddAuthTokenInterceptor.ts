//import { Injectable, Injector } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { OAuthService } from 'angular-oauth2-oidc';
import { Injectable, Injector } from "@angular/core";
//import { AccountService } from '../account.service';
@Injectable()
export class AddAuthTokenInterceptor implements HttpInterceptor
{
  constructor(private injector: Injector)
  {
  }
  protected get authService(): OAuthService
  {
    return this.injector.get(OAuthService);
  }
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>>
  {
    // Get the auth header from the service.
    //const auth = this.inj.get(AccountService);
    console.log(this.authService.getAccessToken());
    const authHeader = this.authService.getAccessToken();//auth.accessToken;
    // Clone the request to add the new header.
    // const authReq = req.clone({ headers: req.headers.set('Authorization', authHeader) });
    // OR shortcut
    const authReq = req.clone({ setHeaders: { Authorization: 'Bearer ' + authHeader } });
    // Pass on the cloned request instead of the original request.
    return next.handle(authReq);
  }
}
