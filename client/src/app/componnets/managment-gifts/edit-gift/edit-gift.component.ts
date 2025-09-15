import { CommonModule, NgIf } from '@angular/common';
import { Component, EventEmitter, Input, Output, SimpleChanges, inject, Inject } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';//NgForm,, Validators
import { GiftService } from '../../../services/gift/gift.service';
import { Gift } from '../../../models/gift.model';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
// import { DonatorService } from '../../../services/donator/donator.service';
import { ActivatedRoute, Router } from '@angular/router';
import { log } from 'console';
import { Donator } from '../../../models/donator.model';
import { DonatorService } from '../../../services/donator/donator.service';
import { MatDialogActions, MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Category } from '../../../models/category.model';
import { CategoryService } from '../../../services/category/category.service';


@Component({
  selector: 'app-edit-gift',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf, FormsModule, MatInputModule, FormsModule, MatButtonModule, MatDividerModule, MatIconModule
  ,ReactiveFormsModule, NgIf, CommonModule, MatSelectModule,MatDialogModule,MatDialogActions],
  templateUrl: './edit-gift.component.html',
  styleUrl: './edit-gift.component.css'
})
export class EditGiftComponent {
  // srvGift: GiftService = inject(GiftService)
  // srvdonator: DonatorService = inject(DonatorService)
  // @Input()
  // id: number = 0
  // selectedGift: Gift = new Gift()
  // form: FormGroup
  // closeFormEdit: boolean = true
  // @Output()
  // closeChanges = new EventEmitter<any>()
  // donators: any = []


  srvGift: GiftService = inject(GiftService)
  srvdonator: DonatorService = inject(DonatorService)
  srvCategory: CategoryService = inject(CategoryService)


  id: number = 0
  selectedGift: Gift = new Gift()
  form: FormGroup
  donators: Donator[] = []
  categories: Category[] = []




  constructor(private dialogRef: MatDialogRef<EditGiftComponent>, @Inject(MAT_DIALOG_DATA) private data: any) {
    if (this.data && this.data.id) {
      this.id = this.data.id
    }
    this.form = new FormGroup({
      name: new FormControl(this.selectedGift.name, [Validators.required, Validators.minLength(2), Validators.maxLength(50)]),
      category: new FormControl(this.selectedGift.category, [Validators.required]),
      donator: new FormControl(this.selectedGift.donatorName, [Validators.required, Validators.minLength(2), Validators.maxLength(20)]),
      price: new FormControl(this.selectedGift.price, [Validators.required, Validators.max(150)]),
      image: new FormControl(this.selectedGift.image, [Validators.required])
    })
  }


  ngOnInit() {
    this.srvdonator.getAll().subscribe((data) => {
      this.donators = data
    })
    this.getAllCategory()
    this.getSelectedGift()
  }


  getAllCategory() {
    this.srvCategory.getAll().subscribe(data => {
      this.categories = data
    })
  }


  getSelectedGift() {
    this.srvGift.getById(this.id).subscribe(gift => {
      this.selectedGift = { ...gift }


      const selectedCategoryName = this.categories.find(c => c.id === this.selectedGift.categoryId)?.name || '';
      const selectedDonatorName = this.donators.find(d => d.id === this.selectedGift.donatorId)?.name || '';


      this.form.patchValue({
        name: this.selectedGift.name,
        category: selectedCategoryName,
        donator: selectedDonatorName,
        price: this.selectedGift.price,
        image: this.selectedGift.image
      });
      console.log(selectedCategoryName)
      console.log(selectedDonatorName)
      console.log(this.selectedGift)
    });
  }




  saveEditForm() {
    const donatorId = this.donators.find(d => d.name === this.form.value.donator)?.id;
    const categoryId = this.categories.find(c => c.name === this.form.value.category)?.id;
    const updatedGift = { ...this.selectedGift, ...this.form.value };
    updatedGift.donatorId = donatorId
    updatedGift.categoryId = categoryId
    console.log(updatedGift)
    this.srvGift.update(updatedGift).subscribe(() => {
      this.dialogRef.close(this.form.value);
    });
  }

}
