import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { TableModule } from 'primeng/table';
import { ToolbarModule } from 'primeng/toolbar';
import { DropdownModule } from 'primeng/dropdown';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { CalendarModule } from 'primeng/calendar';
import {
  NbAlertModule,
  NbButtonModule,
  NbCheckboxModule,
  NbInputModule,
  NbCardModule,
  NbDatepickerModule, NbSpinnerModule
} from '@nebular/theme';

@NgModule({
  imports: [
  ],
  exports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    RouterModule,

    //nb
    NbAlertModule,
    NbInputModule,
    NbButtonModule,
    NbCheckboxModule,
    Ng2SmartTableModule,
    NbCardModule,
    NbDatepickerModule,
    NbSpinnerModule,
    /*primeng*/
    TableModule,
    ToolbarModule ,
    DropdownModule,
    ButtonModule,
    DialogModule,
    CalendarModule
  ]
})
export class DependenciesModule {
}
