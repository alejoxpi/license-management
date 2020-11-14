import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

const baseUrl = environment.validationrequests_api;
const getValidationRequestIdUrl = environment.getValidationRequestIdUrl;

@Injectable({
  providedIn: 'root'
})
export class ValidationRequestService {

  constructor(private http: HttpClient) { }

  getAll(): Observable<any>{
    return this.http.get(baseUrl);
  }

  get(id:string): Observable<any>{
    return this.http.get(`${getValidationRequestIdUrl}/${id}`);
  }
}
