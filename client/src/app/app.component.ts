import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ManagmentGiftsComponent } from './componnets/managment-gifts/managment-gifts/managment-gifts.component';
import { donatorComponent } from "./componnets/donators/donator/donator.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, ManagmentGiftsComponent, donatorComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'project';
}
