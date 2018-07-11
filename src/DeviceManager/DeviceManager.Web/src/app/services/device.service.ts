import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { ServiceResponse, PagingDto, BaseDto, ResultType } from '../code/dto';


const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({ providedIn: 'root' })
export class DeviceService {

  constructor(private http: HttpClient) { }

  private deviceUrl = 'http://localhost:5001/api/device';  // URL to web api

  //#region Property

  // Create
  postProperty(dto: any): Observable<ServiceResponse<number>> {
    return this.http.post<ServiceResponse<number>>(`${this.deviceUrl}/postproperty`, dto, httpOptions).pipe(
      tap((id: ServiceResponse<number>) => this.log(`post dto w/ id=${id}`)),
      catchError(this.handleError<ServiceResponse<number>>('postProperty'))
    );
  }

  // Read
  getProperty(id: number): Observable<ServiceResponse<any>> {
    return this.http.get<ServiceResponse<any>>(`${this.deviceUrl}/getproperty/${id}`).pipe(
      tap(_ => this.log(`fetched dto id=${id}`)),
      catchError(this.handleError<ServiceResponse<any>>(`getProperty id=${id}`))
    );
  }

  // Update
  putProperty(dto: any): Observable<any> {
    return this.http.put(`${this.deviceUrl}/putproperty/${dto.Id}`, dto, httpOptions).pipe(
      tap(_ => this.log(`updated dto id=${dto.Id}`)),
      catchError(this.handleError<any>('putProperty'))
    );
  }

  // Delete
  deleteProperty(dto: any | number): Observable<any> {
    const id = typeof dto === 'number' ? dto : dto.Id;
    const url = `${this.deviceUrl}/deleteproperty/${id}`;

    return this.http.delete<any>(url, httpOptions).pipe(
      tap(_ => this.log(`deleted dto id=${id}`)),
      catchError(this.handleError<any>('deleteProperty'))
    );
  }

  // List
  listProperty(searchDto: any, pagingDto: PagingDto): Observable<ServiceResponse<any[]>> {
    const dictionary = {};
    dictionary['searchDto'] = searchDto;
    dictionary['pagingDto'] = pagingDto;

    return this.http.post<ServiceResponse<any[]>>(`${this.deviceUrl}/listproperty`, dictionary, httpOptions).pipe(
      tap((serviceResponse: ServiceResponse<any>) => this.log(`fetched dtos:` + serviceResponse.Data.length)),
      catchError(this.handleError<ServiceResponse<any[]>>(`getPropertys`)));
  }

  //#endregion Property


  //#region Model

  // Create
  postModel(dto: BaseDto): Observable<ServiceResponse<number>> {
    return this.http.post<ServiceResponse<number>>(`${this.deviceUrl}/postmodel`, dto, httpOptions).pipe(
      tap((id: ServiceResponse<number>) => this.log(`post dto w/ id=${id}`)),
      catchError(this.handleError<ServiceResponse<number>>('postModel'))
    );
  }

  // Read
  getModel(id: number): Observable<ServiceResponse<BaseDto>> {
    return this.http.get<ServiceResponse<BaseDto>>(`${this.deviceUrl}/getmodel/${id}`).pipe(
      tap(_ => this.log(`fetched dto id=${id}`)),
      catchError(this.handleError<ServiceResponse<BaseDto>>(`getModel id=${id}`))
    );
  }

  // Update
  putModel(dto: BaseDto): Observable<BaseDto> {
    return this.http.put(`${this.deviceUrl}/putmodel/${dto.Id}`, dto, httpOptions).pipe(
      tap(_ => this.log(`updated dto id=${dto.Id}`)),
      catchError(this.handleError<any>('putModel'))
    );
  }

  // Delete
  deleteModel(dto: BaseDto | number): Observable<BaseDto> {
    const id = typeof dto === 'number' ? dto : dto.Id;
    const url = `${this.deviceUrl}/deletemodel/${id}`;

    return this.http.delete<BaseDto>(url, httpOptions).pipe(
      tap(_ => this.log(`deleted dto id=${id}`)),
      catchError(this.handleError<any>('deleteModel'))
    );
  }

  // List
  listModel(searchDto: BaseDto, pagingDto: PagingDto): Observable<ServiceResponse<BaseDto[]>> {

    const dictionary = {};
    dictionary['searchDto'] = searchDto;
    dictionary['pagingDto'] = pagingDto;

    return this.http.post<ServiceResponse<BaseDto[]>>(`${this.deviceUrl}/listmodel`, dictionary, httpOptions).pipe(
      tap((serviceResponse: ServiceResponse<BaseDto[]>) => this.log(`fetched dtos:` + serviceResponse.Data.length)),
      catchError(this.handleError<ServiceResponse<BaseDto[]>>(`getModels`)));
  }

  //#endregion Model


  //#region BaseMethods

  // Create
  post(dto: BaseDto, apiPath: string): Observable<ServiceResponse<number>> {
    return this.http.post<ServiceResponse<number>>(apiPath, dto, httpOptions).pipe(
      tap((id: ServiceResponse<number>) => this.log(`post dto w/ id=${id}`)),
      catchError(this.handleError<ServiceResponse<number>>('postModel'))
    );
  }

  // Read
  get(id: number, apiPath: string): Observable<ServiceResponse<BaseDto>> {
    return this.http.get<ServiceResponse<BaseDto>>(`${apiPath}/${id}`).pipe(
      tap(_ => this.log(`fetched dto id=${id}`)),
      catchError(this.handleError<ServiceResponse<BaseDto>>(`getModel id=${id}`))
    );
  }

  // Update
  put(dto: BaseDto, apiPath: string): Observable<BaseDto> {
    return this.http.put(`${apiPath}/${dto.Id}`, dto, httpOptions).pipe(
      tap(_ => this.log(`updated dto id=${dto.Id}`)),
      catchError(this.handleError<any>('putModel'))
    );
  }

  // Delete
  delete(dto: BaseDto | number, apiPath: string): Observable<BaseDto> {
    const id = typeof dto === 'number' ? dto : dto.Id;
    const url = `${apiPath}/${id}`;

    return this.http.delete<BaseDto>(url, httpOptions).pipe(
      tap(_ => this.log(`deleted dto id=${id}`)),
      catchError(this.handleError<any>('deleteModel'))
    );
  }

  // List
  list(searchDto: BaseDto, pagingDto: PagingDto, apiPath: string): Observable<ServiceResponse<BaseDto[]>> {

    const dictionary = {};
    dictionary['searchDto'] = searchDto;
    dictionary['pagingDto'] = pagingDto;

    return this.http.post<ServiceResponse<BaseDto[]>>(apiPath, dictionary, httpOptions).pipe(
      tap((serviceResponse: ServiceResponse<BaseDto[]>) => this.log(`fetched dtos:` + serviceResponse.Data.length)),
      catchError(this.handleError<ServiceResponse<BaseDto[]>>(`getModels`)));
  }

  //#endregion BaseMethods


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

      // TODO: better job of transforming error for user consumption
      this.log(`${operation} failed: ${error.message}`);

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }

  /** Log a HeroService message with the MessageService */
  private log(message: string) {
    // this.messageService.add('HeroService: ' + message);
    console.log(message);
  }
}
