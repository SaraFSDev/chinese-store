// import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
// import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
// import { MatInputModule } from '@angular/material/input';
// import { MatButtonModule } from '@angular/material/button';
// import { MatCardModule } from '@angular/material/card';
// import { CommonModule } from '@angular/common';
// import { MatIconModule } from '@angular/material/icon';
// import { MatFormFieldModule } from '@angular/material/form-field';
// import { AuthService } from '../../../services/auth/auth.service';
// import { Router, RouterLink } from '@angular/router';


// @Component({
//   selector: 'app-login',
//   standalone: true,
//   imports: [
//     CommonModule,
//     ReactiveFormsModule,
//     MatInputModule,
//     MatButtonModule,
//     MatCardModule,
//     MatIconModule, MatFormFieldModule,RouterLink
//   ],
//   changeDetection: ChangeDetectionStrategy.OnPush,
//   templateUrl: './login.component.html',
//   styleUrls: ['./login.component.css'],
// })
// export class LoginComponent {
//   authSrv: AuthService = inject(AuthService)


//   loginForm: FormGroup;


//   constructor(private fb: FormBuilder, private router: Router) {
//     this.loginForm = this.fb.group({
//       email: ['', [Validators.required, Validators.email]],
//       password: ['', [Validators.required, Validators.minLength(6)]],
//       //Validators.pattern('^((?!.*[s])(?=.*[A-Z])(?=.*d).{6,99})')
//     });
//   }


//   onSubmit() {
//     if (this.loginForm.valid) {
//       this.authSrv.login(this.loginForm.value).subscribe({
//         next: (data) => {
//           this.authSrv.saveToken(data.token);
//           this.authSrv.saveRole(data.isAdmin);
//           this.router.navigate(['']);
//         },
//         error:() => {
//           alert("Login failed. Check your credentials and try again")
//         }
//       });
//     } else {
//       console.log('Form is invalid');
//     }
//   }


//   hide = signal(true)
//   clickEvent(event: MouseEvent) {
//     this.hide.set(!this.hide());
//     event.stopPropagation();
//   }
// }

import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { AuthService } from '../../../services/auth/auth.service';
import { Router, RouterLink } from '@angular/router';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule, MatFormFieldModule,RouterLink
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  authSrv: AuthService = inject(AuthService)


  loginForm: FormGroup;


  constructor(private fb: FormBuilder, private router: Router) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      //Validators.pattern('^((?!.*[s])(?=.*[A-Z])(?=.*d).{6,99})')
    });
  }


  onSubmit() {
    if (this.loginForm.valid) {
      this.authSrv.login(this.loginForm.value).subscribe({
        next: (data) => {
          this.authSrv.saveToken(data.token);
          this.authSrv.saveRole(data.isAdmin);
          this.router.navigate(['']);
        },
        error:() => {
          alert("Login failed. Check your credentials and try again")
        }
      });
    } else {
      console.log('Form is invalid');
    }
  }


  hide = signal(true)
  clickEvent(event: MouseEvent) {
    this.hide.set(!this.hide());
    event.stopPropagation();
  }
}

