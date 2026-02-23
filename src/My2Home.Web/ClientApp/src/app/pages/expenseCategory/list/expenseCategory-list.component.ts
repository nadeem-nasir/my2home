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
import { ExpenseCategoryService, NotificationService } from '../../../services'
import { IExpenseCategoryModels, IPagedResult } from '../../../models'

@Component({
  selector: 'expenseCategory-list',
  templateUrl: './expenseCategory-list.component.html',
  styles: [`
    nb-card {
      transform: translate3d(0, 0, 0);
    }
  `],
})
export class ExpenseCategoryListComponent implements OnInit, OnDestroy {

  private ngUnsubscribe: Subject<void> = new Subject<void>();
  createForm: FormGroup;
  public cityModel: IExpenseCategoryModels;
  public tableList: IPagedResult<IExpenseCategoryModels>;
  errorMessage: string;
  rowsPerPage: number;
  rowsPerPageOptions: number[] = [];

  constructor(private fb: FormBuilder, private location: Location,
    private toastrService: NotificationService,
    private expenseCategoryService: ExpenseCategoryService ) {

    this.rowsPerPage = expenseCategoryService.rowsPerPage;
    this.rowsPerPageOptions = expenseCategoryService.rowsPerPageOptions;


  }
  ngOnInit() {

    this.buildForm();
  }

  buildForm(): void
  {
    this.createForm = this.fb.group(
      {
        expenseCategoryName: ['', [Validators.required, Validators.minLength(3)]],
        expenseCategoryId: 0
      });
  }

  create(): void
  {

    this.cityModel = this.createForm.value as IExpenseCategoryModels;
    if (this.cityModel.expenseCategoryId <= 0) {
      this.expenseCategoryService.Create(this.cityModel).subscribe(data => {
        this.toastrService.showSuccess('ExpenseCategory', 'Your data has been successfully saved');
        this.createForm.reset();//reset forms
        this.createForm.patchValue({
          expenseCategoryId: 0

        })
      }, error => {
        this.toastrService.showDanger("ExpenseCategory", <any>error)
      })
    }
    else {

      this.expenseCategoryService.updateById(this.cityModel).subscribe(data => {
        this.toastrService.showSuccess('ExpenseCategory', 'Your data has been successfully saved');
        this.createForm.reset();//reset forms
      }, error => {
        this.toastrService.showDanger("ExpenseCategory", <any>error)
      })


    }
  }

  OnloadListLazy(event: LazyLoadEvent) {
    let pageIndex = event.first / event.rows + 1;
    setTimeout(() => {
      this.expenseCategoryService.getPageList(pageIndex, event.rows).takeUntil(this.ngUnsubscribe)
        .subscribe(data => {
          this.tableList = data;
          ///console.log(data);
        },
          error => this.errorMessage = <any>error);
    }, 250);
  }

  ngOnDestroy() {
    // If subscribed, we must unsubscribe before Angular destroys the component.
    // Failure to do so could create a memory leak.
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
  goBack(): void {
    this.location.back();
  }

  onRowSelect(event)
  {   
    this.createForm.reset();
    var row = event.data as IExpenseCategoryModels;
    if (row != null)
    {
      this.createForm.patchValue({
        expenseCategoryName: row.expenseCategoryName,
        expenseCategoryId: row.expenseCategoryId
      });
    }

  }
  
}
