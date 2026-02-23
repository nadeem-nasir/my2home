//import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
//import { FormsModule } from '@angular/forms';
//import { RouterModule } from '@angular/router';
//import { HttpClientModule } from '@angular/common/http';
import { DependenciesModule } from '../../shared/DependenciesModule'
import { OrganizationRoutingModule } from './organization-routing.module'
import { OrganizationListComponent } from './list/organization-list.component';
import { OrganizationComponent } from './organization.component';
//import { from } from 'rxjs';

@NgModule({
  imports: [
    DependenciesModule, OrganizationRoutingModule,
    //CommonModule,
    //FormsModule,
    //RouterModule,   
    //HttpClientModule,   
  ],
  declarations: [
    // ... here goes our new components
    OrganizationComponent,
    OrganizationListComponent 
  ],
})
export class OrganizationModule {
}
