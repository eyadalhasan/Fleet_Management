import { Component, OnInit } from '@angular/core';
import { ApiService } from '../api.service';
import { MatDialog } from '@angular/material/dialog';
import { VehicleDetailsComponent } from '../vehicle-details/vehicle-details.component';
import GVAR from '../model/GVAR';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialogRef } from '@angular/material/dialog';
import { EventEmitter, Output } from '@angular/core';
import { VehicleComponent } from '../vehicle/vehicle.component';
import { Route, Router } from '@angular/router';
import { WebSocketService } from '../web-socket.service';
@Component({
  selector: 'app-vehicle-list',
  templateUrl: './vehicle-list.component.html',
  styleUrls: ['./vehicle-list.component.css']
})
export class VehicleListComponent implements OnInit {
  vehicles: any[] = [];
  displayedColumns: string[] = ['vehicleID', 'vehicleNumber', 'vehicleType', 'lastDirection', 'lastStatus', 'lastAddress', 'lastLatitude', 'lastLongitude', 'showMore', 'delete', 'edit', "add"];
  @Output() vehicleDeleted = new EventEmitter<void>();


  constructor(private apiService: ApiService, public dialog: MatDialog, public router: Router, private webSocketService: WebSocketService) { }


  ngOnInit(): void {
    this.loadVehicles();

    this.webSocketService.onMessage().subscribe((message: string) => {
      if (message === 'New vehicle added') {
        this.loadVehicles();
      }    });


  }

  loadVehicles(): void {
    this.apiService.getVehicles().subscribe((gvar: GVAR) => {
      if (gvar.DicOfDT['Vehicles']) {
        this.vehicles = gvar.DicOfDT['Vehicles'];
      }
    });
  }
  editVehicle(vehicle: any): void {
    const dialogRef = this.dialog.open(VehicleComponent, {
      width: '500px',
      data: { vehicle, command: "edit" } // Passing the entire vehicle object
    });

    dialogRef.afterClosed().subscribe(result => {
      this.loadVehicles(); // Reload the vehicle list to reflect any changes
    });
  }
  addVehicle(vehicle: any): void {
    const dialogRef = this.dialog.open(VehicleComponent, {
      width: '500px',
      data: { vehicle, command: "add" } // Passing the entire vehicle object
    });

    dialogRef.afterClosed().subscribe(result => {
      this.loadVehicles(); // Reload the vehicle list to reflect any changes
    });
  }

  deleteVehicle(vehicleId: number): void {
    if (confirm('Are you sure you want to delete this vehicle?')) {
      this.apiService.deleteVehicle(vehicleId).subscribe(() => {
        this.vehicleDeleted.emit(); // Emit event after deletion
        this.loadVehicles(); // Reload vehicles after deletion

      })
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




