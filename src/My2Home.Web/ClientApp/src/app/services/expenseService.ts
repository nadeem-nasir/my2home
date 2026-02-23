import { BaseDataService } from './BaseDataService'
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { IExpenseModels } from '../models';
import { IPagedResult } from '../models/paginationModel';
import { Injectable } from '@angular/core';
@Injectable({
  providedIn: 'root'
})
export class ExpenseService extends BaseDataService {
  apiUrl = `${this.baseApiUrl}expense/`
  constructor(private http: HttpClient) {
    super();
  }

  getPageList(hostelId:number , pageNumber: number, rowsPerPage: number, searchCondition: string = ""): Observable<IPagedResult<IExpenseModels>> {
    return this.http.get<IPagedResult<IExpenseModels>>(this.apiUrl + hostelId + '/' + pageNumber + '/' + rowsPerPage + '/' + searchCondition)
      .pipe(
        catchError(this.handleError)
      );
  }

  
  getById(id: number): Observable<IExpenseModels> {
    return this.http.get<IExpenseModels>(this.apiUrl + id).pipe(
      catchError(this.handleError)
    );
  }

  Create(createModel): Observable<IExpenseModels> {
    return this.http.post<IExpenseModels>(this.apiUrl, createModel).pipe(
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
