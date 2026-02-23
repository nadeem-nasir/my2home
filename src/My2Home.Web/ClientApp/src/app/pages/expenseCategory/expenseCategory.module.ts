//import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
//import { FormsModule } from '@angular/forms';
//import { RouterModule } from '@angular/router';
//import { HttpClientModule } from '@angular/common/http';
import { DependenciesModule } from '../../shared/DependenciesModule'
import { ExpenseCategoryRoutingModule } from './expenseCategory-routing.module'
import { ExpenseCategoryListComponent } from './list/expenseCategory-list.component';
import { ExpenseCategoryComponent } from './expenseCategory.component';
//import { from } from 'rxjs';

@NgModule({
  imports: [
    DependenciesModule, ExpenseCategoryRoutingModule,
    //CommonModule,
    //FormsModule,
    //RouterModule,   
    //HttpClientModule,   
  ],
  declarations: [
    // ... here goes our new components
    ExpenseCategoryComponent,
    ExpenseCategoryListComponent 
  ],
})
export class ExpenseCategoryModule {
}
