import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoomListComponent } from './list/room-list.component';
import { GenerateRoomListComponent } from './generate/generate-room-list.component';
import { RoomComponent } from './room.component';
export const routes: Routes = [
  // .. here goes our components routes    
  {
    path: '',
    component: RoomComponent,
    children:[
      {
        path: 'room-list',
        component: RoomListComponent,
      },
      {
        path: 'generate-room-list',
        component: GenerateRoomListComponent,
      },
    ],
  },
];

  

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class RoomRoutingModule {
}
