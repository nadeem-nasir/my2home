import { BaseDataService } from './BaseDataService'
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { IOrganizationStaffModels, RegisterModel } from '../models';

import { Injectable } from '@angular/core';
@Injectable({
  providedIn: 'root'
})
export class organizationStaffService extends BaseDataService {
  apiUrl = `${this.baseApiUrl}Staff/`; // this.baseApiUrl +  'city/';
  constructor(private http: HttpClient) {
    super();
  }

  getPageList(): Observable<IOrganizationStaffModels[]>
  {
    return this.http.get<IOrganizationStaffModels[]>(this.apiUrl)
      .pipe(
        catchError(this.handleError)
      );
  }

  public registerStaff(model: RegisterModel): Observable<any>
  {
    return this.http.post(this.apiUrl+'register', model).pipe(
      catchError(this.handleError));
  }

  public lockUser(userId: string): Observable<any>
  {

    return this.http.put(this.apiUrl + 'lockuser/' + userId, null).pipe(
      catchError(this.handleError));
  }
  public UnlockUser(userId: string): Observable<any> {

    return this.http.put(this.apiUrl + 'unlockuser/' + userId, null).pipe(
      catchError(this.handleError));
  }
  

  deleteById(id): Observable<void>
  {
    return this.http.delete<void>(this.apiUrl + id).pipe(
      catchError(this.handleError)
    );
  }
}
