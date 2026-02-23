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
import { cityService, NotificationService, countryService  } from '../../../services'
import { IcityModel, IPagedResult, ICountryModels} from '../../../models'

@Component({
  selector: 'city-list',
  templateUrl: './city-list.component.html',
  styles: [`
    nb-card {
      transform: translate3d(0, 0, 0);
    }
  `],
})
export class CityListComponent implements OnInit, OnDestroy
{
  private ngUnsubscribe: Subject<void> = new Subject<void>();
  createForm: FormGroup;
  public cityModel: IcityModel;
  public tableList: IPagedResult<IcityModel>;
  //public countryList: IPagedResult<ICountryModels>;
  errorMessage: string;
  rowsPerPage: number;
  rowsPerPageOptions: number[] = [];

  ngOnInit()
  {
    //this.toastrService.showSuccess('hi', 'toaster');
    //this.toastrService.showDanger("hi" , "Danger")
    this.createForm = this.fb.group(
    {
      cityName: ['', [Validators.required, Validators.minLength(3)]],
      cityId: 0,
      cityCountryId: 162
      });
  }
  ngOnDestroy() {
    // If subscribed, we must unsubscribe before Angular destroys the component.
    // Failure to do so could create a memory leak.
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  constructor(private _cityService: cityService, private toastrService: NotificationService,
    private fb: FormBuilder, private location: Location,
    private countryService: countryService)
  {
    this.rowsPerPage = _cityService.rowsPerPage;
    this.rowsPerPageOptions = _cityService.rowsPerPageOptions;

    //this.countryService.getCountryList().subscribe(data =>
    //{
    //  this.countryList = data;
    //  console.log(this.countryList)
    //}, error => {
    //    this.toastrService.showDanger("City", <any>error)
    //  })
  }

  create(): void
  {
    this.cityModel = this.createForm.value as IcityModel;
    if (this.cityModel.cityId <= 0) {
      this._cityService.Create(this.cityModel).subscribe(data => {
        this.toastrService.showSuccess('City', 'Your data has been successfully saved');
        this.createForm.reset();//reset forms
      }, error => {
        this.toastrService.showDanger("City", <any>error)
      })
    }
    else {
      this._cityService.updateById(this.cityModel).subscribe(data => {
        this.toastrService.showSuccess('City', 'Your data has been successfully saved');
        this.createForm.reset();//reset forms
      }, error => {
        this.toastrService.showDanger("City", <any>error)
      })
    }
  }

  onRowSelect(event) {
    this.createForm.reset();
    var row = event.data as IcityModel;
    if (row != null)
    {
      this.createForm.patchValue({
        cityName: row.cityName,
        cityId: row.cityId,
        cityCountryId:row.cityCountryId
      });
    }
  }
  OnloadListLazy(event: LazyLoadEvent)
  {
    let pageIndex = event.first / event.rows + 1;
    setTimeout(() => {
      this._cityService.getPageList(pageIndex, event.rows).takeUntil(this.ngUnsubscribe)
        .subscribe(data =>
        {
          this.tableList = data;
          //console.log(data);
        },
          error => this.errorMessage = <any>error);
    }, 250);
  }
  
  
  goBack(): void {
    this.location.back();
  }
}

//onDeleteConfirm(event): void {
  //  if (window.confirm('Are you sure you want to delete?')) {
  //    event.confirm.resolve();
  //  } else {
  //    event.confirm.reject();
  //  }
  //}
