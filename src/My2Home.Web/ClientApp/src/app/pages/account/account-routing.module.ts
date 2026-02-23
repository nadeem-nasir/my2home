import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AccountListComponent } from './list/account-list.component';
import { AccountComponent } from './account.component';
export const routes: Routes = [
  // .. here goes our components routes    
  {
    path: '',
    component: AccountComponent,
    children:[
      {
        path: 'account-list',
        component: AccountListComponent,
      },            
    ],
  },
];

  

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AccountRoutingModule {
}
