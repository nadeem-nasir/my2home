import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { PagesComponent } from './pages.component';
import { DashboardComponent } from './dashboard/dashboard.component';
const routes: Routes = [{
  path: '',
  component: PagesComponent,
  children: [
    {
      path: 'dashboard',
      component: DashboardComponent,
    },
    {
      path: 'hostel',
      loadChildren: './hostel/hostel.module#HostelModule',
    },
    //room
    {
      path: 'room',
      loadChildren: './room/room.module#RoomModule',
    },
    //bed
    {
      path: 'bed',
      loadChildren: './bed/bed.module#BedModule',
    },
    {
      path: 'tenant',
      loadChildren: './tenant/tenant.module#TenantModule',
    },
    //expense
    {
      path: 'expense',
      loadChildren: './expense/expense.module#ExpenseModule',
    },

    {
      path: 'rent',
      loadChildren: './rent/rent.module#RentModule',
    },

    //expenseCategory
    {
      path: 'expenseCategory',
      loadChildren: './expenseCategory/expenseCategory.module#ExpenseCategoryModule',
    },//end expenseCategory

    //city
    {
      path: 'city',
      loadChildren: './city/city.module#CityModule',
    },
    {
      path: 'organization',
      loadChildren: './organization/organization.module#OrganizationModule',
    },    
    {
      path: 'account',
      loadChildren: './account/account.module#AccountModule',
    },

    //Staff
    {
      path: 'staff',
      loadChildren: './staff/staff.module#StaffModule',
    },
    {
      path: '',
      redirectTo: 'dashboard',
      pathMatch: 'full',
    },
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PagesRoutingModule {
}
