import { Component, EventEmitter, inject, Output, signal } from '@angular/core'
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms'
import { Donator } from '../../../models/donator.model'
import { MatFormFieldModule } from '@angular/material/form-field'
import { MatInputModule } from '@angular/material/input'
import { MatButtonModule } from '@angular/material/button'
import { MatIconModule } from '@angular/material/icon'
import { ActivatedRoute, Router, RouterLink } from '@angular/router'
import { DonatorService } from '../../../services/donator/donator.service'
import { MatDialogRef } from '@angular/material/dialog'
import { CommonModule } from '@angular/common'

@Component({
  selector: 'app-add-donator',
  standalone: true,
  imports: [MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule, RouterLink,ReactiveFormsModule,CommonModule],
  templateUrl: './add-donator.component.html',
  styleUrls: ['./add-donator.component.css']
})
export class AdddonatorComponent {
  srvdonator: DonatorService = inject(DonatorService)

  protected readonly value = signal('');
  protected onInput(event: Event) {
    this.value.set((event.target as HTMLInputElement).value);
  }

  form: FormGroup
  donators: any = []
  @Output() saved: EventEmitter<void> = new EventEmitter();

  constructor(private router: Router, private route: ActivatedRoute, private dialogRef: MatDialogRef<AdddonatorComponent>) {
    this.form = new FormGroup({
      name: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]),
      phone: new FormControl('', [Validators.minLength(9), Validators.pattern(/^([0]\d{1,3}[-])?\d{7,10}$/)]),
      email: new FormControl('', [Validators.email])
    })
  }

  ngOnInit() {
    this.srvdonator.getAll().subscribe((data) => {
      this.donators = data
    })
  }

  // פונקציה לשמירת הנתונים והניווט לדף התורמים
  saveForm() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    let donator: Donator = new Donator()
    donator.name = this.form.controls['name'].value
    donator.phone = this.form.controls['phone'].value
    donator.email = this.form.controls['email'].value

    this.srvdonator.create(donator).subscribe(() => {
      console.log(donator);
      
      this.saved.emit(); 
      this.router.navigate(['/donators']);
      this.dialogRef.close(true); 
    })
  }


  closeDialog() {
    this.dialogRef.close(); 
  }
}
