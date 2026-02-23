import { BaseDataService } from './BaseDataService'
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { ICountryModels } from '../models';
import { IPagedResult } from '../models/paginationModel';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class countryService extends BaseDataService {

  apiUrl = `${this.baseApiUrl}country/`; // this.baseApiUrl +  'city/';
  constructor(private http: HttpClient) {
    super();
  }

  getPageList(pageNumber: number, rowsPerPage: number, searchCondition: string = ""): Observable<IPagedResult<ICountryModels>> {
    return this.http.get<IPagedResult<ICountryModels>>(this.apiUrl + pageNumber + '/' + rowsPerPage)
      .pipe(
        catchError(this.handleError)
      );
  }

  getCountryList(): Observable<IPagedResult<ICountryModels>> {
    return this.http.get<IPagedResult<ICountryModels>>(this.apiUrl)
      .pipe(
        catchError(this.handleError)
      );
  }

  getById(id: number): Observable<ICountryModels> {
    return this.http.get<ICountryModels>(this.apiUrl + id).pipe(
      catchError(this.handleError)
    );
  }

  Create(createModel): Observable<ICountryModels> {
    return this.http.post<ICountryModels>(this.apiUrl, createModel).pipe(
      catchError(this.handleError)
    );
  }

  updateById(id, updateModel): Observable<any> {
    return this.http.put(this.apiUrl, updateModel).pipe(
      catchError(this.handleError)
    );
  }

  deleteById(id): Observable<any> {
    return this.http.delete<any>(this.apiUrl + id).pipe(
      catchError(this.handleError)
    );
  }
}
