//import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
//import { RouterModule } from '@angular/router';
import { NgxAuthRoutingModule } from './auth-routing.module';
import { NbAuthModule } from '@nebular/auth';
import { OAuthModule } from 'angular-oauth2-oidc';
//import { HttpClientModule } from '@angular/common/http';
import { DependenciesModule } from '../../shared/DependenciesModule'


import { NgxLoginComponent } from './login/login.component';
import { NbRegisterComponent } from './register/register.component';
import { NbLogoutComponent } from './logout/logout.component';
import { NbRequestPasswordComponent } from './request-password/request-password.component';
import { NbResetPasswordComponent } from './reset-password/reset-password.component';
import { NbRegisterConfirmationComponent } from './register-confirmation/register-confirmation.component';
import { RequestPasswordConfirmationComponent } from './request-password-confirmation/request-password-confirmation.component';
import { RsetPasswordConfirmationComponent } from './reset-passwprd-confirmation/reset-password-confirmation.component';
@NgModule({
  imports: [
    DependenciesModule,
    //CommonModule,
    FormsModule,
    //RouterModule,
    //NbAlertModule,
    //NbInputModule,
    //NbButtonModule,
    //NbCheckboxModule,
    NgxAuthRoutingModule,

    NbAuthModule,
    //HttpClientModule,
    OAuthModule.forRoot()
  ],
  declarations: [
    // ... here goes our new components
    NgxLoginComponent,
    NbRegisterComponent,
    NbLogoutComponent,
    NbRequestPasswordComponent,
    NbResetPasswordComponent,
    NbRegisterConfirmationComponent,
    RequestPasswordConfirmationComponent,
    RsetPasswordConfirmationComponent
  ],
})
export class NgxAuthModule {
}
