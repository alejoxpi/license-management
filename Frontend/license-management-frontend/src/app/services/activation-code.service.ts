import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';


const baseUrl = environment.activationcodes_api;
const createURL = environment.createactivationcodes_api;

@Injectable({
  providedIn: 'root'
})
export class ActivationCodeService {

  constructor(private http: HttpClient) { }

  getAll(): Observable<any> {
    return this.http.get(baseUrl);
  }

  create(data:any): Observable<any>{
    return this.http.post(createURL,data);
  }
}
