//import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
//import { FormsModule } from '@angular/forms';
//import { RouterModule } from '@angular/router';
//import { HttpClientModule } from '@angular/common/http';
import { DependenciesModule } from '../../shared/DependenciesModule'
import { RentRoutingModule } from './rent-routing.module'

import { RentListComponent } from './list/rent-list.component';
import { RentListYearlyComponent } from './list/rent-list-yearly.component';


import { GenerateRentListComponent } from './generate/generate-rent-list.component';
import { GenerateRentSingleComponent } from './generateSingle/generate-rent-single.component';

import { RentCreateEditComponent } from './createEdit/rent-create-edit.component'
import { RentComponent } from './rent.component';
//import { from } from 'rxjs';

@NgModule({
  imports: [
    DependenciesModule, RentRoutingModule,
    //CommonModule,
    //FormsModule,
    //RouterModule,   
    //HttpClientModule,   
  ],
  declarations: [
    // ... here goes our new components
    RentComponent,
    RentListComponent,
    RentListYearlyComponent,
    GenerateRentListComponent,
    RentCreateEditComponent,
    GenerateRentSingleComponent
  ],
})
export class RentModule {
}
