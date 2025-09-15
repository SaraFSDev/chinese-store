import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatError, MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core'; // לתמיכה בתאריכים מקומיים
import { NgIf } from '@angular/common';
import { BasketService } from '../../services/basket/basket.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-payment',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatError,
    NgIf
  ],
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.css']
})
export class PaymentComponent {
  paymentForm: FormGroup;
  private fb = inject(FormBuilder);
  srvBasket: BasketService = inject(BasketService)

  constructor(private router: Router) {
    this.paymentForm = this.fb.group({
      cardholderName : ['', Validators.required],
      cardNumber: ['', [Validators.required, Validators.pattern(/^\d{16}$/)]],
      expirationDate: ['', Validators.required],
      cvv: ['', [Validators.required, Validators.pattern(/^\d{3}$/)]],
    });
  }
  setMonthAndYear(normalizedMonthAndYear: Date, datepicker: any) {
    const ctrlValue = this.paymentForm.get('expirationDate')!.value;
    const newDate = ctrlValue ? new Date(ctrlValue) : new Date();
    
    newDate.setMonth(normalizedMonthAndYear.getMonth());
    newDate.setFullYear(normalizedMonthAndYear.getFullYear());
    newDate.setDate(1);  
    this.paymentForm.get('expirationDate')!.setValue(newDate);
    datepicker.close();
  }

  onSubmit() {
    this.srvBasket.confirmPurchase().subscribe(()=>{
      this.router.navigate(['endPayment'])
    })
    }


}
