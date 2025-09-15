import { Component, inject, Output, EventEmitter, signal, ChangeDetectionStrategy } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Gift } from '../../../models/gift.model';
import { GiftService } from '../../../services/gift/gift.service';
import { CommonModule, NgIf } from '@angular/common';
// import { DonatorService } from '../../../services/donator/donator.service';
// import { donator } from '../../../models/donator.model';
import { ErrorStateMatcher } from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { DonatorService } from '../../../services/donator/donator.service';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { Category } from '../../../models/category.model';
import { CategoryService } from '../../../services/category/category.service';

@Component({
  selector: 'app-add-gift',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf, MatFormFieldModule, MatInputModule, MatSelectModule,
    FormsModule, MatButtonModule, MatDividerModule, MatIconModule,MatDialogModule,CommonModule ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './add-gift.component.html',
  styleUrl: './add-gift.component.css'
})
export class AddGiftComponent {

  srvGift: GiftService = inject(GiftService)
  srvdonator: DonatorService = inject(DonatorService)
  srvCategory: CategoryService = inject(CategoryService)




  protected readonly value = signal('');


  protected onInput(event: Event) {
    this.value.set((event.target as HTMLInputElement).value);
  }


  form: FormGroup


  donators: any = []
  categories: Category[] = []


  constructor(private dialogRef: MatDialogRef<AddGiftComponent>) {
    this.form = new FormGroup({
      name: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]),
      category: new FormControl('', [Validators.required]),
      donator: new FormControl('', [Validators.required]),
      price: new FormControl(10, [Validators.required, Validators.min(10), Validators.max(150)]),
      image: new FormControl('', [Validators.required]),
    })
  }


  ngOnInit() {
    this.srvdonator.getAll().subscribe((data) => {
      this.donators = data
    })
    this.getAllCategory()
  }
  getAllCategory() {
    this.srvCategory.getAll().subscribe(data => {
      this.categories = data
    })
  }


  saveForm() {
    console.log("saveForm");
    if (this.form.invalid) {
      console.log("invalid form");
      let gift: Gift = new Gift()
      gift.name = this.form.controls['name'].value
      gift.donatorName = this.form.controls['donator'].value
      gift.price = this.form.controls['price'].value
      console.log(gift);
    }
    else {
      let gift: Gift = new Gift()
      gift.name = this.form.controls['name'].value
      gift.donatorName = this.form.controls['donator'].value
      gift.category = this.form.controls['category'].value
      gift.price = this.form.controls['price'].value
      gift.image = this.form.controls['image'].value
      console.log(gift);
      this.srvGift.create(gift).subscribe(() => {

        // next: (data: { winner: string }) => {
        //   const gift = this.gifts.find(g => g.id === giftId);
        //   if (gift) {
        //     gift.winner = data.winner;
        //     this.winner=gift.winner
        //     this.getAllWithDonatorAndCategory();
        //   }
        // },
        // error: (err) => {
        //   const errorMessage = err.error?.Error || err.error?.message || err.message || 'An unknown error occurred';
        //   alert(`An error occurred: ${errorMessage}`);
        //   console.error("Error in lottery:", err);
        // }
        this.dialogRef.close(this.form.value);
      })
    }
  }
}
