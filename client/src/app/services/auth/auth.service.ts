import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  // private baseUrl = 'http://localhost:5062/api/Auth';

  // constructor(private http: HttpClient) { }

  // register(user: any) {
  //   return this.http.post(`${this.baseUrl}/register`, user);
  // }

  // login(details: any): Observable<any> {
  //   return this.http.post(`${this.baseUrl}/login`, details);
  // }

  // saveToken(token: string): void {
  //   localStorage.setItem('authToken', token);
  // }

 

  // getToken(): string | null {
  //   try {
  //     return localStorage?.getItem('authToken') || null;
  //   } catch (error) {
  //     return null;
  //   }
  // }

  // logout(): void {
  //   localStorage.removeItem('authToken');
  // }
  
  // getHeaders(): { [key: string]: string } {
  //   const token = this.getToken();
  //   let headers = {};
  //   if (token) {
  //     headers = {
  //       'Authorization': `Bearer ${token}`,
  //       'Content-Type': 'application/json'
  //     };
  //   }
  //   return headers;
  // }

  private baseUrl = 'http://localhost:5062/api/Auth';


  constructor(private http: HttpClient) { }


  register(user: any) {
    return this.http.post(`${this.baseUrl}/register`, user,{ responseType: 'text' });
  }


  login(details: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/login`, details);
  }


  saveToken(token: string): void {
    localStorage.setItem('authToken', token);
  }


  getToken(): string | null {
    if (typeof window !== 'undefined' && typeof window.localStorage !== 'undefined') {
      return localStorage.getItem('authToken');
    }
    return null;
  }


  logout(): void {
    localStorage.removeItem('authToken');
    localStorage.removeItem('admin');
  }


  getHeaders(): { [key: string]: string } {
    const token = this.getToken();
    let headers = {};
    if (token) {
      headers = {
        'Authorization': `Bearer ${token}`,
        'Content-Type': 'application/json'
      };
    }
    return headers;
  }


  saveRole(isAdmin: boolean): void {
    localStorage.setItem('admin', isAdmin.toString());
  }


  isAdmin() {
    if (typeof window !== 'undefined' && window.localStorage ) {
      const role = localStorage.getItem('admin')
      return role === 'true';      
    }
    return false;
  }


  isAuthenticated() {
    if (typeof window !== 'undefined' && typeof window.localStorage !== 'undefined') {
      if (localStorage.getItem('authToken')) {
        return true;
      }
    }
    return false
  }



}


