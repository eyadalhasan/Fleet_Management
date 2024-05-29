import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { ApiService } from '../api.service';
import { MatDialog } from '@angular/material/dialog';
import { VehicleDetailsComponent } from '../vehicle-details/vehicle-details.component';
import GVAR from '../model/GVAR';
import { MatSnackBar } from '@angular/material/snack-bar';
import { VehicleComponent } from '../vehicle/vehicle.component';
import { Router } from '@angular/router';
import { AddvehicleInfoComponent } from '../addvehicle-info/addvehicle-info.component';
import { WebSocketService } from '../web-socket.service';
@Component({
  selector: 'app-vehicleinformation',
  templateUrl: './vehicleinformation.component.html',
  styleUrls: ['./vehicleinformation.component.css']
})
export class VehicleinformationComponent implements OnInit {
  vehicles: any[] = [];
  displayedColumns: string[] = ['vehicleID', 'driverID', 'vehcileMake', 'vehcileModel', 'purchaseDate', 'Delete', 'edit', 'add'];
  @Output() vehicleDeleted = new EventEmitter<void>();
  constructor(private apiService: ApiService, public dialog: MatDialog, public router: Router, private webSocketService: WebSocketService) { }

  ngOnInit(): void {
    this.loadVehicles();
    this.webSocketService.onMessage().subscribe((message: string) => {
      if (message == 'New vehicleinfo added') {
        this.loadVehicles();
      }
    });

  }

  loadVehicles(): void {
    this.apiService.getVehiclesInformation().subscribe((gvar: GVAR) => {
      if (gvar.DicOfDT['VehcileInformations']) {
        this.vehicles = gvar.DicOfDT['VehcileInformations'];
      }
    });
  }

  editVehicle(vehicle: any): void {
    const dialogRef = this.dialog.open(AddvehicleInfoComponent, {
      width: '500px',
      data: { vehicle, command: 'edit' }
    });

    dialogRef.afterClosed().subscribe(result => {
      this.loadVehicles(); // Reload the vehicle list to reflect any changes
    });
  }

  addVehicle(vehicle: any): void {
    const dialogRef = this.dialog.open(AddvehicleInfoComponent, {
      width: '500px',
      data: { vehicle, command: 'add' }
    });

    dialogRef.afterClosed().subscribe(result => {
      this.loadVehicles(); // Reload the vehicle list to reflect any changes
    });
  }

  deleteVehicleInformation(vehicleId: number): void {
    alert(vehicleId)
    if (confirm('Are you sure you want to delete this vehicle-information?')) {
      this.apiService.deleteVehicleInformation(vehicleId).subscribe(() => {
        this.vehicleDeleted.emit(); // Emit event after deletion
        this.loadVehicles(); // Reload vehicles after deletion
      });
    }
  }

  showDetails(vehicleId: number): void {
    const dialogRef = this.dialog.open(VehicleDetailsComponent, {
      width: '500px',
      data: { vehicleId }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
    dialogRef.componentInstance.vehicleDeleted.subscribe(() => {
      this.loadVehicles(); // Reload vehicles after deletion
    });
  }


}
