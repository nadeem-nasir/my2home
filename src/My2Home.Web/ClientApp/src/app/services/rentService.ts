import { BaseDataService } from './BaseDataService'
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { IRentModels, SelectItem } from '../models';
import { IPagedResult } from '../models/paginationModel';
import { Injectable } from '@angular/core';
@Injectable({
  providedIn: 'root'
})
export class RentService extends BaseDataService {
  apiUrl = `${this.baseApiUrl}rent/`; // this.baseApiUrl +  'city/';
  constructor(private http: HttpClient) {
    super();
  }

  getPageList(hostelId: number, pageNumber: number, rowsPerPage: number, searchCondition: string = "", searchType:boolean=true): Observable<IPagedResult<IRentModels>> {
    return this.http.get<IPagedResult<IRentModels>>(this.apiUrl + hostelId + '/' + pageNumber + '/' + rowsPerPage + '/' + searchCondition + '/'+searchType)
      .pipe(
        catchError(this.handleError)
      );
  }

  
  getById(id: number): Observable<IRentModels> {
    return this.http.get<IRentModels>(this.apiUrl + id).pipe(
      catchError(this.handleError)
    );
  }

  Create(createModel): Observable<IRentModels> {
    return this.http.post<IRentModels>(this.apiUrl, createModel).pipe(
      catchError(this.handleError)
    );
  }

  updateById(updateModel): Observable<any> {
    return this.http.put(this.apiUrl, updateModel).pipe(
      catchError(this.handleError)
    );
  }

  update(rentId: number, rentStatus:string): Observable<any> {
    return this.http.put(this.apiUrl + '/' + rentId + '/' + rentStatus, null ).pipe(
      catchError(this.handleError)
    );
  }

  getRentStatus(): SelectItem[] {
    var statusList: SelectItem[];
    statusList = [];
    statusList.push({ label: "due", value: "due" });
    statusList.push({ label: "paid", value: "paid" });
    return statusList;
  }

  deleteById(id): Observable<void> {
    return this.http.delete<void>(this.apiUrl + id).pipe(
      catchError(this.handleError)
    );
  }
}
