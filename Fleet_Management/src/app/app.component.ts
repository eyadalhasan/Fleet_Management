import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  colors = ["red", "black", "Brown"];

  name: string = "Eyad Alhasan";
  handleClick() {
    alert("clicked")
  };
}
