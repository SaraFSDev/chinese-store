import { Routes } from '@angular/router';
import { donatorComponent } from './componnets/donators/donator/donator.component';
import { NotFoundComponent } from './componnets/not-found/not-found.component';
import { AdddonatorComponent } from './componnets/donators/add-donator/add-donator.component';
import { AddGiftComponent } from './componnets/managment-gifts/add-gift/add-gift.component'; 
import { EditGiftComponent } from './componnets/managment-gifts/edit-gift/edit-gift.component'; 
import { EditDonatorComponent } from './componnets/donators/edit-donator/edit-donator.component';
import { LoginComponent } from './componnets/auth/login/login.component';
import { RegisterComponent } from './componnets/auth/register/register.component'; 
import { GiftsComponent } from './componnets/gifts/gifts.component';
import { BasketComponent } from './componnets/basket/basket.component';
import { GiftsDonatorComponent } from './componnets/donators/gifts-donator/gifts-donator.component';
import { PaymentComponent } from './componnets/payment/payment.component';
import { ManagmentGiftsComponent } from './componnets/managment-gifts/managment-gifts/managment-gifts.component';
import { PurchasesGiftComponent } from './componnets/purchases/purchases-gift/purchases-gift.component';
import { LayotComponent } from './componnets/layot/layot.component';
import { HomeComponent } from './componnets/home/home.component';
import { EndPaymentComponent } from './componnets/end-payment/end-payment.component';


// export const routes: Routes = [
//   // { path: '', component: HomeComponent }, 
//   {
//     path: 'magementGifts', component: ManagmentGiftsComponent,
//     children: [
//       { path: 'add', component: AddGiftComponent },
//       { path: 'edit', component: EditGiftComponent },
//     ],
//   },
//   {
//     path: 'donators', component: donatorComponent,
//     children: [
//       { path: 'add', component: AdddonatorComponent },
//       { path: 'edit', component: EditDonatorComponent },
//       { path: 'giftsDonator', component: GiftsDonatorComponent }
//     ],
//   },
//   { path: 'login', component: LoginComponent },
//   { path: 'register', component: RegisterComponent },
//   { path: 'gifts', component: GiftsComponent },
//   { path: 'basket', component: BasketComponent },
//   { path: 'payment', component: PaymentComponent },
//   { path: 'purchaseGift', component: PurchasesGiftComponent },
//   { path: '**', component: NotFoundComponent },
// ]


export const routes: Routes = [
  {
    path: '',
    component: LayotComponent,
    children: [
      // { path: '', component: HomeComponent },
      { path: '', component: GiftsComponent },
      { path: 'magementGifts', component: ManagmentGiftsComponent },
      // { path: 'donators', component: donatorComponent },
      {
            path: 'donators', component: donatorComponent,
            children: [
            
              { path: 'giftsDonator', component: GiftsDonatorComponent }
            ],
          },
      { path: 'gifts', component: GiftsComponent },
      { path: 'basket', component: BasketComponent },
      { path: 'payment', component: PaymentComponent },
      { path: 'endPayment', component: EndPaymentComponent,},
      { path: 'purchaseGift', component: PurchasesGiftComponent },
    ],
  },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: '**', component: NotFoundComponent },
];


