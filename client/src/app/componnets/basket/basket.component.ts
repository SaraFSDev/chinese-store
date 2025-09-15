// import { NgFor, NgIf } from '@angular/common';
// import { Component, inject, NgModule } from '@angular/core';
// import { FormsModule, NgModel } from '@angular/forms';
// import { MatButtonModule } from '@angular/material/button';
// import { MatCardModule } from '@angular/material/card';
// import { Gift } from '../../models/gift.model';
// import { GiftUser } from '../../models/giftUser.model';
// import { BasketService } from '../../services/basket/basket.service';
// import { Router } from '@angular/router';

// @Component({
//   selector: 'app-basket',
//   standalone: true,
//   imports: [MatCardModule, MatButtonModule, NgFor, NgIf, FormsModule],
//   templateUrl: './basket.component.html',
//   styleUrl: './basket.component.css'
// })
// export class BasketComponent {

//   gifts: GiftUser[] = []
//   srvBasket: BasketService = inject(BasketService)
//   quantity: number = 1;
//   // router: any;

//   constructor(private router: Router) {}
//   ngOnInit() {
//     this.getAll()
//   }

//   getAll() {
//     this.srvBasket.getBasket().subscribe((data) => {
//       this.gifts = data
//       console.log(data);
      
//     })
//   }

//   deleteBasket(giftId: number): void {
//     this.srvBasket.removeFromBasket(giftId).subscribe(() => {
//       this.getAll()
//     })
//   }

  

//   decreaseQuantity(giftId: number) {
//     const gift = this.gifts.find(g => g.giftId === giftId);
//     if (gift && gift.quantity > 1) {
//       gift.quantity--;
//       this.srvBasket.updateBasket(giftId, gift.quantity).subscribe(() => {
//       });
//     }
//   }
  
//   increaseQuantity(giftId: number) {
//     const gift = this.gifts.find(g => g.giftId === giftId);
//     if (gift) {
//       gift.quantity++;
//       console.log( gift.quantity);   
//       this.srvBasket.updateBasket(giftId, gift.quantity).subscribe(() => {
//       });
//     }
//   }
 
//   payment(){
//     this.router.navigate(['/payment'])
//   }
  
// }

import { NgFor, NgIf } from '@angular/common';
import { Component, inject, NgModule } from '@angular/core';
import { FormsModule, NgModel } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { Gift } from '../../models/gift.model';
import { GiftUser } from '../../models/giftUser.model';
import { BasketService } from '../../services/basket/basket.service';
import { Router } from '@angular/router';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-basket',
  standalone: true,
  imports: [MatCardModule, MatButtonModule, NgFor, NgIf, FormsModule,MatIcon],
  templateUrl: './basket.component.html',
  styleUrl: './basket.component.css'
})
export class BasketComponent {

  gifts: GiftUser[] = [];
  srvBasket: BasketService = inject(BasketService);
  quantity: number = 1;

  constructor(private router: Router) {}

  ngOnInit() {
    this.getAll();
  }

  // getAll() {
  //   this.srvBasket.getBasket().subscribe({
  //     next: (data) => {
  //       this.gifts = data;
  //       console.log(data);
  //     },
  //     error: (err) => {
  //       console.error('Error occurred while fetching basket items:', err);
  //       if (err.error && typeof err.error === 'object' && err.error.error) {
  //         alert(err.error.error); 
  //       } else if (err.error && typeof err.error === 'string') {
  //         alert(err.error); // מציג אם השגיאה היא מחרוזת ישירות
  //       } else {
  //         alert('Error occurred while fetching basket items. Please try again.'); // שגיאה כללית
  //       }
  //     }
  //   });
  // }

  // getAll() {
  //   this.srvBasket.getBasket().subscribe({
  //     next: (data) => {
  //       this.gifts = data;
  //       console.log(data);
  //     },
  //     error: (err) => {
  //       console.error('Error occurred while fetching basket items:', err);
  //       // טיפול בשגיאות
  //       if (err.error && typeof err.error === 'object' && err.error.error) {
  //         alert(err.error.error); // מציג את השגיאה במקרה שהיא אובייקט
  //       } else if (err.error && typeof err.error === 'string') {
  //         alert(err.error); // מציג את השגיאה במקרה שהיא מחרוזת ישירות
  //       } else {
  //         alert('Error occurred while fetching basket items. Please try again.'); // שגיאה כללית
  //       }
  //     }
  //   });
  // }
  
  getAll() {
    this.srvBasket.getBasket().subscribe({
      next: (data) => {
        this.gifts = data;
        console.log(data);
      },
      error: (err) => {
        alert("You need to logIn!")
      }
    });
  }



  deleteBasket(giftId: number): void {
    this.srvBasket.removeFromBasket(giftId).subscribe({
      next: () => {
        this.getAll();  // Refresh basket after removal
      },
      error: (err) => {
        // console.error('Error occurred while removing gift from basket:', err);
        alert('Cannot remove gift with ID ' + giftId + ' because it is purchased.');
      }
    });
  }


  decreaseQuantity(giftId: number) {
    const gift = this.gifts.find(g => g.giftId === giftId);
    if (gift && gift.quantity > 1) {
      gift.quantity--;
      this.srvBasket.updateBasket(giftId, gift.quantity).subscribe({
        next: () => {
          // this.getAll(); // Uncomment to refresh after update if needed
        },
        error: (err) => {
          console.error('Error occurred while decreasing quantity:', err);
          // תופס את השגיאה ומציג את המידע המתאים
          if (err.error && typeof err.error === 'object' && err.error.error) {
            alert(err.error.error); // מציג את תוכן השדה "error"
          } else if (err.error && typeof err.error === 'string') {
            alert(err.error); // מציג אם השגיאה היא מחרוזת ישירות
          } else {
            alert('Error occurred while decreasing quantity. Please try again.'); // שגיאה כללית
          }
        }
      });
    }
  }

  increaseQuantity(giftId: number) {
    const gift = this.gifts.find(g => g.giftId === giftId);
    if (gift) {
      gift.quantity++;
      console.log(gift.quantity);
      this.srvBasket.updateBasket(giftId, gift.quantity).subscribe({
        next: () => {
          // this.getAll(); // Uncomment to refresh after update if needed
        },
        error: (err) => {
          console.error('Error occurred while increasing quantity:', err);
          // תופס את השגיאה ומציג את המידע המתאים
          if (err.error && typeof err.error === 'object' && err.error.error) {
            alert(err.error.error); // מציג את תוכן השדה "error"
          } else if (err.error && typeof err.error === 'string') {
            alert(err.error); // מציג אם השגיאה היא מחרוזת ישירות
          } else {
            alert('Error occurred while increasing quantity. Please try again.');
          }
        }
      });
    }
  }

  payment() {
    this.router.navigate(['/payment']);
  }
}
