import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/delay';
import "rxjs/add/operator/takeUntil";
import { Subject } from 'rxjs/Subject';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { HostelService, NotificationService, BedService, RoomService } from '../../../services'
import { IBedModels, IPagedResult, SelectItem } from '../../../models'

@Component({
  selector: 'bed-list',
  templateUrl: './bed-list.component.html',
  styles: [`
    nb-card {
      transform: translate3d(0, 0, 0);
    }
  `],
})
export class BedListComponent implements OnInit, OnDestroy {

  private ngUnsubscribe: Subject<void> = new Subject<void>();
  createForm: FormGroup;
  public model: IBedModels;
  public selectedModel: IBedModels;
  public tableList: IPagedResult<IBedModels>;
  errorMessage: string;
  public hostelDropDownList: IPagedResult<SelectItem>;
  public roomDropDownList: IPagedResult<SelectItem>;
  dialogToCreate: boolean;
  selectedHostelId: number = 0;
  rowsPerPage: number;
  rowsPerPageOptions: number[] = [];

  constructor(private hostelService: HostelService, private fb: FormBuilder,
    private location: Location, private toastrService: NotificationService,
    private bedService: BedService, private roomService: RoomService) {

    this.rowsPerPage = bedService.rowsPerPage;
    this.rowsPerPageOptions = bedService.rowsPerPageOptions;


    //get hostel dropdown list
    hostelService.getHostelDropDownByOrganizationIdAsync().subscribe(data =>
    {
      this.hostelDropDownList = data;
      //console.log(this.hostelDropDownList)
      if (this.hostelDropDownList != null && this.hostelDropDownList != undefined)
      {       
        this.selectedHostelId = this.hostelDropDownList.results[0].value;
      }
    }, error => {
      this.toastrService.showDanger("Hostel", <any>error)
      });

  }

  ngOnInit() {
    this.createForm = this.fb.group(
      {
        bedNumber: 0,
        roomId: [0, [Validators.required]],
        bedRent: [0, [Validators.required]],
        bedRemarks: '',
        roomHostelId: 0
      });

    //this.onChanges();
  }
  create(): void {
    this.model = this.createForm.value as IBedModels;
    if (this.model.bedNumber <= 0) {
      this.model.bedNumber = 0;
      this.bedService.Create(this.model).subscribe(data => {
        this.toastrService.showSuccess('Bed', 'Your data has been successfully saved');
        this.createForm.reset();//reset forms
        this.dialogToCreate = false;//close the dialog
      }, error => {
        this.toastrService.showDanger("Bed", <any>error)
      })
    }
    else {
      this.bedService.updateById(this.model).subscribe(data => {
        this.toastrService.showSuccess('Bed', 'Your data has been successfully saved');
        this.createForm.reset();//reset forms
        this.dialogToCreate = false;//close the dialog
      }, error => {
        this.toastrService.showDanger("Bed", <any>error)
      })
    }
  }

  OnloadListLazy(event: LazyLoadEvent) {
    let pageIndex = event.first / event.rows + 1;
    setTimeout(() => {
      this.bedService.getPageList(this.selectedHostelId, pageIndex, event.rows).takeUntil(this.ngUnsubscribe)
        .subscribe(data => {
          this.tableList = data;
          //console.log(data);
        },
          error => this.errorMessage = <any>error);
    }, 250);
  }


  onHostelChange(event) {
    setTimeout(() => {
      this.bedService.getPageList(this.selectedHostelId, 1, this.rowsPerPage).takeUntil(this.ngUnsubscribe)
        .subscribe(data => {
          this.tableList = data;
          //console.log(data);
        },
          error => this.errorMessage = <any>error);
    }, 250);
  }

  onChange(event)
  {
    //get hostel room, when dropdown list change
    this.roomService.getRoomDropDownByHostelIdAsync(event.value).subscribe(data => {
      this.roomDropDownList = data;
     // console.log(this.roomDropDownList)
    }, error =>
      {
      this.toastrService.showDanger("Room", <any>error)
    });

  }
  ngOnDestroy() {
    // If subscribed, we must unsubscribe before Angular destroys the component.
    // Failure to do so could create a memory leak.
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
  onRowSelect(event) {
    //this.newCar = false;
    //this.car = this.cloneCar(event.data);
    this.createForm.reset();
    var row = event.data as IBedModels;
    if (row != null) {
      this.createForm.patchValue({
        bedNumber:row.bedNumber,
        bedRent: row.bedRent,
        bedRemarks: row.bedRemarks
      });
    }
    //get rooms list
    this.roomService.getRoomDropDownByHostelIdAsync(this.selectedHostelId).subscribe(data => {
      this.roomDropDownList = data;
      this.createForm.patchValue({
        roomId: this.roomDropDownList.results.filter((option) => option.value == row.roomId)[0].value,
      });

    }, error => {
      this.toastrService.showDanger("Room", <any>error)
    });

    this.createForm.patchValue({
      roomHostelId: this.hostelDropDownList.results.filter((option) => option.value == this.selectedHostelId)[0].value,
    });

    this.dialogToCreate = true;

    }

  showDialogToCreate()
  {
    this.dialogToCreate = true;
    this.createForm.reset();
    this.roomService.getRoomDropDownByHostelIdAsync(this.selectedHostelId).subscribe(data =>
    {
      this.roomDropDownList = data;
      this.createForm.patchValue({
        roomHostelId: this.selectedHostelId,//this.hostelDropDownList.results[0].value,
        roomId: this.roomDropDownList.results.filter((option) => option.value == this.roomDropDownList.results[0].value)[0].value,

      });
    }, error => {
      this.toastrService.showDanger("Room", <any>error)
    });
  }

  OnDelete(tableRow)
  {
  if (window.confirm('Are you sure you want to delete?'))
    {
      this.bedService.deleteById(tableRow.bedNumber).takeUntil(this.ngUnsubscribe)
        .subscribe(data =>
        {
          this.tableList.results = this.tableList.results.filter(item => item.bedNumber != tableRow.bedNumber);
          this.toastrService.showSuccess('Bed', 'Your data has been successfully deleted');
        },
        error =>
        {
            this.toastrService.showDanger("Bed", <any>error);
       });
    }
  }

  goBack(): void {
    this.location.back();
  }
}

