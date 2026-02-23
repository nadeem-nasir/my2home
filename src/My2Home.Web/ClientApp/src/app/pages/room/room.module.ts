//import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
//import { FormsModule } from '@angular/forms';
//import { RouterModule } from '@angular/router';
//import { HttpClientModule } from '@angular/common/http';
import { DependenciesModule } from '../../shared/DependenciesModule'
import { RoomRoutingModule } from './room-routing.module'
import { RoomListComponent } from './list/room-list.component';
import { GenerateRoomListComponent } from './generate/generate-room-list.component';

import { RoomComponent } from './room.component';
//import { from } from 'rxjs';

@NgModule({
  imports: [
    DependenciesModule, RoomRoutingModule,
    //CommonModule,
    //FormsModule,
    //RouterModule,   
    //HttpClientModule,   
  ],
  declarations: [
    // ... here goes our new components
    RoomComponent,
    RoomListComponent,
    GenerateRoomListComponent
  ],
})
export class RoomModule {
}
