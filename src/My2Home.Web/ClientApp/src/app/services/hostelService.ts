import { BaseDataService } from './BaseDataService'
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { IHostelModels, SelectItem } from '../models';
import { IPagedResult } from '../models/paginationModel';
import { Injectable } from '@angular/core';
@Injectable({
  providedIn: 'root'
})
export class HostelService extends BaseDataService {
  apiUrl = `${this.baseApiUrl}hostel/`
  constructor(private http: HttpClient) {
    super();
  }

  

  getPageList(pageNumber: number, rowsPerPage: number, searchCondition: string = ""): Observable<IPagedResult<IHostelModels>> {
    return this.http.get<IPagedResult<IHostelModels>>(this.apiUrl + pageNumber + '/' + rowsPerPage)
      .pipe(
        catchError(this.handleError)
      );
  }

  getByOrganizationIdAsync(): Observable<IPagedResult<IHostelModels>>
  {
    return this.http.get<IPagedResult<IHostelModels>>(this.apiUrl)
      .pipe(
        catchError(this.handleError)
      );
  }

  getHostelDropDownByOrganizationIdAsync(): Observable<IPagedResult<SelectItem>>
  {
    return this.http.get<IPagedResult<SelectItem>>(this.apiUrl +'dropdown/')
      .pipe(
        catchError(this.handleError)
      );
  }

  
  getById(id: number): Observable<IHostelModels> {
    return this.http.get<IHostelModels>(this.apiUrl + id).pipe(
      catchError(this.handleError)
    );
  }

  Create(createModel): Observable<IHostelModels> {
    return this.http.post<IHostelModels>(this.apiUrl, createModel).pipe(
      catchError(this.handleError)
    );
  }

  updateById(updateModel): Observable<any> {
    return this.http.put(this.apiUrl, updateModel).pipe(
      catchError(this.handleError)
    );
  }

  deleteById(id): Observable<void> {
    return this.http.delete<void>(this.apiUrl+id).pipe(
      catchError(this.handleError)
    );
  }
}
