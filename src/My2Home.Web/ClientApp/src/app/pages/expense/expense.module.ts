//import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
//import { FormsModule } from '@angular/forms';
//import { RouterModule } from '@angular/router';
//import { HttpClientModule } from '@angular/common/http';
import { DependenciesModule } from '../../shared/DependenciesModule'
import { ExpenseRoutingModule } from './expense-routing.module'
import { ExpenseListComponent } from './list/expense-list.component';
import { ExpenseCreateEditComponent } from './createEdit/expense-create-edit.component';

import { ExpenseComponent } from './expense.component';
//import { from } from 'rxjs';

@NgModule({
  imports: [
    DependenciesModule, ExpenseRoutingModule,
    //CommonModule,
    //FormsModule,
    //RouterModule,   
    //HttpClientModule,   
  ],
  declarations: [
    // ... here goes our new components
    ExpenseComponent,
    ExpenseListComponent,
    ExpenseCreateEditComponent
  ],
})
export class ExpenseModule {
}
