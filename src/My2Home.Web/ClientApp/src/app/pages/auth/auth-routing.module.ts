import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NbAuthComponent } from '@nebular/auth';
import { NgxLoginComponent } from './login/login.component';
import { NbRegisterComponent } from './register/register.component';
import { NbLogoutComponent } from './logout/logout.component';
import { NbRequestPasswordComponent } from './request-password/request-password.component';
import { NbResetPasswordComponent } from './reset-password/reset-password.component';
import { NbRegisterConfirmationComponent } from './register-confirmation/register-confirmation.component';
import { RequestPasswordConfirmationComponent } from './request-password-confirmation/request-password-confirmation.component';
import { RsetPasswordConfirmationComponent } from './reset-passwprd-confirmation/reset-password-confirmation.component'
export const routes: Routes = [
  // .. here goes our components routes
  {
    path: 'auth',
    loadChildren: './auth.module#NgxAuthModule',
  },
  {
    path: '',
    component: NbAuthComponent,  
    children: [
      {
        path: 'login',
        component: NgxLoginComponent, 
      },
      {
        path: 'register',
        component: NbRegisterComponent, 
      },
      {
        path: 'register-confirmation',
        component: NbRegisterConfirmationComponent,
      },
      {
        path: 'logout',
        component: NbLogoutComponent, 
      },
      {
        path: 'request-password',
        component: NbRequestPasswordComponent, 
      },
      {
        path: 'reset-password',
        component: NbResetPasswordComponent, 
      },
      {
        path: 'reset-password-confirmation',
        component: RsetPasswordConfirmationComponent,
      },
      {
        path: 'request-password-confirmation',
        component: RequestPasswordConfirmationComponent
      }
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class NgxAuthRoutingModule {
}
