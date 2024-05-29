  import { Component, Inject } from '@angular/core';
  import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
  import { ApiService } from '../api.service';
  import { MatSnackBar } from '@angular/material/snack-bar';
  import GVAR from '../model/GVAR';

  @Component({
    selector: 'app-vehicle',
    templateUrl: './vehicle.component.html',
    styleUrls: ['./vehicle.component.css']  // Correct the typo from styleUrl to styleUrls
  })
  export class VehicleComponent {
    vehicle: any;  // Declare vehicle property
    command:any
    constructor(
      private apiService: ApiService,
      public dialogRef: MatDialogRef<VehicleComponent>,
      private snackBar: MatSnackBar,
      @Inject(MAT_DIALOG_DATA) public data: any
    ) {
      this.vehicle = data.vehicle;
      // Initialize vehicle with injected data
      this.command=data.command
      console.log(data);
    }

    onSave(): void {
      // Create a new instance of GVAR
      let gvar = new GVAR();

      // Prepare the Tags dictionary with vehicle data
  

      gvar.DicOfDic["Tags"] = {
        vehicleid: this.vehicle.vehicleID,
        vehiclenumber: this.vehicle.vehicleNumber,
        vehicletype: this.vehicle.vehicleType
      };

      // Call the updateVehicle method of the ApiService with the prepared GVAR object
      if (this.command == "edit") {
        gvar.DicOfDic["Tags"] = {
          vehicleid: this.vehicle.vehicleID,
          vehiclenumber: this.vehicle.vehicleNumber,
          vehicletype: this.vehicle.vehicleType
        };

        this.apiService.updateVehicle(this.vehicle.vehicleID, gvar).subscribe({
          next: () => {
            this.snackBar.open('Vehicle updated successfully!', 'Close', { duration: 3000 });
            this.dialogRef.close();
          },
          error: () => {
            this.snackBar.open('Failed to update vehicle.', 'Close', { duration: 3000 });
          }
        });
      }
      else if (this.command = "add") {
        gvar.DicOfDic["Tags"] = {
          vehiclenumber: this.vehicle.vehicleNumber,
          vehicletype: this.vehicle.vehicleType
        };
        this.apiService.addVehicle(gvar).subscribe({
          next: () => {
            this.snackBar.open('Vehicle added successfully!', 'Close', { duration: 3000 });
            this.dialogRef.close();
          },
          error: () => {
            this.snackBar.open('Failed to add vehicle.', 'Close', { duration: 3000 });
          }
        });
      }

    }


    onNoClick(): void {
      this.dialogRef.close();
    }
  }
