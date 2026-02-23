import { BaseDataService } from './BaseDataService'
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { IExpenseCategoryModels, SelectItem } from '../models';
import { IPagedResult } from '../models/paginationModel';
import { Injectable } from '@angular/core';
@Injectable({
  providedIn: 'root'
})
export class ExpenseCategoryService extends BaseDataService {

  apiUrl = `${this.baseApiUrl}expenseCategory/`

  constructor(private http: HttpClient) {
    super();
  }

  getPageList(pageNumber: number, rowsPerPage: number, searchCondition: string = ""): Observable<IPagedResult<IExpenseCategoryModels>> {
    return this.http.get<IPagedResult<IExpenseCategoryModels>>(this.apiUrl + pageNumber + '/' + rowsPerPage)
      .pipe(
        catchError(this.handleError)
      );
  }

  getExpenseCategoryDropDown(): Observable<IPagedResult<SelectItem>>
  {
    return this.http.get<IPagedResult<SelectItem>>(this.apiUrl + 'dropdown/')
      .pipe(
        catchError(this.handleError)
      );
  }

  getById(id: number): Observable<IExpenseCategoryModels> {
    return this.http.get<IExpenseCategoryModels>(this.apiUrl + id).pipe(
      catchError(this.handleError)
    );
  }

  Create(createModel): Observable<IExpenseCategoryModels> {
    return this.http.post<IExpenseCategoryModels>(this.apiUrl, createModel).pipe(
      catchError(this.handleError)
    );
  }

  updateById(updateModel): Observable<any> {
    return this.http.put(this.apiUrl, updateModel).pipe(
      catchError(this.handleError)
    );
  }

  deleteById(id): Observable<void> {
    return this.http.delete<void>(this.apiUrl + id).pipe(
      catchError(this.handleError)
    );
  }
}
