import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { BehaviorSubject } from 'rxjs';  // only for communication across sibling components
import { catchError } from 'rxjs/operators';
import { environment, authConfig } from '../../environments/environment';
import { IAuthConfigSettings } from '../models/authConfigModels'
// @Injectable({ providedIn: 'root' }) //this provides service in appModule

@ Injectable()
export class StartupService {
  data: any;
  constructor(private http: HttpClient) { }

  getAppSettings(): Promise< IAuthConfigSettings > {
    return new Promise< IAuthConfigSettings >((resolve, reject) => {
      this .http.get< IAuthConfigSettings >("/api/AppSettings/").subscribe(
        appSettings => {
          this .data = appSettings != undefined && appSettings != null ? appSettings : "";
          //console.log(this .data);
          this .MapAppSettings(appSettings);
          resolve(this .data);
        },
        error => {
          this .data = {};
          resolve(this .data);
        }
      );
    });
  }// end getMyliAppSettings

  private MapAppSettings(toMap: IAuthConfigSettings) {

   // environment.env = toMap.env;

    //environment.production = true;

    // base urls
    // environment.myliBCJServiceBaseUrl = toMap.myliServiceBaseUrls.bCJServiceBaseUrl;
    // environment.myliCMSServiceBaseUrl = toMap.myliServiceBaseUrls.cMSServiceBaseUrl;
    // environment.myliIdentityServiceBaseUrl = toMap.myliServiceBaseUrls.identityServiceBaseUrl;
    // environment.myliMyIndicatorsWebBaseUrl = toMap.myliServiceBaseUrls.myIndicatorsWebBaseUrl;

    // AuthConfig
    authConfig.issuer = toMap.issuer;
    authConfig.requireHttps = toMap.requireHttps;
    authConfig.redirectUri = toMap.redirectUri;
    authConfig.silentRefreshRedirectUri = toMap.silentRefreshRedirectUri;
    authConfig.clientId = toMap.clientId;
    authConfig.scope = toMap.scope;
    authConfig.showDebugInformation = toMap.showDebugInformation;
    authConfig.sessionChecksEnabled = toMap.sessionChecksEnabled;
    authConfig.tokenEndpoint = toMap.tokenEndpoint;
  }
}
