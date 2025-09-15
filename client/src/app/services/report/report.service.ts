import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from '../auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class ReportService {

  constructor(private authService: AuthService) { }

  BASE_URL = 'http://localhost:5062/api/Report'


  http: HttpClient = inject(HttpClient)


  exportWinnersReport(): Observable<Blob> {
    const headers = this.authService.getHeaders();
    return this.http.get(`${this.BASE_URL}/excel/winners`, { headers, responseType: 'blob' });
  }


  exportRevenueReport(): Observable<Blob> {
    const headers = this.authService.getHeaders();
    return this.http.get(this.BASE_URL + '/excel/revenue', { headers,responseType: 'blob' });
  }


  

}
