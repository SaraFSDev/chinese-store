import { CommonModule, NgIf } from '@angular/common';
import { Component, EventEmitter, Inject, Input, Output, SimpleChanges, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';  // הוספת ה-Injection של MAT_DIALOG_DATA
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { ActivatedRoute, Router } from '@angular/router';
import { DonatorService } from '../../../services/donator/donator.service';
import { Donator } from '../../../models/donator.model';


@Component({
  selector: 'app-edit-donator',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf, MatInputModule, MatSelectModule, MatButtonModule, MatDividerModule, MatIconModule],
  templateUrl: './edit-donator.component.html',
  styleUrls: ['./edit-donator.component.css']
})
export class EditDonatorComponent {
  srvDonator: DonatorService = inject(DonatorService);

  @Input() id: number = 0;  // אם תרצה שה-ID יוכל להתקבל גם כ-Input (לא חובה במקרה הזה)
  donator: Donator = new Donator();
  form: FormGroup;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    @Inject(MAT_DIALOG_DATA) public data: any,  // קבלת ה-ID דרך MAT_DIALOG_DATA
    private dialogRef: MatDialogRef<EditDonatorComponent>  // גישה לדיאלוג לסגירה אחרי השמירה
  ) {
    // כאן אנו בונים את הטופס
    this.form = new FormGroup({
      name: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]),
      phone: new FormControl('', [Validators.required, Validators.pattern(/^([0]\d{1,3}[-])?\d{7,10}$/)]),
      email: new FormControl('', [Validators.email]),
    });
  }

  ngOnInit(): void {
    if (this.data.id) {  // ודא שה-ID הגיע כראוי
      this.load(this.data.id);  // אם ה-ID קיים, טען את הנתונים
    }
  }

  load(id: number): void {
    this.srvDonator.getById(id).subscribe(data => {
      this.donator = data;
      // כאן אנו ממלאים את הטופס עם הערכים שהתקבלו
      this.form.patchValue({
        name: this.donator.name,
        phone: this.donator.phone,
        email: this.donator.email
      });
    });
  }



  saveUpdateForm(): void {
    if (this.form.valid) {
      // עדכון אובייקט ה-donator עם הערכים החדשים מה-form
      this.donator = { ...this.donator, ...this.form.value };
  
      // שליחה לעדכון ב-DB
      this.srvDonator.update(this.donator).subscribe(() => {
        this.dialogRef.close(true);  
      });
    }
  }
  closeDialog() {
    this.dialogRef.close(); // סגירת הדיאלוג בעת ביטול
  }  
}
