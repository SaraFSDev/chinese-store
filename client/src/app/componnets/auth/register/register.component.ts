import { Component, inject, signal } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { AuthService } from '../../../services/auth/auth.service';
import { Router } from '@angular/router';
import { error } from 'node:console';




@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule, MatFormFieldModule
  ],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent {
  authSrv: AuthService = inject(AuthService)
  errorMessage = signal<string | null>(null);



  registerForm: FormGroup;




  constructor(private fb: FormBuilder, private router: Router) {
    this.registerForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&*(),.?":{}|<>]).{6,}$')]],
      confirmPassword: ['', Validators.required],
    }, { validator: this.passwordMatchValidator });
  }




  passwordMatchValidator(group: FormGroup): { [key: string]: boolean } | null {
    const password = group.get('password')?.value;
    const confirmPassword = group.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { mismatch: true };
  }

  onSubmit() {
    if (this.registerForm.valid) {
      this.authSrv.register(this.registerForm.value).subscribe({
        next: () => {
          this.router.navigate(['/login']);
        },
        error: (err) => {
          console.log('Error response:', err);
          if (err.error && typeof err.error === 'object' && err.error.error) {
            console.log(err.error.error.error.err);
            alert(err.error.error);
          } else if (err.error && typeof err.error === 'string') {
            alert(err.error); 
          } else {
            alert('An unexpected error occurred.');
          }
        },
      });
    } else {
      alert('The registration form is invalid.');
    }
  }
  
  
  


  // onSubmit() {
  //   if (this.registerForm.valid) {
  //     this.errorMessage.set(null); // 
  //     this.authSrv.register(this.registerForm.value).subscribe({
  //       next: () => {
  //         this.router.navigate(['/login']);
  //       },
  //       error: (err) => {
  //         if (err.error) {
  //           if (err.error == "Username is already taken.") {
  //             alert("Username is already taken");
  //           } else if (err.error == ("Email is already registered")) {
  //             alert("Email is already registered");
  //           } else {
  //             alert("Unexpected error occurred");
  //           }
  //         } else {
  //           alert("Unexpected error occurred");
  //         }
  //       }
  //     });
  //   } else {
  //     alert("Registration form is invalid");
  //   }
  // }


  hide = signal(true)
  clickEvent(event: MouseEvent) {
    this.hide.set(!this.hide());
    event.stopPropagation();
  }
}








