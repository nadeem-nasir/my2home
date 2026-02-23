//import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
//import { FormsModule } from '@angular/forms';
//import { RouterModule } from '@angular/router';
//import { HttpClientModule } from '@angular/common/http';
import { DependenciesModule } from '../../shared/DependenciesModule'
import { CityRoutingModule } from './city-routing.module'
import { CityListComponent } from './list/city-list.component';
import { CityComponent } from './city.component';
//import { from } from 'rxjs';

@NgModule({
  imports: [
    DependenciesModule, CityRoutingModule,
    //CommonModule,
    //FormsModule,
    //RouterModule,   
    //HttpClientModule,   
  ],
  declarations: [
    // ... here goes our new components
    CityComponent,
    CityListComponent 
  ],
})
export class CityModule {
}
