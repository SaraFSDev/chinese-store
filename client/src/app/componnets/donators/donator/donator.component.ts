// import { Component, inject,Inject, signal, Signal } from '@angular/core';
// // import { DonatorService } from '../../../services/donator/donator.service';
// import { Donator } from '../../../models/donator.model';
// import { MatTableModule } from '@angular/material/table';
// import { MatButtonModule } from '@angular/material/button';
// import { MatIconModule } from '@angular/material/icon';
// import { CommonModule } from '@angular/common';
// import { Router, RouterLink, RouterOutlet } from '@angular/router';
// import { DonatorService } from '../../../services/donator/donator.service';
// import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
// import { AdddonatorComponent } from '../add-donator/add-donator.component';


// @Component({
//   selector: 'app-donator',
//   standalone: true,
//   imports: [CommonModule, MatTableModule, MatButtonModule, MatIconModule, RouterLink, RouterOutlet],
//   templateUrl: './donator.component.html',
//   styleUrl: './donator.component.css'
// })
// export class donatorComponent {
//   // dataSource: Signal<any> = signal(null);
//   srvdonator: DonatorService = inject(DonatorService)


//   displayedColumns: string[] = ['gifts','id', 'name', 'phone', 'email', 'delete', 'edit'];

//   dataSource: Donator[] = []
//   passCreate: boolean = false
//   passEdit: boolean = false
//   passDelete: boolean = false
//   selectedId: number = 0

//   constructor(private router: Router, public dialogRef: MatDialogRef<donatorComponent>,
//     @Inject(MAT_DIALOG_DATA) public data: any) { }

//   ngOnInit() {
//     console.log("init");
    
//     this.getAll()
//   }

//   getAll() {
//     console.log("all");
//     this.srvdonator.getAll().subscribe(data => {
//     this.dataSource = data;
      
//     })
//   }
//   changePassCreate(flag: boolean) {
//     this.passCreate = flag
//     this.getAll()
//   }


//   changePassEdit(id: number) {
//     this.selectedId = id
//     this.passEdit = true
//   }

//   changePassDelete(id: number) {
//     this.srvdonator.delete(id).subscribe(() => {
//       this.getAll()
//     })
//   }

//   closeFormEdit(flag: boolean) {
//     this.passEdit = flag
//     this.getAll()
//   }

//   deletedItem(flag: boolean) {
//     this.passDelete = flag
//     this.getAll()
//   }

//   filterByName(){ 
//     this.srvdonator.getByName().subscribe((data)=>{
//       console.log("sort",data);
//       this.dataSource=data
//     })
//   }

//   filterByEmail(){ 
//     this.srvdonator.getByEmail().subscribe((data)=>{
//       this.dataSource=data
//     })
//   }

//   closeDialog(): void {
//     this.dialogRef.close();
//   }
//   openDialog(){
//     const dialogRef = this.dialog.open(AdddonatorComponent, {
//       width: '90%', 
//       maxWidth: '550px',
//       height: 'auto',
//     });

//     dialogRef.afterClosed().subscribe((result:any) => {
//       if (result) {
//           this.getAll()
//       }
//     });
//   }

//   lottery(gifts:any){

//   }
// }

import { Component, inject } from '@angular/core';
import { DonatorService } from '../../../services/donator/donator.service';
import { Donator } from '../../../models/donator.model';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { AdddonatorComponent } from '../add-donator/add-donator.component'; // ודא שייבאת את הקומפוננטה נכון
import { EditDonatorComponent } from '../edit-donator/edit-donator.component';

@Component({
  selector: 'app-donator',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule, MatIconModule, RouterLink, RouterOutlet],
  templateUrl: './donator.component.html',
  styleUrls: ['./donator.component.css']
})
export class donatorComponent {
  srvdonator: DonatorService = inject(DonatorService);
  displayedColumns: string[] = ['gifts', 'id', 'name', 'phone', 'email', 'delete', 'edit'];
  dataSource: Donator[] = [];
  selectedId: number = 0;
  passCreate: boolean = false
  passEdit: boolean = false
  passDelete: boolean = false

  constructor(private router: Router, private dialog: MatDialog) { } // הוספת dialog כשרת

  ngOnInit() {
    this.getAll();
  }

  // getAll() {
  //   this.srvdonator.getAll().subscribe(data => {
  //     this.dataSource = data;
  //   });
  // }

  getAll() {
    this.srvdonator.getAll().subscribe({
      next: (data) => {
        this.dataSource = data;
      },
      error: (err) => {
        alert(`Error while fetching donators: ${err}`); // Displaying error message
      }
    });
  }

  changePassEdit(id: number) {
    this.selectedId = id;
    this.passEdit = true;
  }

  changePassDelete(id: number) {
    this.srvdonator.delete(id).subscribe(() => {
      this.getAll();
    });
  }

  closeFormEdit(flag: boolean) {
    this.passEdit = flag;
    this.getAll();
  }

  deletedItem(flag: boolean) {
    this.passDelete = flag;
    this.getAll();
  }

  // filterByName() {
  //   this.srvdonator.getByName().subscribe((data) => {
  //     console.log('sort', data);
  //     this.dataSource = data;
  //   });
  // }

  // filterByEmail() {
  //   this.srvdonator.getByEmail().subscribe((data) => {
  //     this.dataSource = data;
  //   });
  // }
  filterByName() {
    this.srvdonator.getByName().subscribe({
      next: (data) => {
        console.log('Filtered by name', data);
        this.dataSource = data;
      },
      error: (err) => {
        alert(`Error filtering by name: ${err}`); // Show error alert
      }
    });
  }
  
  filterByEmail() {
    this.srvdonator.getByEmail().subscribe({
      next: (data) => {
        console.log('Filtered by email', data);
        this.dataSource = data;
      },
      error: (err) => {
        alert(`Error filtering by email: ${err}`); // Show error alert
      }
    });
  }
  
  openDialogAdd() {
    const dialogRef = this.dialog.open(AdddonatorComponent, {
      width: '1200%',
      maxWidth: '550px',
      height: 'auto',
    });

    dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        console.log("res");
        
        this.getAll();
      }
    });
  }

  // openDialogEdit(id: number) {
  //   const dialogRef = this.dialog.open(EditDonatorComponent, {
  //     width: '120%',
  //     maxWidth: '550px',
  //     height: 'auto',
  //     data: { id: id }  // העברת ה-ID
  //   });
  
  //   dialogRef.afterClosed().subscribe((result: any) => {
  //     if (result) {
  //       this.getAll();
  //     }
  //   });
  // }
  
  openDialogEdit(id: number) {
    const dialogRef = this.dialog.open(EditDonatorComponent, {
      width: '90%', // יכול להתאים להקטנה או הגדלה
      maxWidth: '550px',
      height: 'auto',
      data: { id: id }  // העברת ה-ID של התורם
    });
  
    dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        this.getAll(); // אם נערך משהו, עדכן את רשימת התורמים
      }
    });
}

  
  closeDialog(): void {
    this.dialog.closeAll(); // אם יש דיאלוג פעיל
  }

  lottery(gifts: any) {
  }
}

