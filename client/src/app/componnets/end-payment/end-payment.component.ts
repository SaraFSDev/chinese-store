import { Component } from '@angular/core';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-end-payment',
  standalone: true,
  imports: [ CommonModule,MatIconModule],
  templateUrl: './end-payment.component.html',
  styleUrl: './end-payment.component.css'
})
export class EndPaymentComponent {

}
