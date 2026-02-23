import { BaseDataService } from './BaseDataService'
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { IBedModels, SelectItem } from '../models';
import { IPagedResult } from '../models/paginationModel';
import { Injectable } from '@angular/core';
@Injectable({
  providedIn: 'root'
})
export class BedService extends BaseDataService {
  apiUrl = `${this.baseApiUrl}bed/`
  constructor(private http: HttpClient) {
    super();
  }

  getBedDropDownByHostelIdAsync(hostelId: any): Observable<SelectItem[]>
  {
    return this.http.get<SelectItem[]>(this.apiUrl + 'dropdown/' + hostelId)
      .pipe(
        catchError(this.handleError)
      );
  }

  getPageList(hostelId:number , pageNumber: number, rowsPerPage: number, searchCondition: string = ""): Observable<IPagedResult<IBedModels>> {
    return this.http.get<IPagedResult<IBedModels>>(this.apiUrl + hostelId + '/' + pageNumber + '/' + rowsPerPage)
      .pipe(
        catchError(this.handleError)
      );
  }

  getById(id: number): Observable<IBedModels> {
    return this.http.get<IBedModels>(this.apiUrl + id).pipe(
      catchError(this.handleError)
    );
  }

  Create(createModel): Observable<IBedModels> {
    return this.http.post<IBedModels>(this.apiUrl, createModel).pipe(
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
