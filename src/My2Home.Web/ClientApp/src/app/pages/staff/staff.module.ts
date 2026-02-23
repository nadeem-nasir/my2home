//import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
//import { FormsModule } from '@angular/forms';
//import { RouterModule } from '@angular/router';
//import { HttpClientModule } from '@angular/common/http';
import { DependenciesModule } from '../../shared/DependenciesModule'
import { StaffRoutingModule } from './staff-routing.module'
import { StaffListComponent } from './list/staff-list.component';
import { StaffComponent } from './staff.component';
import { RegisterStaffComponent } from './register/register.component';

//import { from } from 'rxjs';

@NgModule({
  imports: [
    DependenciesModule, StaffRoutingModule,
    //CommonModule,
    //FormsModule,
    //RouterModule,   
    //HttpClientModule,   
  ],
  declarations: [
    // ... here goes our new components
    StaffComponent,
    StaffListComponent,
    RegisterStaffComponent
  ],
})
export class StaffModule {
}
