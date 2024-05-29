//import { Component, Inject, OnInit } from '@angular/core';
//import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
//import { ApiService } from '../api.service';

//@Component({
//  selector: 'app-vehicle-details',
//  templateUrl: './vehicle-details.component.html',
//  styleUrls: ['./vehicle-details.component.css']
//})
//export class VehicleDetailsComponent implements OnInit {
//  vehicle: any = {};
//  drivers: any[] = [];
//  inputfields: { [key: string]: any };

//  constructor(
//    private apiService: ApiService,
//    public dialogRef: MatDialogRef<VehicleDetailsComponent>,


//    @Inject(MAT_DIALOG_DATA) public data: any) {

//    //this.inputfields = {
//    //  VehicleNumber: data.vehicle.VehicleNumber||null,
//    //  VehicleType: data.vehicle.VehicleType || null,
//    //  PhoneNumber: data.vehicle.PhoneNumber || null,
//    //  VehicleMake: data.vehicle.VehicleMake || null,
//    //  VehicleModel: data.vehicle.VehicleModel || null,
//    //  LastGPSTime: data.vehicle.LastGPSTime || null,
//    //  LastGPSSpeed: data.vehicle.LastGPSSpeed || null,
//    //  LastAddress: data.vehicle.LastAddress || null
//    //};
//  }

//  ngOnInit(): void {
//    this.apiService.getVehicleDetails(this.data.vehicleId).subscribe((data: any) => {
//      this.vehicle = data.vehiclesInformations;
//    });

//    this.apiService.getDrivers().subscribe((data: any) => {
//      this.drivers = data.drivers;
//    });
//  }

//  onNoClick(): void {
//    this.dialogRef.close();
//  }
//}

import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ApiService } from '../api.service';
import { EventEmitter, Output } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import GVAR from '../model/GVAR';

@Component({
  selector: 'app-vehicle-details',
  templateUrl: './vehicle-details.component.html',
  styleUrls: ['./vehicle-details.component.css']
})
export class VehicleDetailsComponent implements OnInit {
  vehicle: any = {};
  drivers: any[] = [];
  inputfields: { [key: string]: any } = {};
  @Output() vehicleDeleted = new EventEmitter<void>();

  constructor(
    private apiService: ApiService,


    public dialogRef: MatDialogRef<VehicleDetailsComponent>,
    private snackBar: MatSnackBar,

    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    // Initialize inputfields with default or existing data
    //this.inputfields = {
    //  ID: data.vehicle?.ID || null,
    //  VehicleNumber: data.vehicle?.VehicleNumber || null,
    //  VehicleType: data.vehicle?.VehicleType || null,
    //  PhoneNumber: data.vehicle?.PhoneNumber || null,
    //  VehicleMake: data.vehicle?.VehicleMake || null,
    //  VehicleModel: data.vehicle?.VehicleModel || null,
    //  LastGPSTime: data.vehicle?.LastGPSTime || null,
    //  LastGPSSpeed: data.vehicle?.LastGPSSpeed || null,
    //  LastAddress: data.vehicle?.LastAddress || null,
    //};
  }

  ngOnInit(): void {
    this.apiService.getDrivers().subscribe((gvar: GVAR) => {
      if (gvar.DicOfDT['drivers']) {

        this.drivers = gvar.DicOfDT['drivers'];
        this.inputfields = {
          ...this.inputfields,
          driverID: this.drivers[0].driverID
        }
      }
    });



    this.apiService.getVehicleDetails(this.data.vehicleId).subscribe((gvar: GVAR) => {
      // Assuming that vehicle data is stored under a specific key in DicOfDT
      if (gvar.DicOfDic['VehiclesInformations']) {
        this.vehicle = gvar.DicOfDic['VehiclesInformations'];  // Access the first vehicle details if it's an array
        // Update inputfields with fetched data
        this.inputfields = {
          ID: this.vehicle.ID,
          VehicleNumber: this.vehicle.VehicleNumber,
          VehicleType: this.vehicle.VehicleType,
          PhoneNumber: this.vehicle.PhoneNumber,
          VehicleMake: this.vehicle.VehicleMake,
          VehicleModel: this.vehicle.VehicleModel,
          LastGPSTime: this.vehicle.LastGPSTime,
          LastGPSSpeed: this.vehicle.LastGPSSpeed,
          LastAddress: this.vehicle.LastAddress,

        };
      }
      else {
        console.error('No vehicle details available');
      }
    }, error => {
      console.error('Error loading vehicle details:', error);
    });

  }


  onNoClick(): void {
    this.dialogRef.close();
  }
  onSaveClick(): void {

    let updatedVehicle = {
      vehicleid: this.data.vehicleId,
      driverid: this.data.driverid,
      vehiclenumber: this.inputfields['VehicleNumber'],
      vehicletype: this.inputfields['VehicleType'],
      phonenumber: this.inputfields['PhoneNumber'],
      vehiclemake: this.inputfields['VehicleMake'],
      vehiclemodel: this.inputfields['VehicleModel'],
      lastgpstime: this.inputfields['LastGPSTime'],
      lastgpsspeed: this.inputfields['LastGPSSpeed'],
      lastaddress: this.inputfields['LastAddress'],
      ID: this.inputfields["ID"],

    };
    let gvar = new GVAR()
    gvar.DicOfDic["Tags"] = updatedVehicle
    alert(this.inputfields["driverID"])
    this.apiService.updateVehicle(this.data.vehicleId, gvar).subscribe(
      () => {

        this.apiService.updateVehicleDetails(this.inputfields["ID"] * 1, gvar).subscribe(
          () => {
            this.snackBar.open('Vehicle updated successfully!', 'Close', { duration: 3000 });
            this.dialogRef.close();
          },
          error => {
            this.snackBar.open('Failed to update vehicle!', 'Close', { duration: 3000 });
          }
        )

        //this.snackBar.open('Vehicle updated successfully!', 'Close', { duration: 3000 });

        //this.dialogRef.close();

      },
      error => {
        this.snackBar.open('Failed to update vehicle!', 'Close', { duration: 3000 });
      }
    );

  }

  onDeleteClick(): void {
    if (confirm('Are you sure you want to delete this vehicle?')) {
      this.apiService.deleteVehicle(this.data.vehicleId).subscribe(() => {
        this.dialogRef.close();
        this.vehicleDeleted.emit(); // Emit event after deletion
      })

    }
  }
}
