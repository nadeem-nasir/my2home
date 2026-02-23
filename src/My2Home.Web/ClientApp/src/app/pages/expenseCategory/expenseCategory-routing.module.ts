import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ExpenseCategoryListComponent } from './list/expenseCategory-list.component';
import { ExpenseCategoryComponent } from './expenseCategory.component';
export const routes: Routes = [
  // .. here goes our components routes    
  {
    path: '',
    component: ExpenseCategoryComponent,
    children:[
      {
        path: 'expenseCategory-list',
        component: ExpenseCategoryListComponent,
      },            
    ],
  },
];

  

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ExpenseCategoryRoutingModule {
}
