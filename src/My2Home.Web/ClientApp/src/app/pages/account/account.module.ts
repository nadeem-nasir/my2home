//import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
//import { FormsModule } from '@angular/forms';
//import { RouterModule } from '@angular/router';
//import { HttpClientModule } from '@angular/common/http';
import { DependenciesModule } from '../../shared/DependenciesModule'
import { AccountRoutingModule } from './account-routing.module'
import { AccountListComponent } from './list/account-list.component';
import { AccountComponent } from './account.component';
//import { from } from 'rxjs';

@NgModule({
  imports: [
    DependenciesModule, AccountRoutingModule,
    //CommonModule,
    //FormsModule,
    //RouterModule,   
    //HttpClientModule,   
  ],
  declarations: [
    // ... here goes our new components
   AccountComponent,
    AccountListComponent 
  ],
})
export class AccountModule {
}
