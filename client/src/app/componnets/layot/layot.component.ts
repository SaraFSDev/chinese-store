import { Component, inject } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { MatToolbar } from '@angular/material/toolbar';
import { AuthService } from '../../services/auth/auth.service';
import { NgIf } from '@angular/common';
import { RouterLinkActive, RouterModule } from '@angular/router';

@Component({
  selector: 'app-layot',
  standalone: true,
  imports: [MatToolbar, MatIcon, NgIf, RouterModule, RouterLinkActive],
  templateUrl: './layot.component.html',
  styleUrl: './layot.component.css'
})
export class LayotComponent {
  srvAuth: AuthService = inject(AuthService)

  get isAuthenticated(): boolean {
    return this.srvAuth.isAuthenticated()
  }

  get isAdmin(): boolean {
    return this.srvAuth.isAdmin()
   }

  logout(): void {
    this.srvAuth.logout();
  }
}
