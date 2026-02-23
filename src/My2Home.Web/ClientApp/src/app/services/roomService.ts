import { BaseDataService } from './BaseDataService'
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { IRoomModels, SelectItem } from '../models';
import { IPagedResult } from '../models/paginationModel';
import { Injectable } from '@angular/core';
@Injectable({
  providedIn: 'root'
})
export class RoomService extends BaseDataService {
  apiUrl = `${this.baseApiUrl}room/`
  constructor(private http: HttpClient) {
    super();
  }
  getPageList(hostelId:number , pageNumber: number, rowsPerPage: number, searchCondition: string = ""): Observable<IPagedResult<IRoomModels>> {
    return this.http.get<IPagedResult<IRoomModels>>(this.apiUrl + hostelId + '/' + pageNumber + '/' + rowsPerPage)
      .pipe(
        catchError(this.handleError)
      );
  }

  getRoomDropDownByHostelIdAsync(hostelId: any): Observable<IPagedResult<SelectItem>>
  {
    return this.http.get<IPagedResult<SelectItem>>(this.apiUrl + 'dropdown/'+ hostelId)
      .pipe(
        catchError(this.handleError)
      );
  }

  getById(id: number): Observable<IRoomModels> {
    return this.http.get<IRoomModels>(this.apiUrl + id).pipe(
      catchError(this.handleError)
    );
  }

  Create(createModel): Observable<IRoomModels> {
    return this.http.post<IRoomModels>(this.apiUrl, createModel).pipe(
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
