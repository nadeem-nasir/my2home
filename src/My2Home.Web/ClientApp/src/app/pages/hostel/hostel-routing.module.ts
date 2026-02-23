import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HostelListComponent } from './list/hostel-list.component';
import { HostelComponent } from './hostel.component';
export const routes: Routes = [
  // .. here goes our components routes    
  {
    path: '',
    component: HostelComponent,
    children:[
      {
        path: 'hostel-list',
        component: HostelListComponent,
      },            
    ],
  },
];

  

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class HostelRoutingModule {
}
