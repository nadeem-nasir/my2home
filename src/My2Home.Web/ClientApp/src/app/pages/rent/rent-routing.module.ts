import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RentListComponent } from './list/rent-list.component';

import { GenerateRentListComponent } from './generate/generate-rent-list.component';
import { GenerateRentSingleComponent } from './generateSingle/generate-rent-single.component';
import { RentListYearlyComponent } from './list/rent-list-yearly.component';

import { RentCreateEditComponent } from './createEdit/rent-create-edit.component'
import { RentComponent } from './rent.component';
export const routes: Routes = [
  // .. here goes our components routes    
  {
    path: '',
    component: RentComponent,
    children:[
      {
        path: 'rent-list',
        component: RentListComponent,
      },
      {
        path: 'rent-list-yearly',
        component: RentListYearlyComponent,
      },
      
      {
        path: 'generate-rent-list',
        component: GenerateRentListComponent,
      },
      {
        path: 'generate-rent-single',
        component: GenerateRentSingleComponent,
      },
      {
        path: 'rent-create-edit',
        component: RentCreateEditComponent,
      },
    ],
  }, 
];

  

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class RentRoutingModule {
}
