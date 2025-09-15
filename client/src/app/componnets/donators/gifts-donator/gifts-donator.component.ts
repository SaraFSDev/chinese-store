// import { NgFor } from '@angular/common';
// import { Component, inject } from '@angular/core';
// import { ActivatedRoute } from '@angular/router';
// import { Donator } from '../../../models/donator.model';
// import { DonatorService } from '../../../services/donator/donator.service';

// @Component({
//   selector: 'app-gifts-donator',
//   standalone: true,
//   imports: [NgFor],
//   templateUrl: './gifts-donator.component.html',
//   styleUrl: './gifts-donator.component.css'
// })
// export class GiftsDonatorComponent {
//   srvDonator:DonatorService=inject(DonatorService)
//   // dataSource: string[]=[];
//   dataSource: any = {};
//   id: number=0;
//   constructor(private route: ActivatedRoute) { }
  
//   ngOnInit() {
//     console.log("init");
//     this.route.queryParams.subscribe(params => {
//       this.id = +params['id'];
//       this.getAll(this.id);
//     });  // סוגר סוגריים חסר כאן
//   }  // סוגר סוגריים חסר כאן ל-ngOnInit
  



//   getAll(id:number) {
//     console.log("all");
//     this.srvDonator.GetDonatorGiftsAsync(id).subscribe(data => {
//     this.dataSource = data;    
//     })
//   }

  
// }


import { NgFor, NgIf } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatCard, MatCardContent, MatCardTitle } from '@angular/material/card';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { DonatorService } from '../../../services/donator/donator.service';

@Component({
  selector: 'app-gifts-donator',
  standalone: true,
  imports: [NgFor,MatCardTitle,RouterLink,MatCardContent,MatCard,NgIf],
  templateUrl: './gifts-donator.component.html',
  styleUrls: ['./gifts-donator.component.css']
})
export class GiftsDonatorComponent {
  srvDonator: DonatorService = inject(DonatorService);
  dataSource: any = {};  // נתונים שיתעדכנו במבנה הנכון
  id: number = 0;
  donatorName: string = ''; // משתנה לשם התורם

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    console.log("init");
    this.route.queryParams.subscribe(params => {
      this.id = +params['id'];
      this.getAll(this.id);
    });
  }

  // getAll(id: number) {
  //   console.log("all");
  //   this.srvDonator.GetDonatorGiftsAsync(id).subscribe(data => {
  //     this.dataSource = data;
  //     // this.donatorName = data.donatorName; // שמירת שם התורם
  //   });
  // }

  getAll(id: number) {
    console.log("all");
    this.srvDonator.GetDonatorGiftsAsync(id).subscribe(data => {
      console.log({data});  // הוספת log למבנה הנתונים
      this.dataSource = data;
      if (data && this.dataSource.donatorName && this.dataSource.gifts) {
        this.donatorName = this.dataSource.donatorName;
        console.log(this.dataSource.donatorName);
         // שמירת שם התורם
      } else {
        console.error('donatorName not found in the response data');
      }
    });
  }
  
}


