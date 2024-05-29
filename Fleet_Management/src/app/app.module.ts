////import { NgModule } from '@angular/core';
////import { BrowserModule } from '@angular/platform-browser';
////import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
////import { MatTableModule } from '@angular/material/table';
////import { MatButtonModule } from '@angular/material/button';
////import { MatDialogModule } from '@angular/material/dialog';
////import { MatFormFieldModule } from '@angular/material/form-field';
////import { MatInputModule } from '@angular/material/input';
////import { MatSelectModule } from '@angular/material/select';
////import { HttpClientModule } from '@angular/common/http';
////import { FormsModule } from '@angular/forms';

////import { AppComponent } from './app.component';
////import { VehicleListComponent } from './vehicle-list/vehicle-list.component';
////import { VehicleDetailsComponent } from './vehicle-details/vehicle-details.component';
////import { AppRoutingModule } from './app-routing.module';


////@NgModule({
////  declarations: [
////    AppComponent,
////    VehicleListComponent,
////    VehicleDetailsComponent
////  ],
////  imports: [
////    BrowserModule,
////    BrowserAnimationsModule,
////    MatTableModule,
////    MatButtonModule,
////    MatDialogModule,
////    MatFormFieldModule,
////    MatInputModule,
////    MatSelectModule,
////    HttpClientModule,
////    FormsModule,
////    AppRoutingModule // Import AppRoutingModule here
////  ],
////  providers: [],
////  bootstrap: [AppComponent]
////})
////export class AppModule { }

//import { NgModule } from '@angular/core';
//import { BrowserModule } from '@angular/platform-browser';
//import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
//import { MatTableModule } from '@angular/material/table';
//import { MatButtonModule } from '@angular/material/button';
//import { MatDialogModule } from '@angular/material/dialog';
//import { MatFormFieldModule } from '@angular/material/form-field';
//import { MatInputModule } from '@angular/material/input';
//import { MatSelectModule } from '@angular/material/select';
//import { HttpClientModule } from '@angular/common/http';
//import { FormsModule, ReactiveFormsModule } from '@angular/forms';

//import { AppComponent } from './app.component';
//import { VehicleListComponent } from './vehicle-list/vehicle-list.component';
//import { VehicleDetailsComponent } from './vehicle-details/vehicle-details.component';
////import { DriverListComponent } from './driver-list/driver-list.component';
////import { RouteHistoryComponent } from './route-history/route-history.component';
////import { RouteHistoryListComponent } from './route-history-list/route-history-list.component';
////import { GeofenceListComponent } from './geofence-list/geofence-list.component';
//import { AppRoutingModule } from './app-routing.module';
//import { MatIconModule } from '@angular/material/icon';
//import { VehicleComponent } from './vehicle/vehicle.component';
//import { HeaderComponent } from './shared/header/header.component';

//@NgModule({
//  declarations: [
//    AppComponent,
//    VehicleListComponent,
//    VehicleDetailsComponent,
//    VehicleComponent,
//    HeaderComponent,
//    //DriverListComponent,
//    //RouteHistoryComponent,
//    //RouteHistoryListComponent,
//    //GeofenceListComponent
//  ],
//  imports: [
//    BrowserModule,
//    BrowserAnimationsModule,
//    MatTableModule,
//    MatButtonModule,
//    MatDialogModule,
//    MatFormFieldModule,
//    MatInputModule,
//    MatSelectModule,
//    HttpClientModule,
//    FormsModule,
//    ReactiveFormsModule, // Import ReactiveFormsModule here
//    AppRoutingModule ,// Import AppRoutingModule here
//    MatIconModule,

//  ],
//  providers: [],
//  bootstrap: [AppComponent]
//})
//export class AppModule { }


import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { VehicleListComponent } from './vehicle-list/vehicle-list.component';
import { VehicleDetailsComponent } from './vehicle-details/vehicle-details.component';
//import { DriverListComponent } from './driver-list/driver-list.component';
//import { RouteHistoryComponent } from './route-history/route-history.component';
//import { RouteHistoryListComponent } from './route-history-list/route-history-list.component';
//import { GeofenceListComponent } from './geofence-list/geofence-list.component';
import { AppRoutingModule } from './app-routing.module';
import { MatIconModule } from '@angular/material/icon';
import { VehicleComponent } from './vehicle/vehicle.component';
import { HeaderComponent } from './shared/header/header.component';
import { VehicleinformationComponent } from './vehicleinformation/vehicleinformation.component';
import { AddvehicleInfoComponent } from './addvehicle-info/addvehicle-info.component';
import { DriverComponent } from './driver/driver.component';
import { AddDriverComponent } from './add-driver/add-driver.component';
import { RoutehistoryComponent } from './routehistory/routehistory.component';
import { AddRoutehistoryComponent } from './add-routehistory/add-routehistory.component';
import { GeofencesComponent } from './geofences/geofences.component';

@NgModule({
  declarations: [
    AppComponent,
    VehicleListComponent,
    VehicleDetailsComponent,
    VehicleComponent,
    HeaderComponent,
    VehicleinformationComponent,
    AddvehicleInfoComponent,
    DriverComponent,
    AddDriverComponent,
    RoutehistoryComponent,
    AddRoutehistoryComponent,
    GeofencesComponent,
    //DriverListComponent,
    //RouteHistoryComponent,
    //RouteHistoryListComponent,
    //GeofenceListComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    MatTableModule,
    MatButtonModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule, // Import ReactiveFormsModule here
    AppRoutingModule,// Import AppRoutingModule here
    MatIconModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
