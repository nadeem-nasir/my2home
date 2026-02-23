/**
 * @license
 * Copyright Akveo. All Rights Reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 */
import { APP_BASE_HREF } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule, APP_INITIALIZER  } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { CoreModule } from './@core/core.module';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { ThemeModule } from './@theme/theme.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { StartupService } from '../app/services/startupService';
import { OpenIdConnectRequestIntercept} from '../app/helpers/openIdConnectRequestIntercept';
import { RefreshTokenInterceptor } from '../app/helpers/RefreshTokenInterceptor';
import { AddAuthTokenInterceptor } from '../app/helpers/AddAuthTokenInterceptor';
//StartupService factory

export function setupStartupServiceFactory(
  service: StartupService): Function {
  return () => service.getAppSettings();
}

@ NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    AppRoutingModule,
    NgbModule.forRoot(),
    ThemeModule.forRoot(),
    CoreModule.forRoot(),
    
  ],
  bootstrap: [AppComponent],
  providers: [
    { provide: APP_BASE_HREF, useValue: '/' },
    StartupService,
    {
      provide: APP_INITIALIZER,
      useFactory: setupStartupServiceFactory,
      deps: [
        StartupService,
      ],
      multi: true
    },
    { provide: HTTP_INTERCEPTORS, useClass: RefreshTokenInterceptor, multi: true },
  ],
})
export class AppModule {
}
