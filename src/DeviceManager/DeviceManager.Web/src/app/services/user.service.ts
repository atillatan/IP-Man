import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable, of, config } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

import { ServiceResponse, PagingDto } from '../code/dto';
import { ConfigService } from './config.service';


const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()
export class UserService {

  private apiUrl: String = '';

  constructor(
    private http: HttpClient,
    private configService: ConfigService
  ) {
    this.apiUrl = configService.config.DefaultAPIAddress;
  }


  // Read
  getUser(id: number): Observable<ServiceResponse<any>> {
    return this.http.get<ServiceResponse<any>>(`${this.apiUrl}/api/example/getuser/${id}`).pipe(
      tap(serviceResponse => console.log(`fetched user id=${serviceResponse.Data.Id}`)),
      catchError(this.handleError<ServiceResponse<any>>(`getUser id=${id}`))
    );
  }


  /**
 * Handle Http operation that failed.
 * Let the app continue.
 * @param operation - name of the operation that failed
 * @param result - optional value to return as the observable result
 */
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead


      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }
}
