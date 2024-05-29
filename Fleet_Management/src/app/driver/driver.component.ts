//import { Component } from '@angular/core';
//import { ApiService } from '../api.service';
//import { MatDialog } from '@angular/material/dialog';
//import { VehicleDetailsComponent } from '../vehicle-details/vehicle-details.component';
//import GVAR from '../model/GVAR';
//import { MatSnackBar } from '@angular/material/snack-bar';
//import { MatDialogRef } from '@angular/material/dialog';
//import { EventEmitter, Output } from '@angular/core';
//import { VehicleComponent } from '../vehicle/vehicle.component';
//import { Route, Router } from '@angular/router';
//@Component({
//  selector: 'app-driver',
//  templateUrl: './driver.component.html',
//  styleUrl: './driver.component.css'
//})
//export class DriverComponent {
//  drivers: any = {}


//  displayedColumns: string[] = ['driverID', 'driverName', 'phoneNumber', "delete", "edit", "add"];
//  deleteDriver(id: number): void {

//  }
//  editDriver(driver: any): void {

//  }
//  addDriver(driver: any): void {

//  }
//  constructor(private apiService: ApiService, public dialog: MatDialog, public router: Router) {


//  }
//  ngOnInit(): void {
//    this.loadDrivers();
//  }
//  loadDrivers(): void {
//    this.apiService.getDrivers().subscribe((gvar: GVAR) => {
//      if (gvar.DicOfDT['drivers']) {
//        this.drivers = gvar.DicOfDT['drivers'];
//        console.log("drivers", this.drivers)
//      }
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
import { AddDriverComponent } from '../add-driver/add-driver.component';
@Component({
  selector: 'app-driver',
  templateUrl: './driver.component.html',
  styleUrls: ['./driver.component.css']
})
export class DriverComponent implements OnInit {
  drivers: any[] = [];

  displayedColumns: string[] = ['driverID', 'driverName', 'phoneNumber', 'delete', 'edit', 'add'];

  constructor(
    private apiService: ApiService,
    public dialog: MatDialog,
    public router: Router,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.loadDrivers();
  }

  loadDrivers(): void {
    this.apiService.getDrivers().subscribe((gvar: GVAR) => {
      if (gvar.DicOfDT && gvar.DicOfDT['drivers']) {
        this.drivers = gvar.DicOfDT['drivers'];
        console.log('drivers', this.drivers);
      }
    });
  }

  deleteDriver(id: number): void {
    // Implement delete functionality here
    if (confirm('Are you sure you want to delete this Driver?')) {
      this.apiService.deletDriver(id).subscribe(() => {
        this.loadDrivers();
      });
    }
  }

  editDriver(driver: any): void {
    // Implement edit functionality here
    console.log(`Editing driver: ${driver}`);
    // Example: open dialog to edit driver details
    const dialogRef = this.dialog.open(AddDriverComponent, {
      width: '250px',
      data: { driver,command:"edit" }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      this.loadDrivers();
    });
  }

  addDriver(driver: any): void {
    console.log(`Editing driver: ${driver}`);
    // Example: open dialog to edit driver details
    const dialogRef = this.dialog.open(AddDriverComponent, {
      width: '250px',
      data: { driver, command: "add" }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      this.loadDrivers();
    });
  }
}

