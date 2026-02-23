import { Injectable, Injector } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { throwError } from 'rxjs';
import { environment } from '../../environments/environment'

@Injectable({
  providedIn: 'root'
})
export class BaseDataService {

  public baseApiUrl: string = "";
  public rowsPerPage: number = 10;
  public rowsPerPageOptions:number[] = [5,10,20,50,80,100]
  //Base service an return errors 
  constructor()
  {
    this.baseApiUrl = environment.baseApiUrl;

  }
  

  public handleError(err: HttpErrorResponse)
  {
    // in a real world app, we may send the server to some remote logging infrastructure
    // instead of just logging it to the console
    let errorMessage = '';
    if (err.error instanceof Error) {
      // A client-side or network error occurred. Handle it accordingly.
      errorMessage = `An error occurred: ${err.error.message}`;
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      errorMessage = `Server returned code: ${err.status}, error message is: ${err.message}`;
    }
    return throwError(errorMessage);
    //https://github.com/ReactiveX/rxjs/issues/3733
  }
}
