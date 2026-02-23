import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TenantListComponent } from './list/tenant-list.component';
import { TenantCreateEditComponent} from './createEdit/tenant-create-edit.component'
import { TenantComponent } from './tenant.component';
export const routes: Routes = [
  // .. here goes our components routes    
  {
    path: '',
    component: TenantComponent,
    children:[
      {
        path: 'tenant-list',
        component: TenantListComponent,
      },
      {
        path: 'tenant-create-edit',
        component: TenantCreateEditComponent,
      }, 
    ],
  },
];

  

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TenantRoutingModule {
}
