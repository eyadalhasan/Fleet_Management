import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'] // Correct typo from styleUrl to styleUrls
})
export class HeaderComponent {
  constructor(private router: Router) { }

  clickVehicleInformation(): void {
    this.router.navigate(['/vehicleInformation']); // Navigate to Vehicle Information
  }

  clickDriverDetails(): void {
    this.router.navigate(['/driverDetails']); // Navigate to Driver Details
  }
  clickVehicle(): void {
    this.router.navigate(['/']); // Navigate to Driver Details

  }
  clickRouteHistory(): void {
    this.router.navigate(['/routehistory']); // Navigate to Driver Details

  }
  clickGeofences(): void {
    this.router.navigate(['/Geofences']); // Navigate to Driver Details

  }
}
