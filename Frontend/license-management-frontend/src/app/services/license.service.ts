import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

const baseUrl = environment.licenses_api;
const getLicenseIdUrl = environment.getLicenseIdUrl;

@Injectable({
  providedIn: 'root'
})
export class LicenseService {

  constructor(private http: HttpClient) { }

  getAll(): Observable<any>{
    return this.http.get(baseUrl);
  }

  get(id:any): Observable<any>{
    return this.http.get(`${getLicenseIdUrl}/${id}`);
  }


}
