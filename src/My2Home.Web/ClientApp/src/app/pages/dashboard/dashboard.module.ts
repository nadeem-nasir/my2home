import { NgModule } from '@angular/core';

import { DependenciesModule } from '../../shared/DependenciesModule'
import { ThemeModule } from '../../@theme/theme.module';
import { DashboardComponent } from './dashboard.component';

@NgModule({
  imports: [
    ThemeModule,
    DependenciesModule
  ],
  declarations: [
    DashboardComponent,
  ],
})
export class DashboardModule { }
