import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Category } from '../../models/category.model';
import { AuthService } from '../auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor(private authService: AuthService) { }

  BASE_URL = 'http://localhost:5062/api/Category'
 
  http: HttpClient = inject(HttpClient)
 
  getAll(): Observable<Category[]> {
    const headers = this.authService.getHeaders();
      return this.http.get<[]>(this.BASE_URL)
  }
}
