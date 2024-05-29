//import { Component, OnInit } from '@angular/core';
//import { ApiService } from '../api.service';
//import { MatDialog } from '@angular/material/dialog';
//import { Router } from '@angular/router';
//import GVAR from '../model/GVAR';
//import { MatSnackBar } from '@angular/material/snack-bar';
//import { VehicleDetailsComponent } from '../vehicle-details/vehicle-details.component';
//import { AddRoutehistoryComponent } from '../add-routehistory/add-routehistory.component';

//@Component({
//  selector: 'app-routehistory',
//  templateUrl: './routehistory.component.html',
//  styleUrls: ['./routehistory.component.css']
//})
//export class RoutehistoryComponent implements OnInit {
//  routeHistories: any[] = [];
//  displayedColumns: string[] = ['vehicleID', 'vehicleNumber', 'vehicleDirection', 'status', 'address', 'latitude', 'longitude', 'gpsSpeed', 'gpsTime', 'showMore',  'add'];
//  vehiclesID: any
//  startEpoch: any="0001-02-01"
//  endEpoch: any ="2022-02-01"
//  vehicleId: any
//  vehicles: any
//  vehicle: any = {

//  }
//  startDate: any
//  endDate:any

//  constructor(private apiService: ApiService, public dialog: MatDialog, public router: Router) { }

//  ngOnInit(): void {
//    this.loadVehicles()
//    this.startDate = this.convertDateToEpoch(this.startEpoch);

//    this.endDate = this.convertDateToEpoch(this.endEpoch);
//    this.loadRouteHistories(11, this.startDate, this.endDate); // Reload the route history list to reflect any changes
//  }

//  loadRouteHistories(vehicleId: any, startEpoch: any, endEpoch: any): void {

//    this.apiService.getRouteHistories(vehicleId, startEpoch, endEpoch).subscribe((gvar: any) => {
//      if (gvar.DicOfDT && gvar.DicOfDT['RouteHistory']) {
//        this.routeHistories = gvar.DicOfDT['RouteHistory'];
//      }
//      console.log("dicofdt", gvar.DicOfDT)

//    });
//  }

//  changeVehcileID(): void {
//  }
//   convertDateToEpoch(dateString: string): number {
//  const date = new Date(dateString);
//  return date.getTime();
//}

//  fetch(): void {
//    alert(this.startEpoch)
//    this.startDate = this.convertDateToEpoch(this.startEpoch);

//    this.endDate = this.convertDateToEpoch(this.endEpoch);

//    this.loadRouteHistories(this.vehicle.vehicleID, this.startDate, this.endDate);
//    console.log(this.routeHistories)

//  }
//  loadVehicles(): void {

//    this.apiService.getVehicles().subscribe((gvar: GVAR) => {
//      if (gvar.DicOfDT['Vehicles']) {
//        this.vehicles = gvar.DicOfDT['Vehicles'];

//      }
//    });
//  }

//  editVehicle(vehicle: any): void {
//    const dialogRef = this.dialog.open(AddRoutehistoryComponent, {
//      width: '500px',
//      data: { vehicle, command: "edit" } // Passing the entire vehicle object
//    });

//    dialogRef.afterClosed().subscribe(result => {
//      this.loadRouteHistories(this.vehicle.vehicleID, this.convertDateToEpoch(this.startEpoch), this.convertDateToEpoch(this.endEpoch)); // Reload the route history list to reflect any changes
//    });
//  }

//  addVehicle(vehicle: any): void {
//    const dialogRef = this.dialog.open(AddRoutehistoryComponent, {
//      width: '500px',
//      data: { vehicle, command: "add" } // Passing the entire vehicle object
//    });

//    dialogRef.afterClosed().subscribe(result => {
//      this.loadRouteHistories(this.vehicle.vehicleID, this.convertDateToEpoch(this.startEpoch), this.convertDateToEpoch(this.endEpoch)); // Reload the route history list to reflect any changes
//    });
//  }

//  deleteRoute(vehicleId: number): void {
//    if (confirm('Are you sure you want to delete this vehicle?')) {
//      this.apiService.deleteVehicle(vehicleId).subscribe(() => {
//        this.loadRouteHistories(this.vehiclesID, this.startEpoch, this.endEpoch); // Reload the route history list to reflect any changes
//      });
//    }
//  }

//  showDetails(vehicleId: number): void {
//    const dialogRef = this.dialog.open(VehicleDetailsComponent, {
//      width: '500px',
//      data: { vehicleId }
//    });

//    dialogRef.afterClosed().subscribe(result => {
//      console.log('The dialog was closed');
//    });
//    dialogRef.componentInstance.vehicleDeleted.subscribe(() => {
//      this.loadRouteHistories('11', 'startEpoch', 'endEpoch'); // Reload route histories after deletion
//    });
//  }
//}

