//import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
//import { FormsModule } from '@angular/forms';
//import { RouterModule } from '@angular/router';
//import { HttpClientModule } from '@angular/common/http';
import { DependenciesModule } from '../../shared/DependenciesModule'
import { BedRoutingModule } from './bed-routing.module'
import { BedListComponent } from './list/bed-list.component';
import { BedComponent } from './bed.component';
//import { from } from 'rxjs';

@NgModule({
  imports: [
    DependenciesModule, BedRoutingModule,
    //CommonModule,
    //FormsModule,
    //RouterModule,   
    //HttpClientModule,   
  ],
  declarations: [
    // ... here goes our new components
    BedComponent,
    BedListComponent 
  ],
})
export class BedModule {
}
