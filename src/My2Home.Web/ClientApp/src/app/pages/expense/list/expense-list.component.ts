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
import { NotificationService, ExpenseCategoryService, HostelService, ExpenseService } from '../../../services'
import {IPagedResult , SelectItem, IExpenseModels} from '../../../models'
import * as Moment from "moment";

@Component({
  selector: 'expense-list',
  templateUrl: './expense-list.component.html',
  styles: [`
    nb-card {
      transform: translate3d(0, 0, 0);
    }
  `],
})
export class ExpenseListComponent implements OnInit, OnDestroy {
  private ngUnsubscribe: Subject<void> = new Subject<void>();
  createForm: FormGroup;
  public hostelDropDownList: IPagedResult<SelectItem>;
  public expenseCategoryDropDownList: IPagedResult<SelectItem>;
  //public selectedModel: IExpenseModels;
  public tableList: IPagedResult<IExpenseModels>;
  dialogToCreate: boolean;
  searchCondition: Date = new Date();;
  public createModel: IExpenseModels;
  selectedHostelId: number = 0;
  errorMessage: string;
  rowsPerPage: number;
  rowsPerPageOptions: number[] = [];

  constructor(private fb: FormBuilder, private location: Location,
    private expenseCategoryService: ExpenseCategoryService,
    private hostelService: HostelService,
    private toastrService: NotificationService,
    private expenseService: ExpenseService)
  {
    this.rowsPerPage = expenseService.rowsPerPage;
    this.rowsPerPageOptions = expenseService.rowsPerPageOptions;


    //get hostel dropdown list
   
  }

  ngOnInit() {

    this.createForm = this.fb.group(
      {
        expenseId:0,
        expenseName: ['', [Validators.required, Validators.minLength(3)]],
        expenseYear: 0,
        expenseMonth: '',
        expenseAmount: 0,
        expenseDescription: '',
        expenseCreatedOn: '',       
        expenseCreatedBy: '',
        expenseUpdatedBy: '',
        expenseCategoryId: 0,
        expenseHostelId:0      
      });

    this.hostelService.getHostelDropDownByOrganizationIdAsync().subscribe(data => {
      this.hostelDropDownList = data;
      if (this.hostelDropDownList != null && this.hostelDropDownList != undefined){
        this.createForm.patchValue({
          expenseHostelId: this.hostelDropDownList.results[0].value
        });

        this.selectedHostelId = this.hostelDropDownList.results[0].value;
      }
      console.log(this.hostelDropDownList)
    }, error => {
        this.toastrService.showDanger("Hostel", <any>error)
      })

    //get expense category dropdown list

    this.expenseCategoryService.getExpenseCategoryDropDown().subscribe(data => {
      this.expenseCategoryDropDownList = data;
      if (this.expenseCategoryDropDownList != null) {
        this.createForm.patchValue({
          expenseCategoryId: this.expenseCategoryDropDownList.results[0].value
        });
      }
      console.log(this.expenseCategoryDropDownList)
    }, error => {
        this.toastrService.showDanger("expenseCategory", <any>error)
      })
  }
  ngOnDestroy() {
    // If subscribed, we must unsubscribe before Angular destroys the component.
    // Failure to do so could create a memory leak.
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
  OnloadListLazy(event: LazyLoadEvent) {

    let pageIndex = event.first / event.rows + 1;
    let searchCondition = Moment(this.searchCondition).format("YYYY-MM-DDTHH:mm:ss.SSS[Z]");

    setTimeout(() => {
      this.expenseService.getPageList(this.selectedHostelId, pageIndex, event.rows, searchCondition).takeUntil(this.ngUnsubscribe)
        .subscribe(data => {
          this.tableList = data;
          console.log(data);
        },
          error => this.errorMessage = <any>error);
    }, 250);
  }

  create(): void {
    this.createModel = this.createForm.value as IExpenseModels;
    this.expenseService.Create(this.createModel).subscribe(data => {
      this.toastrService.showSuccess('Expense', 'Your data has been successfully saved');
      this.createForm.reset();//reset forms
    }, error => {
      this.toastrService.showDanger("Expense", <any>error)
    })
  }
  //[rowsPerPageOptions]="expenseService.rowsPerPageOptions"
  onHostelChange(event)
  {
    let searchCondition = Moment(this.searchCondition).format("YYYY-MM-DDTHH:mm:ss.SSS[Z]");

    setTimeout(() => {
      this.expenseService.getPageList(this.selectedHostelId, 1, this.rowsPerPage, searchCondition).takeUntil(this.ngUnsubscribe)
        .subscribe(data => {
          this.tableList = data;
          console.log(data);
        },
          error => this.errorMessage = <any>error);
    }, 250);

  }

  onMonthSelect(event)
  {

    let searchCondition = Moment(event).format("YYYY-MM-DDTHH:mm:ss.SSS[Z]");
    console.log(searchCondition);
    setTimeout(() => {
      this.expenseService.getPageList(this.selectedHostelId, 1, this.rowsPerPage, searchCondition).takeUntil(this.ngUnsubscribe)
        .subscribe(data => {
          this.tableList = data;
          console.log('rent list')
          console.log(data);
        },
          error => this.errorMessage = <any>error);
    }, 250);
  }
  

  onRowSelect(event) {
    //this.newCar = false;
    //this.car = this.cloneCar(event.data);
    this.dialogToCreate = true;
    console.log(event);
  }
  showDialogToCreate() {

    this.dialogToCreate = true;

  }

  OnDelete(tableRow) {
    if (window.confirm('Are you sure you want to delete?')) {
      this.expenseService.deleteById(tableRow.expenseId).takeUntil(this.ngUnsubscribe)
        .subscribe(data => {
          this.tableList.results = this.tableList.results.filter(item => item.expenseId != tableRow.expenseId);
          this.toastrService.showSuccess('Expense', 'Your data has been successfully deleted');
        },
          error => {
            this.toastrService.showDanger("Expense", <any>error);
          });
    }
  }
  goBack(): void
  {
    this.location.back();
  }
}
  
  //source: LocalDataSource = new LocalDataSource();
  //onDeleteConfirm(event): void {
  //  if (window.confirm('Are you sure you want to delete?')) {
  //    event.confirm.resolve();
  //  } else {
  //    event.confirm.reject();
  //  }
  //}