import { Component, OnInit } from '@angular/core';
import { ApiService } from '../api.service';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import GVAR from '../model/GVAR';
import { MatSnackBar } from '@angular/material/snack-bar';
import { VehicleDetailsComponent } from '../vehicle-details/vehicle-details.component';
import { AddRoutehistoryComponent } from '../add-routehistory/add-routehistory.component';

@Component({
  selector: 'app-routehistory',
  templateUrl: './routehistory.component.html',
  styleUrls: ['./routehistory.component.css']
})
export class RoutehistoryComponent implements OnInit {
  routeHistories: any[] = [];
  displayedColumns: string[] = ['vehicleID', 'vehicleNumber', 'vehicleDirection', 'status', 'address', 'latitude', 'longitude', 'gpsSpeed', 'gpsTime', 'showMore',  'add'];
  vehiclesID: any;
  startEpoch: any = "2024-05-23 02:00";
  endEpoch: any = "2025-12-01 02:00";


  //startEpoch: any

  //endEpoch: any


  vehicleId: any;
  vehicles: any;
  vehicle: any = {};

  startDate: any;
  endDate: any;

  constructor(private apiService: ApiService, public dialog: MatDialog, public router: Router) { }

  ngOnInit(): void {
    this.loadVehicles();
    this.startDate = this.convertDateToEpoch(this.startEpoch);
    this.endDate = this.convertDateToEpoch(this.endEpoch);
    this.loadRouteHistories(11, this.startDate, this.endDate); // Reload the route history list to reflect any changes
  }

  loadRouteHistories(vehicleId: any, startEpoch: any, endEpoch: any): void {
    this.apiService.getRouteHistories(vehicleId, startEpoch, endEpoch).subscribe((gvar: any) => {
      if (gvar.DicOfDT && gvar.DicOfDT['RouteHistory']) {
        this.routeHistories = gvar.DicOfDT['RouteHistory'];
        this.sortRouteHistories(); // Sort the route histories by epoch time
      }
      console.log("dicofdt", gvar.DicOfDT);
    });
  }
  add() {
    this.addVehicle({})
  }

  sortRouteHistories(): void {
    this.routeHistories.sort((a, b) => a.gpsTime - b.gpsTime);
  }

  changeVehcileID(): void {
    // Implement change vehicle ID logic here if needed
  }

  convertDateToEpoch(dateString: string): number {
    const date = new Date(dateString);
    return date.getTime();
  }

  fetch(): void {
    alert(this.startEpoch);
    this.startDate = this.convertDateToEpoch(this.startEpoch);
    this.endDate = this.convertDateToEpoch(this.endEpoch);
    this.loadRouteHistories(this.vehicle.vehicleID, this.startDate, this.endDate);
    console.log(this.routeHistories);
  }

  loadVehicles(): void {
    this.apiService.getVehicles().subscribe((gvar: GVAR) => {
      if (gvar.DicOfDT['Vehicles']) {
        this.vehicles = gvar.DicOfDT['Vehicles'];
      }
    });
  }

  editVehicle(vehicle: any): void {
    const dialogRef = this.dialog.open(AddRoutehistoryComponent, {
      width: '500px',
      data: { vehicle, command: "edit" } // Passing the entire vehicle object
    });

    dialogRef.afterClosed().subscribe(result => {
      this.loadRouteHistories(this.vehicle.vehicleID, this.convertDateToEpoch(this.startEpoch), this.convertDateToEpoch(this.endEpoch)); // Reload the route history list to reflect any changes
      this.sortRouteHistories(); // Sort the route histories after reloading
    });
  }

  addVehicle(vehicle: any): void {
    const dialogRef = this.dialog.open(AddRoutehistoryComponent, {
      width: '500px',
      data: { vehicle, command: "add" } // Passing the entire vehicle object
    });

    dialogRef.afterClosed().subscribe(result => {
      this.sortRouteHistories(); // Sort the route histories after reloading

      this.loadRouteHistories(this.vehicle.vehicleID, this.convertDateToEpoch(this.startEpoch), this.convertDateToEpoch(this.endEpoch)); // Reload the route history list to reflect any changes
    });
  }

  deleteRoute(vehicleId: number): void {
    if (confirm('Are you sure you want to delete this vehicle?')) {
      this.apiService.deleteVehicle(vehicleId).subscribe(() => {
        this.loadRouteHistories(this.vehiclesID, this.startEpoch, this.endEpoch); // Reload the route history list to reflect any changes
        this.sortRouteHistories(); // Sort the route histories after reloading
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
      this.loadRouteHistories('11', this.startEpoch, this.endEpoch); // Reload route histories after deletion
      this.sortRouteHistories(); // Sort the route histories after reloading
    });
  }
}

