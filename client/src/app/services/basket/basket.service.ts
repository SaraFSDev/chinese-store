// import { Injectable } from '@angular/core';
// import { HttpClient } from '@angular/common/http';
// import { AuthService } from '../auth/auth.service';
// import { Observable } from 'rxjs';
// import { GiftUser } from '../../models/giftUser.model';

// @Injectable({
//   providedIn: 'root'
// })
// export class BasketService {
//   private baseUrl = 'http://localhost:5062/api/User';

//   constructor(private http: HttpClient, private authService: AuthService) { }

//   addToBasket(giftId: number) {
//     const headers = this.authService.getHeaders();
//     console.log(headers);

//     //  const body = { quantity }
//     return this.http.post(`${this.baseUrl}?giftId=${giftId}`,{}, { headers: headers });
//   }


//   // addToBasket(giftId: number): void {
//   //   this.giftService.addGiftToBasket(giftId).then(response => {
//   //     console.log("Gift added to basket:", response);
//   //   }).catch(error => {
//   //     console.error("Error adding gift to basket:", error);
//   //   });
//   // }


//   removeFromBasket(giftId: number) {
//     const headers = this.authService.getHeaders();  // מקבל את הכותרת עם Bearer token
//     return this.http.delete(`${this.baseUrl}?giftId=${giftId}`, { headers: headers });
//   }

//   getBasket():Observable<any> {
//     const headers = this.authService.getHeaders();  // מקבל את הכותרת עם Bearer token
//     console.log(headers);

//     return this.http.get(`${this.baseUrl}/aa`, { headers: headers });
//   }

//   updateBasket(giftId: number,quantity:number) {
//     const headers = this.authService.getHeaders();  // מקבל את הכותרת עם Bearer token
//     return this.http.post(`${this.baseUrl}?giftId=${giftId}&quantity=&{quantity}`, {}, { headers: headers });
//   }

//   confirmPurchase() {
//     const headers = this.authService.getHeaders();
//     return this.http.post(`${this.baseUrl}/confirmPurchase`, {headers: headers});
//   }


// }
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../auth/auth.service';
import { Observable } from 'rxjs';
import { GiftUser } from '../../models/giftUser.model';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  private baseUrl = 'http://localhost:5062/api/User';


  constructor(private http: HttpClient, private authService: AuthService) { }

  addToBasket(giftId: number) {
    const headers = this.authService.getHeaders();  // מקבל את הכותרת עם Bearer token
    return this.http.post(`${this.baseUrl}?giftId=${giftId}`, {}, { headers });
  }


  removeFromBasket(giftId: number) {
    const headers = this.authService.getHeaders();  // מקבל את הכותרת עם Bearer token
    console.log(headers);

    return this.http.delete(`${this.baseUrl}?giftId=${giftId}`, { headers: headers });
  }

  getBasket(): Observable<GiftUser[]> {
    const headers = this.authService.getHeaders();  // מקבל את הכותרת עם Bearer token
    return this.http.get<GiftUser[]>(`${this.baseUrl}/userBasket`, { headers: headers });
  }


  updateBasket(giftId: number, quantity: number) {

    const headers = this.authService.getHeaders();  // מקבל את הכותרת עם Bearer token
    return this.http.post(`${this.baseUrl}?giftId=${giftId}&quantity=${quantity}`, { quantity }, { headers: headers });
  }

  confirmPurchase() {
    const headers = this.authService.getHeaders();
    console.log(headers);

    return this.http.post(`${this.baseUrl}/confirmPurchase`, {}, { headers: headers });
  }

  getGiftBuyers(giftId: number): Observable<any[]> {
    const headers = this.authService.getHeaders();
    return this.http.get<any[]>(`${this.baseUrl}/${giftId}/buyers`, { headers: headers });
  }
}
