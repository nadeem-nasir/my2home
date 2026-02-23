import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StaffListComponent } from './list/staff-list.component';
import { StaffComponent } from './staff.component';
import { RegisterStaffComponent } from './register/register.component';

export const routes: Routes = [
  // .. here goes our components routes    
  {
    path: '',
    component: StaffComponent,
    children:[
      {
        path: 'staff-list',
        component: StaffListComponent,
      },
      {
        path: 'register-staff',
        component: RegisterStaffComponent,
      }
    ],
  },
];

  

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class StaffRoutingModule {
}
