import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrganizationListComponent } from './list/organization-list.component';
import { OrganizationComponent } from './organization.component';
export const routes: Routes = [
  // .. here goes our components routes    
  {
    path: '',
    component: OrganizationComponent,
    children:[
      {
        path: 'organization-list',
        component: OrganizationListComponent,
      },            
    ],
  },
];

  

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class OrganizationRoutingModule {
}
