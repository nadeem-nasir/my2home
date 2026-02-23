import { BaseDataService } from './BaseDataService'
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { IcityModel, SelectItem } from '../models';
import { IPagedResult } from '../models/paginationModel';
import { Injectable } from '@angular/core';
@Injectable({
  providedIn: 'root'
})
export class cityService extends BaseDataService {
  apiUrl = `${this.baseApiUrl}city/`; // this.baseApiUrl +  'city/';
  constructor(private http: HttpClient) {
    super();
  }

  getPageList(pageNumber: number, rowsPerPage: number, searchCondition: string = ""): Observable<IPagedResult<IcityModel>> {
    return this.http.get<IPagedResult<IcityModel>>(this.apiUrl + pageNumber + '/' + rowsPerPage)
      .pipe(
        catchError(this.handleError)
      );
  }

  getCityDropDownPageList(): Observable<IPagedResult<SelectItem>>
  {
    return this.http.get<IPagedResult<SelectItem>>(this.apiUrl + '/dropdown')
      .pipe(
        catchError(this.handleError)
    );
   }

  getById(id: number): Observable<IcityModel> {
    return this.http.get<IcityModel>(this.apiUrl + id).pipe(
      catchError(this.handleError)
    );
  }

  Create(createModel): Observable<IcityModel> {
    return this.http.post<IcityModel>(this.apiUrl, createModel).pipe(
      catchError(this.handleError)
    );
  }

  updateById( updateModel): Observable<any> {
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
