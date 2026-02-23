import { Component, OnDestroy, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/delay';
import "rxjs/add/operator/takeUntil";
import { Subject } from 'rxjs/Subject';
import { RoomService, NotificationService, HostelService } from '../../../services'
import { IRoomModels, IPagedResult, SelectItem } from '../../../models'

@Component({
  selector: 'generate-room-list',
  templateUrl: './generate-room-list.component.html',
  styles: [`
    nb-card {
      transform: translate3d(0, 0, 0);
    }
  `],
})
export class GenerateRoomListComponent implements OnInit, OnDestroy {
  private ngUnsubscribe: Subject<void> = new Subject<void>();
  createForm: FormGroup;

  public roomModel: IRoomModels;
  public tableList: IPagedResult<IRoomModels>;
  errorMessage: string;
  public hostelDropDownList: IPagedResult<SelectItem>;
  selectedRoomHostelId: number = 0;
  constructor(private fb: FormBuilder, private location: Location,
    private toastrService: NotificationService,
    private roomService: RoomService,

    private hostelService: HostelService) {
    //get hostel dropdown list
    this.hostelService.getHostelDropDownByOrganizationIdAsync().subscribe(data => {
      this.hostelDropDownList = data;
      //console.log(this.hostelDropDownList)
      if (this.hostelDropDownList != null && this.hostelDropDownList != undefined) {
        this.selectedRoomHostelId = this.hostelDropDownList.results[0].value;
      }
    }, error => {
        this.toastrService.showDanger("Hostel", <any>error)
      })
  }

  ngOnInit() {

    this.createForm = this.fb.group(
      {
        roomHostelId: '',
        roomId: 0,
        roomNumber: '',
        roomFloorNumber: ['', [Validators.required, Validators.pattern("^[0-9]*$")]],
        roomRemarks: '',
        roomRent: ['', [Validators.required, Validators.pattern("^[0-9]*$")]],
        roomNoOfBeds: ['', [Validators.required, Validators.pattern("^[0-9]*$")]],
        roomRentPerBed: ['', [Validators.required, Validators.pattern("^[0-9]*$")]],
        //Create multiple rooms of same type
        numberOfRoomesToCreate: ['', [Validators.required, Validators.pattern("^[0-9]*$")]],
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
    this.roomModel.roomId = 0;
    this.roomService.Create(this.roomModel).subscribe(data => {
      this.toastrService.showSuccess('Room', 'Your data has been successfully saved');
      this.createForm.reset();//reset forms
    }, error => {
      this.toastrService.showDanger("Room", <any>error)
    })
  }



  goBack(): void {
    this.location.back();
  }

  onHostelChange(event) {
    setTimeout(() => {
      this.roomService.getPageList(this.selectedRoomHostelId, 1, 5).takeUntil(this.ngUnsubscribe)
        .subscribe(data => {
          this.tableList = data;
          console.log(data);
        },
          error => this.errorMessage = <any>error);
    }, 250);
  }

}

