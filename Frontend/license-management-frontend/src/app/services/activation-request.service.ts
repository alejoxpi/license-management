import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

const baseUrl = environment.activationrequests_api;

@Injectable({
  providedIn: 'root'
})
export class ActivationRequestService {

  constructor(private http: HttpClient) { }

  getAll(): Observable<any>{
    return this.http.get(baseUrl);
  }
}
