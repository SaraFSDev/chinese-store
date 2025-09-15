import { CommonModule, NgFor } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatCard, MatCardContent, MatCardHeader, MatCardTitle } from '@angular/material/card';
import { MatDialog } from '@angular/material/dialog';
import { MatToolbarModule } from '@angular/material/toolbar';
import { Gift } from '../../../models/gift.model';
import { GiftService } from '../../../services/gift/gift.service';
import { PurchasesUsersComponent } from '../purchases-users/purchases-users.component';


@Component({
  selector: 'app-purchases-gift',
  standalone: true,
  imports: [NgFor, CommonModule, MatCardHeader, MatToolbarModule,NgFor, CommonModule, MatCard, MatCardTitle, MatCardContent],
  templateUrl: './purchases-gift.component.html',
  styleUrl: './purchases-gift.component.css'
})
export class PurchasesGiftComponent {
  constructor(public dialog: MatDialog) { }


  srvGift: GiftService = inject(GiftService)
  gifts: Gift[] = [];
  order: string = 'desc';
  ngOnInit() {
    this.getAll()
  }

  getAll() {
    this.srvGift.getAllWithPurchases().subscribe(data => {
      this.gifts = data
    })
  }


  showDetails(giftId: number) {
    const dialogRef = this.dialog.open(PurchasesUsersComponent, {
      width: '600px',
      data: { giftId: giftId }
    });
    dialogRef.componentInstance.giftId = giftId;
  }


  sortGifts(criteria: string) {
    this.srvGift.sort(criteria,this.order).subscribe(data => {
      console.log("data"+data)
      this.gifts = data
      console.log(this.gifts)
    })
  }

}
