import { BaseDataService } from './BaseDataService'
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { IOrganizationModels} from '../models';
import { IPagedResult } from '../models/paginationModel';
import { Injectable } from '@angular/core';
@Injectable({
  providedIn: 'root'
})
export class OrganizationService extends BaseDataService {
  apiUrl = `${this.baseApiUrl}organization/`

  constructor(private http: HttpClient) {
    super();
  }

  getPageList(pageNumber: number, rowsPerPage: number, searchCondition: string = ""): Observable<IPagedResult<IOrganizationModels>> {
    return this.http.get<IPagedResult<IOrganizationModels>>(this.apiUrl + pageNumber + '/' + rowsPerPage)
      .pipe(
        catchError(this.handleError)
      );
  }
  
  getById(id: number): Observable<IOrganizationModels> {
    return this.http.get<IOrganizationModels>(this.apiUrl + id).pipe(
      catchError(this.handleError)
    );
  }

  getByIdentityUserId(): Observable<IOrganizationModels>
  {
    return this.http.get<IOrganizationModels>(this.apiUrl).pipe(
      catchError(this.handleError)
    );
  }

  Create(createModel): Observable<IOrganizationModels> {
    return this.http.post<IOrganizationModels>(this.apiUrl, createModel).pipe(
      catchError(this.handleError)
    );
  }

  update(updateModel): Observable<any> {
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

