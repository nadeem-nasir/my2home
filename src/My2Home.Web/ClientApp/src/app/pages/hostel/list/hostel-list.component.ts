import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators, AbstractControl, ValidatorFn, FormArray } from "@angular/forms";
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/delay';
import "rxjs/add/operator/takeUntil";
import "rxjs/add/operator/filter";
import { Subject } from 'rxjs/Subject';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { HostelService, NotificationService, cityService } from '../../../services'
import { IHostelModels, IPagedResult, SelectItem } from '../../../models'

@Component({
  selector: 'hostel-list',
  templateUrl: './hostel-list.component.html',
  styles: [`
    nb-card {
      transform: translate3d(0, 0, 0);
    }
  `],
})
export class HostelListComponent implements OnInit, OnDestroy {

  private ngUnsubscribe: Subject<void> = new Subject<void>();
  createForm: FormGroup;
  public model: IHostelModels;
  public selectedModel: IHostelModels;
  public tableList: IPagedResult<IHostelModels>;
  errorMessage: string;
  public cityDropDownList: IPagedResult<SelectItem>;
  rowsPerPage: number;
  rowsPerPageOptions: number[] = [];

  constructor(private _hostelService: HostelService, private fb: FormBuilder,
    private location: Location, private toastrService: NotificationService,
  private cityService:cityService) {

    this.rowsPerPage = _hostelService.rowsPerPage;
    this.rowsPerPageOptions = _hostelService.rowsPerPageOptions;


  }

  ngOnInit() {

    this.createForm = this.fb.group(
      {
        hostelId: 0,
        hostelName: ['', [Validators.required, Validators.minLength(3)]],
        hostelAddress: '',
        hostelContactNumber: '',
        hostelContactNumber2: '',
        hostelContactPersonName: '',
        hostelContactPersonName2: '',
        hostelLat: null,
        hostelLong: null,
        hostelOrganizationId: 0,
        hostelCityId: ['', [Validators.required]],
      });

    this.cityService.getCityDropDownPageList().subscribe(data =>
    {
      this.cityDropDownList = data;
     // console.log(this.cityDropDownList)
      if (this.cityDropDownList != null && this.cityDropDownList != undefined)
      {
        //this.selectedRoomHostelId = this.cityDropDownList.results[0].value;
      }
    }, error => {
        this.toastrService.showDanger("City", <any>error)
      })
  }
  create(): void {
    this.model = this.createForm.value as IHostelModels;
    //Create
    if (this.model.hostelId <= 0)
    {
      this._hostelService.Create(this.model).subscribe(data => {
        this.toastrService.showSuccess('Hostel', 'Your data has been successfully saved');
       // this.createForm.reset();//reset forms
        this.LoadTableData();
      }, error => {
        this.toastrService.showDanger("Hostel", <any>error)
      })
    }
    //update 
    else {
      this._hostelService.updateById(this.model).subscribe(data => {
        this.toastrService.showSuccess('Hostel', 'Your data has been successfully saved');
        //this.createForm.reset();//reset forms
        this.LoadTableData();
      }, error => {
        this.toastrService.showDanger("Hostel", <any>error)
      })
    }
  }

  LoadTableData() {
    this._hostelService.getByOrganizationIdAsync().takeUntil(this.ngUnsubscribe)
      .subscribe(data =>
      {
        this.tableList = data;       
      },
        error => this.errorMessage = <any>error);
  }
  OnloadListLazy(event: LazyLoadEvent) {
    let pageIndex = event.first / event.rows + 1;
    setTimeout(() => {
      this._hostelService.getByOrganizationIdAsync().takeUntil(this.ngUnsubscribe)
        .subscribe(data => {
          this.tableList = data;
         // console.log(data);
        },
          error => this.errorMessage = <any>error);
    }, 250);
  }
  onRowSelect(event)
  {
    //console.log(event);
    var row = event.data as IHostelModels;
   // console.log(row);

    if (row != null) {
      this.createForm.reset();
      this.createForm.patchValue({
        hostelId: row.hostelId,
        hostelName: row.hostelName,
        hostelAddress: row.hostelAddress,
        hostelContactNumber: row.hostelContactNumber,
        hostelContactNumber2: row.hostelContactNumber2,
        hostelContactPersonName: row.hostelContactPersonName,
        hostelContactPersonName2: row.hostelContactPersonName2,
        hostelLat: null,
        hostelLong: null,
        hostelOrganizationId: row.hostelOrganizationId,
        hostelCityId: this.getSelectedcityDropDownList(row.hostelCityId)
        //Angular will only make a value as default selected if it points to the same memory location as in the original dataSource array
      });
    }
  }
  getSelectedcityDropDownList(SelectedhostelCityId: number): number
  {
    var retResult =
      this.cityDropDownList.results.filter((option) =>
      option.value == SelectedhostelCityId )   
    return retResult.length <= 0 ? 0 : retResult[0].value;
  }
  //https://stackoverflow.com/questions/50679446/angular-reactive-forms-how-to-set-default-value-of-select-dropdown-control/50679447
  ngOnDestroy() {
    // If subscribed, we must unsubscribe before Angular destroys the component.
    // Failure to do so could create a memory leak.
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  goBack(): void {
    this.location.back();
  }

  OnDelete(tableRow)
  {   
    if (window.confirm('Are you sure you want to delete?'))
    {
      this._hostelService.deleteById(tableRow.hostelId).takeUntil(this.ngUnsubscribe)
        .subscribe(data =>
        {
          this.tableList.results = this.tableList.results.filter(item => item.hostelId != tableRow.hostelId);
          this.toastrService.showSuccess('Hostel', 'Your data has been successfully deleted');
        },
        error =>
        {
          this.toastrService.showDanger("Hostel", <any>error);
        });         
    }
  }
}

