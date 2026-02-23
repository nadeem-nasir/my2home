import { BaseDataService } from './BaseDataService'
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { IaccountModels, SelectItem, IDashboardModels } from '../models';
import { IPagedResult } from '../models/paginationModel';
import { Injectable } from '@angular/core';
@Injectable({
  providedIn: 'root'
})
export class dashboardService extends BaseDataService {
  apiUrl = `${this.baseApiUrl}dashboard/`; // this.baseApiUrl +  'city/';
  constructor(private http: HttpClient) {
    super();
  }
  
  getAccountPageListAsync(hostelId: number, pageNumber: number,
    rowsPerPage: number, searchCondition: string = ""): Observable<IPagedResult<IaccountModels>> {
    return this.http.get<IPagedResult<IaccountModels>>(
      this.apiUrl + hostelId + '/' + pageNumber + '/' + rowsPerPage + '/' + searchCondition)
      .pipe(
        catchError(this.handleError)
      );

  }

  getMonthlyAccountPageListAsync(hostelId: number, 
     searchCondition: string = ""): Observable<IaccountModels> {
    return this.http.get<IaccountModels>(
      this.apiUrl + hostelId + '/'+ searchCondition)
      .pipe(
        catchError(this.handleError)
      );

  }

  getDashboardAsync(hostelId: number): Observable<IDashboardModels> {
    return this.http.get<IDashboardModels>(
      this.apiUrl + hostelId)
      .pipe(
        catchError(this.handleError)
      );
  } 
}
