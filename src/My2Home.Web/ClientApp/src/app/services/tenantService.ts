import { BaseDataService } from './BaseDataService'
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { ITenantModels, SelectItem } from '../models';
import { IPagedResult } from '../models/paginationModel';
import { Injectable } from '@angular/core';
@Injectable({
  providedIn: 'root'
})
export class TenantService extends BaseDataService {
  apiUrl = `${this.baseApiUrl}tenant/`
  constructor(private http: HttpClient) {
    super();
  }

  getPageList(hostelId:number,pageNumber: number, rowsPerPage: number, searchCondition: string =""): Observable<IPagedResult<ITenantModels>> {
    return this.http.get<IPagedResult<ITenantModels>>(this.apiUrl + hostelId + '/' + pageNumber + '/' + rowsPerPage + '/'+ searchCondition)
      .pipe(
        catchError(this.handleError)
      );
  }

  getTenantDropDownListAsync(hostelId: number): Observable<IPagedResult<SelectItem>> {
    return this.http.get<IPagedResult<SelectItem>>(this.apiUrl + 'dropdown/' + hostelId)
      .pipe(
        catchError(this.handleError)
      );
  }

  getById(id: number): Observable<ITenantModels> {
    return this.http.get<ITenantModels>(this.apiUrl + id).pipe(
      catchError(this.handleError)
    );
  }

  getTenantStatus(): SelectItem[] {
   var statusList:  SelectItem[];
    statusList =[];
    statusList.push({ label: "active", value:"active" });
    statusList.push({ label: "inactive", value:"inactive" });
    return statusList;
  }
  Create(createModel): Observable<ITenantModels> {
    return this.http.post<ITenantModels>(this.apiUrl, createModel).pipe(
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
