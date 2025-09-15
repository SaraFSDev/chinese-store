// import { Injectable, inject } from '@angular/core';
// import { Gift } from '../../models/gift.model';
// import { HttpClient, HttpParams } from '@angular/common/http';
// import { Observable } from 'rxjs';
// import { AuthService } from '../auth/auth.service';

// @Injectable({
//   providedIn: 'root'
// })
// export class GiftService {

//   // constructor(private authService: AuthService) { }

//   // BASE_URL = 'http://localhost:5062/api/Gift'

//   // http: HttpClient = inject(HttpClient)

//   // getAll(): Observable<Gift[]> {
//   //   return this.http.get<Gift[]>(this.BASE_URL)
//   // }

//   // getAllWithDonator(): Observable<Gift[]> {
//   //   return this.http.get<Gift[]>(this.BASE_URL + '/giftsWithDonator')
//   // }

//   // getById(id: number): Observable<Gift> {
//   //   return this.http.get<Gift>(this.BASE_URL + '/' + id)
//   // }

//   // create(gift: Gift) {
//   //   return this.http.post(this.BASE_URL, gift)
//   // }

//   // update(gift: Gift) {
//   //   return this.http.put(this.BASE_URL, gift)
//   // }

//   // delete(id: number) {
//   //   return this.http.delete(this.BASE_URL + '/' + id)
//   // }

//   // lottery(giftId:number){
//   //   const headers = this.authService.getHeaders();
//   //   return this.http.post(`${this.BASE_URL}/lottery`,giftId,{ headers: headers })
//   // }
  

//   constructor(private authService: AuthService) { }


//   BASE_URL = 'http://localhost:5062/api/Gift'


//   http: HttpClient = inject(HttpClient)


//   getAll(): Observable<Gift[]> {
//     const headers = this.authService.getHeaders();
//     return this.http.get<Gift[]>(this.BASE_URL,{ headers: headers })
//   }


//   getAllWithDonatorAndCategory(): Observable<Gift[]> {
//     const headers = this.authService.getHeaders();
//     return this.http.get<Gift[]>(this.BASE_URL + '/giftsWithDonatorAndCategory',{ headers: headers })
//   }


//   getById(id: number): Observable<Gift> {
//     const headers = this.authService.getHeaders();
//     return this.http.get<Gift>(this.BASE_URL + '/' + id,{ headers: headers })
//   }


//   create(gift: Gift) {
//     const headers = this.authService.getHeaders();
//     return this.http.post(this.BASE_URL, gift,{ headers: headers })
//   }


//   update(gift: Gift) {
//     console.log(gift)
//     const headers = this.authService.getHeaders();
//     return this.http.put(this.BASE_URL, gift,{ headers: headers })
//   }


//   delete(id: number) {
//     const headers = this.authService.getHeaders();
//     return this.http.delete(this.BASE_URL + '/' + id,{ headers: headers })
//   }


//   lottery(giftId:number){
//     const headers = this.authService.getHeaders();
//     return this.http.post(`${this.BASE_URL}/lottery`,giftId,{ headers: headers })
//   }
 
//   search(text: string): Observable<Gift[]> {
//     const headers = this.authService.getHeaders();
//     return this.http.get<Gift[]>(`${this.BASE_URL}/search?text=${text}`, { headers: headers });
//   }

//   getAllWithPurchases():Observable<Gift[]>{
//     const headers = this.authService.getHeaders();
//     return this.http.get<Gift[]>(`${this.BASE_URL}/giftsWithPurchases`, { headers: headers });
//   }


//   sort(sortBy: string, order: string): Observable<Gift[]> {
//     const headers = this.authService.getHeaders();
//     let params = new HttpParams()
//       .set('sortBy', sortBy)
//       .set('order', order);
//     var g=this.http.get<Gift[]>(this.BASE_URL+'/sortBy', { params, headers });
//     console.log(g);
//     return g;
    
    
//   }

// }

import { Injectable, inject } from '@angular/core';
import { Gift } from '../../models/gift.model';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../auth/auth.service';


@Injectable({
  providedIn: 'root'
})
export class GiftService {


  // constructor(private authService: AuthService) { }


  // BASE_URL = 'http://localhost:5062/api/Gift'


  // http: HttpClient = inject(HttpClient)


  // getAll(): Observable<Gift[]> {
  //   return this.http.get<Gift[]>(this.BASE_URL)
  // }


  // getAllWithDonator(): Observable<Gift[]> {
  //   return this.http.get<Gift[]>(this.BASE_URL + '/giftsWithDonator')
  // }


  // getById(id: number): Observable<Gift> {
  //   return this.http.get<Gift>(this.BASE_URL + '/' + id)
  // }


  // create(gift: Gift) {
  //   return this.http.post(this.BASE_URL, gift)
  // }


  // update(gift: Gift) {
  //   return this.http.put(this.BASE_URL, gift)
  // }


  // delete(id: number) {
  //   return this.http.delete(this.BASE_URL + '/' + id)
  // }


  // lottery(giftId:number){
  //   const headers = this.authService.getHeaders();
  //   return this.http.post(`${this.BASE_URL}/lottery`,giftId,{ headers: headers })
  // }
 


  constructor(private authService: AuthService) { }

  BASE_URL = 'http://localhost:5062/api/Gift'




  http: HttpClient = inject(HttpClient)




  getAll(): Observable<Gift[]> {
    // const headers = this.authService.getHeaders();
    return this.http.get<Gift[]>(this.BASE_URL)//,{ headers: headers }
  }




  getAllWithDonatorAndCategory(): Observable<Gift[]> {
    const headers = this.authService.getHeaders();
    return this.http.get<Gift[]>(this.BASE_URL + '/giftsWithDonatorAndCategory',{ headers: headers })
  }


  getById(id: number): Observable<Gift> {
    const headers = this.authService.getHeaders();
    return this.http.get<Gift>(this.BASE_URL + '/' + id,{ headers: headers })
  }

  create(gift: Gift) {
    const headers = this.authService.getHeaders();
    return this.http.post(this.BASE_URL, gift,{ headers: headers })
  }

  update(gift: Gift) {
    console.log(gift)
    const headers = this.authService.getHeaders();
    return this.http.put(this.BASE_URL, gift,{ headers: headers })
  }




  delete(id: number) {
    const headers = this.authService.getHeaders();
    return this.http.delete(this.BASE_URL + '/' + id,{ headers: headers })
  }




  lottery(giftId:number):Observable<any>{
    const headers = this.authService.getHeaders();
    return this.http.post(`${this.BASE_URL}/lottery`,giftId,{ headers: headers })
  }
 
  search(text: string): Observable<Gift[]> {
    const headers = this.authService.getHeaders();
    return this.http.get<Gift[]>(`${this.BASE_URL}/search?text=${text}`, { headers: headers });
  }


  getAllWithPurchases():Observable<Gift[]>{
    const headers = this.authService.getHeaders();
    return this.http.get<Gift[]>(`${this.BASE_URL}/giftsWithPurchases`, { headers: headers });
  }




  sort(sortBy: string, order: string): Observable<Gift[]> {
    const headers = this.authService.getHeaders();
    let params = new HttpParams()
      .set('sortBy', sortBy)
      .set('order', order);
    return this.http.get<Gift[]>(this.BASE_URL+'/sortBy', { params, headers });
  }

}

