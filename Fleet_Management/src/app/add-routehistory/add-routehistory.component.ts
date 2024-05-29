
import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ApiService } from '../api.service';
import { EventEmitter, Output } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import GVAR from '../model/GVAR';

@Component({
  selector: 'app-add-routehistory',
  templateUrl: './add-routehistory.component.html',
  styleUrl: './add-routehistory.component.css'
})
export class AddRoutehistoryComponent implements OnInit {
  status: any
  date: any = "2024-05-23 02:00"
  history: any = {


  }
  vehicles:any=[]
  constructor(
    private apiService: ApiService,


    public dialogRef: MatDialogRef<AddRoutehistoryComponent>,
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
    this.loadVehicles()
  }
  onSave(): void {
    let newRouteHistory= {
      VehicleID: this.history.VehicleID,
      VehicleDirection: this.history.VehicleDirection,
      Status: this.status,
      VehicleSpeed: this.history.VehicleSpeed,
      Epoch: this.convertDateToEpoch(this.date),
      Address: this.history.Address,
      Latitude: this.history.Latitude,
      Longitude: this.history.Longitude,




    };
    let gvar = new GVAR()
    gvar.DicOfDic["Tags"] = newRouteHistory
    this.apiService.addRouteHistory(gvar).subscribe(
      () => {

          
        this.snackBar.open('route added successfully!', 'Close', { duration: 3000 });

        this.dialogRef.close();

      },
      error => {
        this.snackBar.open('Failed to update vehicle!', 'Close', { duration: 3000 });
      }
    );
  }
  loadVehicles(): void {

    this.apiService.getVehicles().subscribe((gvar: GVAR) => {
      if (gvar.DicOfDT['Vehicles']) {
        
        this.vehicles = gvar.DicOfDT['Vehicles'];

      }
    });
  }

  onNoClick(): void {
    this.dialogRef.close();

  }
  convertDateToEpoch(dateString: string): number {
    const date = new Date(dateString);
    return date.getTime();
  }

  
}
