import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BedListComponent } from './list/bed-list.component';
import { BedComponent } from './bed.component';
export const routes: Routes = [
  // .. here goes our components routes    
  {
    path: '',
    component: BedComponent,
    children:[
      {
        path: 'bed-list',
        component: BedListComponent,
      },            
    ],
  },
];

  

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class BedRoutingModule {
}
