/**
 * @license
 * Copyright Akveo. All Rights Reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 */
import { Component, OnInit } from '@angular/core';
import { AnalyticsService } from './@core/utils/analytics.service';

import { OAuthService } from 'angular-oauth2-oidc';
import { JwksValidationHandler } from 'angular-oauth2-oidc';
import { authConfig } from '../environments/environment'

@Component({
  selector: 'ngx-app',
  template: '<router-outlet></router-outlet>',
})
export class AppComponent implements OnInit {

  constructor(private analytics: AnalyticsService, public oauthService: OAuthService) {

  this.configureOidc();

    //this.oauthService.configure(authConfig);
    //this.oauthService.tokenValidationHandler = new JwksValidationHandler();
    //console.log(authConfig);

  }

  ngOnInit() {
    this.analytics.trackPageViews();
  }

  private configureOidc() {
   // const url = `${this.document.location.protocol}//${this.document.location.host}`;
    this.oauthService.setStorage(localStorage);
    this.oauthService.configure(authConfig);
    //
    this.oauthService.tokenValidationHandler = new JwksValidationHandler();
    this.oauthService.loadDiscoveryDocumentAndTryLogin();
  }


}
