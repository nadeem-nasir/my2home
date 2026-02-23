import { Component,  OnDestroy, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/delay';
import "rxjs/add/operator/takeUntil";
import { Subject } from 'rxjs/Subject';
import { NotificationService, organizationStaffService } from '../../../services'

import { IOrganizationStaffModels } from '../../../models'

@Component({
  selector: 'staff-list',
  templateUrl: './staff-list.component.html',
  styles: [`
    nb-card {
      transform: translate3d(0, 0, 0);
    }
  `],
})
export class StaffListComponent implements OnInit, OnDestroy {

  private ngUnsubscribe: Subject<void> = new Subject<void>();
    
  public tableList: IOrganizationStaffModels[];
  errorMessage: string;
  
  constructor(private organizationStaffService: organizationStaffService, 
    private location: Location, private toastrService: NotificationService,

  )
  {    
    organizationStaffService.getPageList().subscribe(data =>
    {
      this.tableList = data;     
    }, error =>
      {
      this.toastrService.showDanger("organization", <any>error)
      });   
  }

  ngOnInit() {    
  }
  

  lockUser(tableRow) {
    if (window.confirm('Are you sure you want to update?')) {
      this.organizationStaffService.lockUser(tableRow.userId).takeUntil(this.ngUnsubscribe)
        .subscribe(data => {         
          this.toastrService.showSuccess('Staff', 'Your data has been successfully updated');
        },
          error => {
            this.toastrService.showDanger("Staff", <any>error);
          });
    }
  }
  unlockUser(tableRow) {
    if (window.confirm('Are you sure you want to update?')) {
      this.organizationStaffService.UnlockUser(tableRow.userId).takeUntil(this.ngUnsubscribe)
        .subscribe(data => {
          this.toastrService.showSuccess('Staff', 'Your data has been successfully updated');
        },
          error => {
            this.toastrService.showDanger("Staff", <any>error);
          });
    }
  }

  deleteById(tableRow) {
    if (window.confirm('Are you sure you want to delete?')) {
      this.organizationStaffService.deleteById(tableRow.userId).takeUntil(this.ngUnsubscribe)
        .subscribe(data =>
        { 
          this.tableList = this.tableList.filter(item => item.userId != tableRow.userId);
          this.toastrService.showSuccess('Staff', 'Your data has been successfully deleted');
        },
          error => {
            this.toastrService.showDanger("Staff", <any>error);
          });
    }
  }
    
  ngOnDestroy()
  {
    // If subscribed, we must unsubscribe before Angular destroys the component.
    // Failure to do so could create a memory leak.
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  goBack(): void {
    this.location.back();
  }
}

