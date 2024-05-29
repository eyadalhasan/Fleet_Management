import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ApiService } from '../api.service';
import { EventEmitter, Output } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import GVAR from '../model/GVAR';
@Component({
  selector: 'app-add-driver',
  templateUrl: './add-driver.component.html',
  styleUrl: './add-driver.component.css'
})
export class AddDriverComponent {
  drivre: any
  command: any
  constructor(
    private apiService: ApiService,
    public dialogRef: MatDialogRef<AddDriverComponent>,
    private snackBar: MatSnackBar,

    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.driver = data.driver
    this.command = data.command


  }
  driver: any = {

  }
  onSave(): void {

    var gvar = new GVAR()
    alert(this.command)
    if (this.command == "edit") {

      gvar.DicOfDic["Tags"] = {
        phoneNumber: this.driver.phoneNumber,
        driverName: this.driver.driverName,
        driverid: this.driver.driverID,

      }
      console.log(gvar)

      this.apiService.updateDriver(this.driver.driverID, gvar).subscribe({
        next: () => {
          this.snackBar.open('Driver  edited successfully!', 'Close', { duration: 3000 });
          this.dialogRef.close();
        },
        error: () => {
          this.snackBar.open('Failed to edit driver info.', 'Close', { duration: 3000 });
        }
      });

    }

    else {



      if (this.command == "add") {

        gvar.DicOfDic["Tags"] = {
          phonenumber: this.driver.phoneNumber,
          drivername: this.driver.driverName,

        }
        console.log(gvar)

        this.apiService.addDriver(gvar).subscribe({
          next: () => {
            this.snackBar.open('Driver  added successfully!', 'Close', { duration: 3000 });
            this.dialogRef.close();
          },
          error: () => {
            this.snackBar.open('Failed to add driver info.', 'Close', { duration: 3000 });
          }
        });

      }

    }
  }
  onNoClick(): void {
    this.dialogRef.close()
  }


}
