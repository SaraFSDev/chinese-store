// import { NgFor, NgIf } from '@angular/common';
// import { Component, inject } from '@angular/core';
// import { MatButtonModule } from '@angular/material/button';
// import { MatCardModule } from '@angular/material/card';
// import { MatOption } from '@angular/material/core';
// import { MatIconModule } from '@angular/material/icon';
// import { MatFormField, MatLabel, MatSelect } from '@angular/material/select';
// import { BrowserModule } from '@angular/platform-browser';
// import { Gift } from '../../models/gift.model';
// import { BasketService } from '../../services/basket/basket.service';
// import { GiftService } from '../../services/gift/gift.service';

// @Component({
//   selector: 'app-gifts',
//   standalone: true,
//   imports: [MatCardModule, NgFor, MatIconModule, MatButtonModule,MatOption,MatSelect,MatLabel,MatFormField,NgIf],
//   templateUrl: './gifts.component.html',
//   styleUrl: './gifts.component.css'
// })
// export class GiftsComponent {
//   srvGift: GiftService = inject(GiftService)
//   srvBasket:BasketService=inject(BasketService)
//   sortBy: string = 'Price';
//   order: string = 'asc';

//   gifts: Gift[] = []


//   ngOnInit() {
//     this.getAll()
//   }
//   getAll() {
//     this.srvGift.getAll().subscribe(data => {
//       this.gifts = data;
//     })
//   }

//   // addToBasket1(giftId:number) {
//   //   this.srvBasket.addToBasket(giftId).subscribe()
//   // }
//   sort(): void {
//     this.srvGift.sort(this.sortBy, this.order).subscribe((data: Gift[]) => {   
//       this.gifts = data;
//     });
//   }

//   addToBasket(giftId: number){
//     this.srvGift.getById(giftId).subscribe(data => {
//       if (data.isLotteryCompleted) {
//         alert("Cannot add gift to basket, lottery has been completed.");
//       } else {
//         this.srvBasket.addToBasket(giftId).subscribe(() => {
//         });
//       }
//     });
//   }

// }

import { CommonModule, NgFor, NgIf } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatOption } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';
import { MatFormField, MatLabel, MatSelect } from '@angular/material/select';
import { BrowserModule } from '@angular/platform-browser';
import { Gift } from '../../models/gift.model';
import { BasketService } from '../../services/basket/basket.service';
import { GiftService } from '../../services/gift/gift.service';
import { AuthService } from '../../services/auth/auth.service';


@Component({
  selector: 'app-gifts',
  standalone: true,
  imports: [MatCardModule, NgFor, MatIconModule, MatButtonModule,MatOption,MatSelect,MatLabel,MatFormField,
    NgIf,CommonModule],
  templateUrl: './gifts.component.html',
  styleUrl: './gifts.component.css'
})
export class GiftsComponent {
  srvGift: GiftService = inject(GiftService)
  srvBasket:BasketService=inject(BasketService)
  srvAuth: AuthService = inject(AuthService)

  sortBy: string = 'Price';
  order: string = 'asc';


  gifts: Gift[] = []




  ngOnInit() {
    this.getAll()
  }
  getAll() {
    this.srvGift.getAll().subscribe(data => {
      this.gifts = data;
    })
  }


  // addToBasket1(giftId:number) {
  //   this.srvBasket.addToBasket(giftId).subscribe()
  // }
  sort(): void {
    this.srvGift.sort(this.sortBy, this.order).subscribe((data: Gift[]) => {
      console.log(this.gifts);
      
      this.gifts = data;
    });
  }

//   sort(): void {
//     this.srvGift.sort(this.sortBy, this.order).subscribe((data: Gift[]) => {   
//       this.gifts = data;
//     });
//   }

  addToBasket(giftId: number){
    const isAdmin = this.srvAuth.isAdmin()
    if(!isAdmin){
    this.srvGift.getById(giftId).subscribe(data => {
      if (data.isLotteryCompleted) {
        alert("Cannot add gift to basket, lottery has been completed.");
      } else {
        this.srvBasket.addToBasket(giftId).subscribe(() => {
        });
      }
    });}
    else{
      alert("Manager can't add gifts to the basket.")
    }
  }


}





