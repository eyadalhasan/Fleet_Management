import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VehicleListComponent } from './vehicle-list/vehicle-list.component';
import { VehicleDetailsComponent } from './vehicle-details/vehicle-details.component';
import { VehicleComponent } from './vehicle/vehicle.component';
import { VehicleinformationComponent } from './vehicleinformation/vehicleinformation.component';
//import { DriverDetailsComponent } from './driver-details/driver-details.component'; // Import DriverDetailsComponent
import { DriverComponent } from './driver/driver.component';
import { RoutehistoryComponent } from './routehistory/routehistory.component';
import { GeofencesComponent } from './geofences/geofences.component';

const routes: Routes = [
  { path: '', redirectTo: '/vehicles', pathMatch: 'full' },
  { path: 'vehicles', component: VehicleListComponent },
  { path: 'vehicleInformation', component: VehicleinformationComponent },
  { path: 'driverDetails', component: DriverComponent },
  { path: 'routehistory', component: RoutehistoryComponent },
  { path: 'Geofences', component: GeofencesComponent }


];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
