import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ApiService } from '../api.service';
import { EventEmitter, Output } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import GVAR from '../model/GVAR';

@Component({
  selector: 'app-addvehicle-info',
  templateUrl: './addvehicle-info.component.html',
  styleUrl: './addvehicle-info.component.css'
})
export class AddvehicleInfoComponent implements OnInit {

  vehicles: any = {};
  drivers: any[] = [];
  command:any=""
  inputfields: any = {

  };
  vehicle: any={

  }
  purchaseDate: any = {

  }

  constructor(
    private apiService: ApiService,
    public dialogRef: MatDialogRef<AddvehicleInfoComponent>,
    private snackBar: MatSnackBar,

    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.vehicle = data.vehicle
    this.command=data.command
 
 
  }
  convertDateToTimestamp(dateString: string): number {
    const date = new Date(dateString);
   
    return date.getTime();


  }
  onSave(): void {
    this.vehicle.purchaseDate=this.convertDateToTimestamp(this.vehicle.purchaseDate);
    var gvar = new GVAR()
    if (this.command == "edit") {
      gvar.DicOfDic["Tags"] = {
        id: this.vehicle.id,
        vehicleid: this.vehicle.vehicleID,
        driverid: this.vehicle.driverID,
        vehiclemake: this.vehicle.vehicleMake,
        vehiclemodel: this.vehicle.vehicleModel,
        purchasedate: this.vehicle.purchaseDate,
      }
      this.apiService.updateVehicleDetails(this.vehicle.id, gvar).subscribe({
        next: () => {
          this.snackBar.open('Vehicle info updated successfully!', 'Close', { duration: 3000 });
          this.dialogRef.close();
        },
        error: () => {
          this.snackBar.open('Failed to update vehicle info.', 'Close', { duration: 3000 });
        }
      });


    }
    if (this.command == "add") {
      alert(this.command)

      gvar.DicOfDic["Tags"] = {
        vehicleid: this.vehicle.vehicleID,
        driverid: this.vehicle.driverID,
        vehiclemake: this.vehicle.vehicleMake,
        vehiclemodel: this.vehicle.vehicleModel,
        purchasedate: this.vehicle.purchaseDate,
      }
      console.log("add", gvar)
          this.apiService.addVehicleInformations(this.vehicle.id, gvar).subscribe({
            next: () => {
              this.snackBar.open('Vehicle info added successfully!', 'Close', { duration: 3000 });
              this.dialogRef.close();
            },
            error: () => {
              this.snackBar.open('Failed to added vehicle info.', 'Close', { duration: 3000 });
            }
          });
      console.log(gvar)
      

    }



  }



  ngOnInit(): void {
    this.loadVehicles();
    this.apiService.getDrivers().subscribe((gvar: GVAR) => {
      if (gvar.DicOfDT['drivers']) {
        this.drivers = gvar.DicOfDT['drivers'];
        this.inputfields = {
          ...this.inputfields,
          driverID: this.drivers[0].driverID
        }
      }
    });
  }
  onNoClick(): void {
    this.dialogRef.close();
  }


  loadVehicles(): void {
    this.apiService.getVehicles().subscribe((gvar: GVAR) => {
      if (gvar.DicOfDT['Vehicles']) {
        this.vehicles = gvar.DicOfDT['Vehicles'];
      }
    });
  }

}
