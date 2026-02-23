import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CityListComponent } from './list/city-list.component';
import { CityComponent } from './city.component';
export const routes: Routes = [
  // .. here goes our components routes    
  {
    path: '',
    component: CityComponent,
    children:[
      {
        path: 'city-list',
        component: CityListComponent,
      },            
    ],
  },
];

  

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CityRoutingModule {
}
