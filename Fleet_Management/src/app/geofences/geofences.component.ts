import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { ApiService } from '../api.service';
import { MatDialog } from '@angular/material/dialog';
import GVAR from '../model/GVAR';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialogRef } from '@angular/material/dialog';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-geofences',
  templateUrl: './geofences.component.html',
  styleUrls: ['./geofences.component.css']
})
export class GeofencesComponent implements OnInit {
  geofences: any[] = [];
  displayedColumns: string[] = ['geofenceID', 'geofenceType', 'addedDate', 'strokeColor', 'strokeOpacity', 'strokeWeight', 'fillColor', 'fillOpacity'];
  @Output() geofenceDeleted = new EventEmitter<void>();

  constructor(private apiService: ApiService, public dialog: MatDialog, public router: Router) { }

  ngOnInit(): void {
    this.loadGeofences();

  }

  loadGeofences(): void {
    this.apiService.getGeofences().subscribe((gvar: GVAR) => {
      if (gvar.DicOfDT['Geofences']) {
        this.geofences = gvar.DicOfDT['Geofences'];
      }
      console.log(this.geofences)
    });
  }



}
