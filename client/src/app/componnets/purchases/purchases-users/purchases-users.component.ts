import { Component, inject } from '@angular/core';
import { MatDialogActions, MatDialogContent, MatDialogRef } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { GiftUser } from '../../../models/giftUser.model';
import { BasketService } from '../../../services/basket/basket.service';

@Component({
  selector: 'app-purchases-users',
  standalone: true,
  imports: [MatDialogActions, MatTableModule, MatDialogContent],
  templateUrl: './purchases-users.component.html',
  styleUrl: './purchases-users.component.css'
})
export class PurchasesUsersComponent {

  constructor(public dialogRef: MatDialogRef<PurchasesUsersComponent>) { }


  srvBasket: BasketService = inject(BasketService)


  giftId: number = 0;
  buyers: GiftUser[] = [];


  ngOnInit(): void {
    this.getBuyers()
  }

  getBuyers() {
    if (this.giftId > 0) {
      this.srvBasket.getGiftBuyers(this.giftId).subscribe((data) => {
        this.buyers = data;
        console.log(data)
      });
    }
  }

  closeDialog(): void {
    this.dialogRef.close();
  }
}
