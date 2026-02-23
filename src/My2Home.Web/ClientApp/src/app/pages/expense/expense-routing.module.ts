import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ExpenseListComponent } from './list/expense-list.component';
import { ExpenseCreateEditComponent } from './createEdit/expense-create-edit.component';

import { ExpenseComponent } from './expense.component';
export const routes: Routes = [
  // .. here goes our components routes    
  {
    path: '',
    component: ExpenseComponent,
    children:[
      {
        path: 'expense-list',
        component: ExpenseListComponent,
      },
      {
        path: 'expense-create-edit',
        component: ExpenseCreateEditComponent,
      },  
    ],
  },
];

  

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ExpenseRoutingModule {
}
