// import { Component, SimpleChanges, inject } from '@angular/core';
// import { GiftService } from '../../../services/gift/gift.service';
// import { Gift } from '../../../models/gift.model';
// import { CommonModule } from '@angular/common';
// import { AddGiftComponent } from '../add-gift/add-gift.component';
// import { EditGiftComponent } from '../edit-gift/edit-gift.component';
// import { MatButtonModule } from '@angular/material/button';
// import { MatTableDataSource, MatTableModule } from '@angular/material/table';
// import { MatIconModule } from '@angular/material/icon';
// import { RouterLink, RouterOutlet } from '@angular/router';
// import { MatFormField, MatFormFieldModule, MatLabel } from '@angular/material/form-field';
// import { FormsModule } from '@angular/forms';
// import { MatInputModule } from '@angular/material/input';
// import { MatDialog, MatDialogModule } from '@angular/material/dialog';
// import { CategoryService } from '../../../services/category/category.service';
// import { ReportService } from '../../../services/report/report.service';
// import { Category } from '../../../models/category.model';

// // export interface PeriodicElement {
// //   name: string;
// //   position: number;
// //   weight: number;
// //   symbol: string;
// // }

// // @Component({
// //   selector: 'app-managment-gifts',
// //   standalone: true,
// //   imports: [CommonModule, AddGiftComponent, EditGiftComponent, MatTableModule, MatButtonModule, MatIconModule, RouterOutlet, RouterLink],
// //   templateUrl: './managment-gifts.component.html',
// //   styleUrl: './managment-gifts.component.css'
// // })
// // export class ManagmentGiftsComponent {
// //   displayedColumns: string[] = ['lottery','id', 'name', 'donatorName', 'price', 'delete', 'edit'];
// //   clickedRows = new Set<PeriodicElement>();
// //   srvGift: GiftService = inject(GiftService)

// //   dataSource: Gift[] = []
// //   passCreate: boolean = false
// //   passEdit: boolean = false
// //   passDelete: boolean = false
// //   selectedId: number = 0

// //   ngOnInit() {
// //     this.getAllWithDonator()
// //   }
// //   getAllWithDonator() {
// //     this.srvGift.getAllWithDonator().subscribe(data => {
// //       this.dataSource = data;
// //     })
// //   }
// //   changePassCreate(flag: boolean) {
// //     this.passCreate = flag
// //     this.getAllWithDonator()
// //   }

// //   changePassEdit(id: number) {
// //     this.selectedId = id
// //     this.passEdit = true
// //   }

// //   // <!-- this.router.navigate(['/target-route'], {
// //   //   queryParams: bodyParams
// //   // }); -->

// //   changePassDelete(id: number) {
// //     this.srvGift.delete(id).subscribe(() => {
// //       this.getAllWithDonator()
// //     })
// //   }

// //   closeFormEdit(flag: boolean) {
// //     this.passEdit = flag
// //     this.getAllWithDonator()
// //   }

// //   deletedItem(flag: boolean) {
// //     this.passDelete = flag
// //     this.getAllWithDonator()
// //   }

// //   lottery(giftId:number){
// //     this.srvGift.lottery(giftId).subscribe()
// //   }
// // }

// // import { Component, inject } from '@angular/core';
// // import { GiftService } from '../../../services/gift/gift.service';
// // import { Gift } from '../../../models/gift.model';
// // import { CommonModule } from '@angular/common';
// // import { CreateGiftComponent } from '../create-gift/create-gift.component';
// // import { EditGiftComponent } from '../edit-gift/edit-gift.component';
// // import { MatButtonModule } from '@angular/material/button';
// // import { MatTableDataSource, MatTableModule } from '@angular/material/table';
// // import { MatIconModule } from '@angular/material/icon';
// // import { Router, RouterLink, RouterOutlet } from '@angular/router';
// // import { MatFormField, MatFormFieldModule, MatLabel } from '@angular/material/form-field';
// // import { FormsModule } from '@angular/forms';
// // import { MatInputModule } from '@angular/material/input';
// // import { MatDialog, MatDialogModule } from '@angular/material/dialog';
// // import { Category } from '../../../models/category.model';
// // import { CategoryService } from '../../../services/category/category.service';
// // import { ReportService } from '../../../services/report/report.service';


