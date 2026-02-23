import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';

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
  selector: 'expense-create-edit',
  templateUrl: './expense-create-edit.component.html',
  styles: [`
    nb-card {
      transform: translate3d(0, 0, 0);
    }
  `],
})
export class ExpenseCreateEditComponent implements OnInit, OnDestroy {
  private ngUnsubscribe: Subject<void> = new Subject<void>();

  createForm: FormGroup;
  public hostelDropDownList: IPagedResult<SelectItem>;
  public expenseCategoryDropDownList: IPagedResult<SelectItem>;
  public selectedModel: IExpenseModels;
  //public tableList: IPagedResult<IExpenseModels>;
  dialogToCreate: boolean;
  public createModel: IExpenseModels;
  selectedHostelId: number = 0;
  //selectedexpenseId: number = 0;
  errorMessage: string;
  
  constructor(private fb: FormBuilder, private location: Location,
    private expenseCategoryService: ExpenseCategoryService,
    private hostelService: HostelService,
    private toastrService: NotificationService,
    private expenseService: ExpenseService, private route: ActivatedRoute)
  {
    //get hostel dropdown list

    //this.selectedexpenseId = Number.parseInt(this.route.snapshot.paramMap.get('Id'));

    //console.log(this.selectedexpenseId);

    this.hostelService.getHostelDropDownByOrganizationIdAsync().subscribe(data => {
      this.hostelDropDownList = data;
      if (this.hostelDropDownList != null && this.hostelDropDownList != undefined) {
        this.createForm.patchValue({
          expenseHostelId: this.hostelDropDownList.results[0].value
        });

        this.selectedHostelId = this.hostelDropDownList.results[0].value;
      }
      console.log(this.hostelDropDownList)
    }, error => {
      this.toastrService.showDanger("Hostel", <any>error)
      })


    this.expenseCategoryService.getExpenseCategoryDropDown().subscribe(data => {
      this.expenseCategoryDropDownList = data;
      if (this.expenseCategoryDropDownList != null){
        this.createForm.patchValue({          
          expenseCategoryId: this.expenseCategoryDropDownList.results[0].value
        });
      }
      console.log(this.expenseCategoryDropDownList)
    }, error => {
      this.toastrService.showDanger("expenseCategory", <any>error)
    })

    
  }

  ngOnInit() {

    this.createForm = this.fb.group(
      {
        expenseId:0,
        expenseName: ['', [Validators.required]],
        expenseYear: 0,
        expenseMonth: '',
        expenseAmount: ['', [Validators.required]],
        expenseDescription: '',
        expenseCreatedOn: [new Date(), [Validators.required]] ,       
        expenseCreatedBy:'' ,
        expenseUpdatedBy: '',
        expenseCategoryId: 0,
        expenseHostelId:0      
      });

    

    

    //get expense category dropdown list
       
    this.route.queryParams.subscribe(params => {
      var expenseId = Number.parseInt(params['Id']);
      console.log(expenseId);
      if (expenseId > 0) {
        this.expenseService.getById(expenseId).subscribe(data => {
                    
          this.createForm.patchValue
            ({
              expenseId: data.expenseId,
              expenseName: data.expenseName,
              expenseYear: data.expenseYear,
              expenseMonth: data.expenseMonth,
              expenseAmount: data.expenseAmount,
              expenseDescription: data.expenseDescription,
              expenseCreatedOn: data.expenseCreatedOn ? Moment(data.expenseCreatedOn).toDate() : '',
              expenseCreatedBy: data.expenseCreatedBy,
              expenseUpdatedBy: data.expenseUpdatedBy,
              expenseCategoryId: this.expenseCategoryDropDownList.results.filter((option) => option.value == data.expenseCategoryId)[0].value,
              expenseHostelId: this.hostelDropDownList.results.filter((option)=> option.value == data.expenseHostelId)[0].value,

            });
        }, error => {
            this.toastrService.showDanger("expense", <any>error)
          })
      }
    });


  }
  ngOnDestroy() {
    // If subscribed, we must unsubscribe before Angular destroys the component.
    // Failure to do so could create a memory leak.
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
  

  create(): void {

    this.createModel = this.createForm.value as IExpenseModels;

    //console.log(this.createModel);
    
    if (this.createModel.expenseId <= 0)
    {
      this.expenseService.Create(this.createModel).subscribe(data => {
        this.toastrService.showSuccess('Expense', 'Your data has been successfully saved');
        this.createForm.reset();//reset forms
        this.createForm.patchValue
          ({
            expenseId:0,
            expenseCategoryId: this.expenseCategoryDropDownList.results[0].value,
            expenseHostelId: this.hostelDropDownList.results[0].value,
            expenseCreatedOn: new Date(),
            expenseYear: 0,
          });
      }, error => {
        this.toastrService.showDanger("Expense", <any>error)
      })

    }
    else
    {
        this.expenseService.updateById(this.createModel).subscribe(data => {
        this.toastrService.showSuccess('Expense', 'Your data has been successfully saved');
        //this.createForm.reset();//reset forms
      }, error => {
        this.toastrService.showDanger("Expense", <any>error)
      })

    }

  }
  

  onRowSelect(event) {
    //this.newCar = false;
    //this.car = this.cloneCar(event.data);
    this.dialogToCreate = true;
    //console.log(event);
  }
  showDialogToCreate() {

    this.dialogToCreate = true;

  }

  goBack(): void
  {
    this.location.back();
  }
}

//OnloadListLazy(event: LazyLoadEvent) {

  //  let pageIndex = event.first / event.rows + 1;
  //  setTimeout(() => {
  //    this.expenseService.getPageList(this.selectedHostelId, pageIndex, event.rows).takeUntil(this.ngUnsubscribe)
  //      .subscribe(data => {
  //        this.tableList = data;
  //        console.log(data);
  //      },
  //        error => this.errorMessage = <any>error);
  //  }, 250);
  //}

//[rowsPerPageOptions]="expenseService.rowsPerPageOptions"
  //onHostelChange(event)
  //{
  //  setTimeout(() => {
  //    this.expenseService.getPageList(this.selectedHostelId, 1, this.expenseService.rowsPerPage).takeUntil(this.ngUnsubscribe)
  //      .subscribe(data => {
  //        this.tableList = data;
  //        console.log(data);
  //      },
  //        error => this.errorMessage = <any>error);
  //  }, 250);

  //}

  //onRowSelect(event) {
  //  console.log(event);
  //}

  //source: LocalDataSource = new LocalDataSource();
  //onDeleteConfirm(event): void {
  //  if (window.confirm('Are you sure you want to delete?')) {
  //    event.confirm.resolve();
  //  } else {
  //    event.confirm.reject();
  //  }
  //}

