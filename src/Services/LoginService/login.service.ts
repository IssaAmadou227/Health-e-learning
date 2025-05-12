import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class LoginService {
   private apiUrlss = ``; 
    constructor(private http:HttpClient) 
    { }
    postData(data: FormData): Observable<any> {
      return this.http.post(`https://localhost:7168/api/Account/login`, data);
    }
    postVerifyData(data:FormData): Observable<any>{
      return this.http.post(`https://localhost:7168/api/Account/verify-email-code`, data);
    }
    postRegister(data:FormData): Observable<any>{
     return this.http.post(`https://localhost:7168/api/Account/register`, data);
    }
}