// export interface PeriodicElement {
//   name: string;
//   position: number;
//   weight: number;
//   symbol: string;
// }


// @Component({
//   selector: 'appmanagment-gifts',
//   standalone: true,
//   imports: [CommonModule, MatTableModule, MatButtonModule,
//     MatIconModule, RouterOutlet, MatFormField, FormsModule, MatLabel, MatInputModule,
//     MatDialogModule, MatFormFieldModule,],
//   templateUrl: './managment-gifts.component.html',
//   styleUrl: './managment-gifts.component.css'
// })
// export class ManagmentGiftsComponent {
//   constructor(private dialog: MatDialog) { }


//   displayedColumns: string[] = ['lottery', 'id', 'name', 'category', 'donatorName','purchases', 'price', 'image', 'delete', 'edit'];
//   clickedRows = new Set<PeriodicElement>();
//   srvGift: GiftService = inject(GiftService)
//   srvCategory: CategoryService = inject(CategoryService)
//   srvReport: ReportService = inject(ReportService)


//   dataSource = new MatTableDataSource<Gift>([])
//   passCreate: boolean = false
//   passEdit: boolean = false
//   passDelete: boolean = false
//   selectedId: number = 0
//   searchText: string = ''
//   categories: Category[] = []


//   ngOnInit() {
//     console.log("getAllWithDonatorAndCategory");
//     this.getAllWithDonatorAndCategory()
//   }


//   getAllWithDonatorAndCategory() {
//     this.srvGift.getAllWithDonatorAndCategory().subscribe(data => {
//       this.dataSource.data = data;
//       console.log(data);
      
//     })
//   }


//   getAllCategory() {
//     this.srvCategory.getAll().subscribe(data => {
//       this.categories = data
//     })
//   }


//   deletedItem(id: number) {
//     this.srvGift.delete(id).subscribe(() => {
//       this.getAllWithDonatorAndCategory()
//     })
//   }


//   lottery(giftId: number) {
//     this.srvGift.lottery(giftId).subscribe()
//   }


//   search() {
//     if (this.searchText) {
//       console.log(this.searchText);
      
//       this.srvGift.search(this.searchText).subscribe((data) => {
//         this.dataSource.data = data;
//       });
//     }
//     else {
//       this.getAllWithDonatorAndCategory()
//     }
//   }


//   openAdd() {
//     const dialogRef = this.dialog.open(AddGiftComponent, {
//       width: '120%',
//       maxWidth: '580px',
//       height: 'auto',
//     });


//     dialogRef.afterClosed().subscribe((result) => {
//       if (result) {
//         this.getAllWithDonatorAndCategory()
//       }
//     });
//   }




//   openEdit(giftId: number) {
//     const dialogRef = this.dialog.open(EditGiftComponent, {
//       width: '120%',
//       maxWidth: '580px',
//       height: 'auto',
//       data: { id: giftId }
//     });


//     dialogRef.afterClosed().subscribe((result) => {
//       if (result) {
//         this.getAllWithDonatorAndCategory()
//       }
//     });
//   }


//   downloadWinnersReport() {
//     this.srvReport.exportWinnersReport().subscribe((data) => {
//       const blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
//       const link = document.createElement('a');
//       link.href = URL.createObjectURL(blob);
//       link.download = 'WinnersReport.xlsx';
//       link.click();
//     });
//   }


//   downloadRevenueReport(){
//     this.srvReport.exportRevenueReport().subscribe((data) => {
//       const blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
//       const link = document.createElement('a');
//       link.href = URL.createObjectURL(blob);
//       link.download = 'RevenueReport.xlsx';
//       link.click();
//     });
//   }
// }


import { Component, SimpleChanges, inject } from '@angular/core';
import { GiftService } from '../../../services/gift/gift.service';
import { Gift } from '../../../models/gift.model';
import { CommonModule } from '@angular/common';
import { AddGiftComponent } from '../add-gift/add-gift.component';
import { EditGiftComponent } from '../edit-gift/edit-gift.component';
import { MatButtonModule } from '@angular/material/button';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink, RouterOutlet } from '@angular/router';
import { MatFormField, MatFormFieldModule, MatLabel } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { CategoryService } from '../../../services/category/category.service';
import { ReportService } from '../../../services/report/report.service';
import { Category } from '../../../models/category.model';


