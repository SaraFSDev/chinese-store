import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Donator } from '../../models/donator.model';
import { AuthService } from '../auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class DonatorService {

  constructor(private authService: AuthService) { }
  
  BASE_URL = 'http://localhost:5062/api/Donator'

  http: HttpClient = inject(HttpClient)

  getAll(): Observable<Donator[]> {
    const headers = this.authService.getHeaders();
    return this.http.get<Donator[]>(this.BASE_URL,{ headers: headers })
  }

  getByName(): Observable<Donator[]> {
    const headers = this.authService.getHeaders();
    return this.http.get<Donator[]>(`${this.BASE_URL}/filterByName`,{ headers: headers })
  }

  getByEmail(): Observable<Donator[]> {
    const headers = this.authService.getHeaders();
    return this.http.get<Donator[]>(`${this.BASE_URL}/filterByEmail`,{ headers: headers })
  }

  getById(id: number): Observable<Donator> {
    const headers = this.authService.getHeaders();
    return this.http.get<Donator>(this.BASE_URL + '/' + id,{ headers: headers })
  }
  create(donator: Donator) {
    const headers = this.authService.getHeaders();
    return this.http.post(this.BASE_URL, donator,{ headers: headers })
  }

  update(donator: Donator) {
    const headers = this.authService.getHeaders();
    return this.http.put(this.BASE_URL, donator,{ headers: headers })
  }

  delete(id: number) {
    const headers = this.authService.getHeaders();
    return this.http.delete(this.BASE_URL + '/' + id,{ headers: headers })
  }


GetDonatorGiftsAsync(donatorId: number) {
  const headers = this.authService.getHeaders();
 return this.http.get(`${this.BASE_URL}/giftsByDonator/${donatorId}`,{ headers: headers });
}

}


