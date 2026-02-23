import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators, AbstractControl, ValidatorFn, FormArray } from "@angular/forms";
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/delay';
import "rxjs/add/operator/takeUntil";
import { Subject } from 'rxjs/Subject';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { RoomService, NotificationService, HostelService} from '../../../services'
import { IRoomModels, IPagedResult, SelectItem } from '../../../models'

@Component({
  selector: 'room-list',
  templateUrl: './room-list.component.html',
  styles: [`
    nb-card {
      transform: translate3d(0, 0, 0);
    }
  `],
})
export class RoomListComponent implements OnInit, OnDestroy {
  private ngUnsubscribe: Subject<void> = new Subject<void>();
  createForm: FormGroup;

  public roomModel: IRoomModels;
  public tableList: IPagedResult<IRoomModels>;
  errorMessage: string;
  public hostelDropDownList: IPagedResult<SelectItem>;
  selectedRoomHostelId: number = 0;
  dialogToCreate: boolean;
  rowsPerPage: number;
  rowsPerPageOptions: number[] = [];

  constructor(private fb: FormBuilder, private location: Location,
    private toastrService: NotificationService,
    private roomService: RoomService,

    private hostelService: HostelService)
  {
    this.rowsPerPage = roomService.rowsPerPage;
    this.rowsPerPageOptions = roomService.rowsPerPageOptions;


    //get hostel dropdown list
    hostelService.getHostelDropDownByOrganizationIdAsync().subscribe(data =>
    {
      this.hostelDropDownList = data;
      console.log(this.hostelDropDownList)
      if (this.hostelDropDownList != null && this.hostelDropDownList != undefined)
      {
        this.selectedRoomHostelId = this.hostelDropDownList.results[0].value;
      }
    }, error =>
      {
        this.toastrService.showDanger("Hostel", <any>error)
      })
  }

  ngOnInit() {

    this.createForm = this.fb.group(
      {
        roomHostelId:0,
        roomId: 0,
        roomNumber:0 ,       
        roomFloorNumber: [0, [Validators.required, Validators.pattern("^[0-9]*$")]],
        roomRemarks: '',
        roomRent: [0, [Validators.required, Validators.pattern("^[0-9]*$")]],
        roomNoOfBeds: [0, [Validators.required, Validators.pattern("^[0-9]*$")]],
        roomRentPerBed: [0, [Validators.required, Validators.pattern("^[0-9]*$")]],
        //Create multiple rooms of same type
        numberOfRoomesToCreate: 1
        //RoomType: If single or multiple
        //rent type if single room rent or rent per bed
      });    
  }
  ngOnDestroy() {
    // If subscribed, we must unsubscribe before Angular destroys the component.
    // Failure to do so could create a memory leak.
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }


  create(): void {
    this.roomModel = this.createForm.value as IRoomModels;
    //this.roomModel.roomId = 0;
    this.roomModel.numberOfRoomesToCreate = 1;
   // console.log(this.roomModel);
    
    if (this.roomModel.roomId <= 0) {
      this.roomModel.roomId = 0;
      this.roomService.Create(this.roomModel).subscribe(data => {
        this.toastrService.showSuccess('Room', 'Your data has been successfully saved');
        this.createForm.reset();//reset forms
        this.dialogToCreate = false;//close the dialog
      }, error => {
        this.toastrService.showDanger("Room", <any>error)
      })
    }
    else {
      this.roomService.updateById(this.roomModel).subscribe(data =>
      {
        this.toastrService.showSuccess('Room', 'Your data has been successfully saved');
        //this.createForm.reset();//reset forms
        this.dialogToCreate = false; //close the dialog
      }, error => {
        this.toastrService.showDanger("Room", <any>error)
      })
    }
  }

  OnloadListLazy(event: LazyLoadEvent) {
    let pageIndex = event.first / event.rows + 1;
    setTimeout(() => {
      this.roomService.getPageList(this.selectedRoomHostelId, pageIndex, event.rows).takeUntil(this.ngUnsubscribe)
        .subscribe(data => {
          this.tableList = data;
          console.log(data);
        },
          error => this.errorMessage = <any>error);
    }, 250);
  }


  goBack(): void {
    this.location.back();
  }

  onHostelChange(event) {
    setTimeout(() => {
      this.roomService.getPageList(this.selectedRoomHostelId, 1, this.rowsPerPage).takeUntil(this.ngUnsubscribe)
        .subscribe(data => {
          this.tableList = data;
          console.log(data);
        },
          error => this.errorMessage = <any>error);
    }, 250);   
  }

  onRowSelect(event)
  {  
    var row = event.data as IRoomModels;
    if (row != null) {
      this.createForm.reset();
      this.createForm.patchValue
      ({
         roomHostelId: this.hostelDropDownList.results.filter((option) =>option.value == row.roomHostelId)[0].value,
         roomId:row.roomId,
         roomNumber:row.roomNumber,
         roomFloorNumber:row.roomFloorNumber,
         roomRemarks:row.roomRemarks,
         roomRent:row.roomRent,
         roomNoOfBeds:row.roomNoOfBeds,
         roomRentPerBed: row.roomRentPerBed,
         numberOfRoomesToCreate:1
        });
      this.dialogToCreate = true;
    }
  }
  showDialogToCreate() {

    this.dialogToCreate = true;
    this.createForm.reset();
  }

  OnDelete(tableRow) {
    if (window.confirm('Are you sure you want to delete?')) {
      this.roomService.deleteById(tableRow.roomId).takeUntil(this.ngUnsubscribe)
        .subscribe(data => {
          this.tableList.results = this.tableList.results.filter(item => item.roomId != tableRow.roomId);
          this.toastrService.showSuccess('Room', 'Your data has been successfully deleted');
        },
          error => {
            this.toastrService.showDanger("Room", <any>error);
          });
    }
  }
}

//onDeleteConfirm(event): void {
  //  if (window.confirm('Are you sure you want to delete?')) {
  //    event.confirm.resolve();
  //  } else {
  //    event.confirm.reject();
  //  }
  //}

//  redirectDelay: number = 0;
//  showMessages: any = {};
//  strategy: string = '';
//  submitted = false;
//  errors: string[] = [];
//  messages: string[] = [];
//  user: any = {};
//  // socialLinks: NbAuthSocialLink[] = [];

//  constructor() { }

//  register(): void { }
//}