export interface PeriodicElement {
  name: string;
  position: number;
  weight: number;
  symbol: string;
}

@Component({
  selector: 'appmanagment-gifts',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule,
    MatIconModule, RouterOutlet, MatFormField, FormsModule, MatLabel, MatInputModule,
    MatDialogModule, MatFormFieldModule,],
  templateUrl: './managment-gifts.component.html',
  styleUrl: './managment-gifts.component.css'
})
export class ManagmentGiftsComponent {
  constructor(private dialog: MatDialog) { }




  displayedColumns: string[] = ['lottery', 'id', 'name', 'category', 'donatorName','purchases', 'price', 'image', 'delete', 'edit'];
  clickedRows = new Set<PeriodicElement>();
  srvGift: GiftService = inject(GiftService)
  srvCategory: CategoryService = inject(CategoryService)
  srvReport: ReportService = inject(ReportService)




  dataSource = new MatTableDataSource<Gift>([])
  passCreate: boolean = false
  passEdit: boolean = false
  passDelete: boolean = false
  selectedId: number = 0
  searchText: string = ''
  categories: Category[] = []
  gifts:Gift[] = []
  winner:string=''


  ngOnInit() {
    this.getAllWithDonatorAndCategory()
  }




  getAllWithDonatorAndCategory() {
    this.srvGift.getAllWithDonatorAndCategory().subscribe(data => {
      this.dataSource.data = data;  
      this.gifts = data;
    })
  }




  getAllCategory() {
    this.srvCategory.getAll().subscribe(data => {
      this.categories = data
    })
  }




  // deletedItem(id: number) {
  //   this.srvGift.delete(id).subscribe(() => {
  //     this.getAllWithDonatorAndCategory()
  //   })
  // }
  deletedItem(id: number) {
    this.srvGift.delete(id).subscribe({
      next:()=>{
        this.getAllWithDonatorAndCategory()
      },
      error:()=>{
        alert("delete this gift failed. Check if someone bought it")
      }
    })
  }


  lottery(giftId: number) {
    this.srvGift.lottery(giftId).subscribe({
      next: (data: { winner: string }) => {
        const gift = this.gifts.find(g => g.id === giftId);
        if (gift) {
          gift.winner = data.winner;
          this.winner=gift.winner
          this.getAllWithDonatorAndCategory();
        }
      },
      error: (err) => {
        alert(`An error occurred`);
      } 
    });
  }
  
  
  search() {
    if (this.searchText?.trim()) {
      console.log(this.searchText);
  
      this.srvGift.search(this.searchText.trim()).subscribe({
        next: (data) => {
          this.dataSource.data = data;
        },
        error: (err) => {
          if (err.error?.message) {
            alert(err.error.message);
          } else {
            alert("An unexpected error occurred. Please try again later.");
          }
        }
      });
    } else {
      alert("Search field cannot be empty. Please enter a keyword to search.");
      this.getAllWithDonatorAndCategory();
    }
  }
  

  openAdd() {
    const dialogRef = this.dialog.open(AddGiftComponent, {
      width: '120%',
      maxWidth: '580px',
      height: 'auto',
    });


    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.getAllWithDonatorAndCategory()
      }
    });
  }

  openEdit(giftId: number) {
    const dialogRef = this.dialog.open(EditGiftComponent, {
      width: '120%',
      maxWidth: '580px',
      height: 'auto',
      data: { id: giftId }
    });




    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.getAllWithDonatorAndCategory()
      }
    });
  }




  downloadWinnersReport() {
    this.srvReport.exportWinnersReport().subscribe((data) => {
      const blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
      const link = document.createElement('a');
      link.href = URL.createObjectURL(blob);
      link.download = 'WinnersReport.xlsx';
      link.click();
    });
  }




  downloadRevenueReport(){
    this.srvReport.exportRevenueReport().subscribe((data) => {
      const blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
      const link = document.createElement('a');
      link.href = URL.createObjectURL(blob);
      link.download = 'RevenueReport.xlsx';
      link.click();
    });
  }
}







