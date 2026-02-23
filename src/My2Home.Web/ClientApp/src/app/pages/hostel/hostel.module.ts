//import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
//import { FormsModule } from '@angular/forms';
//import { RouterModule } from '@angular/router';
//import { HttpClientModule } from '@angular/common/http';
import { DependenciesModule } from '../../shared/DependenciesModule'
import { HostelRoutingModule } from './hostel-routing.module'
import { HostelListComponent } from './list/hostel-list.component';
import { HostelComponent } from './hostel.component';
//import { from } from 'rxjs';

@NgModule({
  imports: [
    DependenciesModule, HostelRoutingModule,
    //CommonModule,
    //FormsModule,
    //RouterModule,   
    //HttpClientModule,   
  ],
  declarations: [
    // ... here goes our new components
    HostelComponent,
    HostelListComponent 
  ],
})
export class HostelModule {
}
