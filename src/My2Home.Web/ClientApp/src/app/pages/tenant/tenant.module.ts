//import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
//import { FormsModule } from '@angular/forms';
//import { RouterModule } from '@angular/router';
//import { HttpClientModule } from '@angular/common/http';
import { DependenciesModule } from '../../shared/DependenciesModule'
import { TenantRoutingModule } from './tenant-routing.module'
import { TenantListComponent } from './list/tenant-list.component';
import { TenantCreateEditComponent } from './createEdit/tenant-create-edit.component'
import { TenantComponent } from './tenant.component';
//import { from } from 'rxjs';

@NgModule({
  imports: [
    DependenciesModule, TenantRoutingModule,
    //CommonModule,
    //FormsModule,
    //RouterModule,   
    //HttpClientModule,   
  ],
  declarations: [
    // ... here goes our new components
    TenantComponent,
    TenantListComponent,
    TenantCreateEditComponent
  ],
})
export class TenantModule {
}
