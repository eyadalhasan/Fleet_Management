import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import GVAR from './model/GVAR';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl = 'https://localhost:7251/api'; // Update with your API base URL

  constructor(private http: HttpClient) { }



  getVehicleDetails(vehicleId: number): Observable<any> {

    return this.http.get<{ sts: number, gvar: any }>(`${this.baseUrl}/vehiclesinformations/${vehicleId}`).pipe(
      map(response => {
        if (response.sts == 1) {

          let gvar = new GVAR();
          gvar.DicOfDic = response.gvar.dicOfDic || {};
          gvar.DicOfDT = response.gvar.dicOfDT || {};
          return gvar;
        } else {
          throw new Error('Failed to load vehicles. Status code: ' + response.sts);
        }
      })
    );
  }

  getDrivers(): Observable<GVAR> {

    return this.http.get<{ sts: number, gvar: any }>(`${this.baseUrl}/drivers`).pipe(
      map(response => {
        if (response.sts == 1) {

          let gvar = new GVAR();
          gvar.DicOfDic = response.gvar.dicOfDic || {};
          gvar.DicOfDT = response.gvar.dicOfDT || {};
          return gvar;
        } else {
          throw new Error('Failed to load vehicles. Status code: ' + response.sts);
        }
      })
    );
  }
  getVehicles(): Observable<GVAR> {
    return this.http.get<{ sts: number, gvar: any }>(`${this.baseUrl}/vehicle`).pipe(
      map(response => {
        if (response.sts == 1) {

          let gvar = new GVAR();
          gvar.DicOfDic = response.gvar.dicOfDic || {};
          gvar.DicOfDT = response.gvar.dicOfDT || {};
          return gvar;
        } else {
          throw new Error('Failed to load vehicles. Status code: ' + response.sts);
        }
      })
    );
  }
  getVehiclesInformation(): Observable<GVAR> {
    return this.http.get<{ sts: number, gvar: any }>(`${this.baseUrl}/vehiclesinformations`).pipe(
      map(response => {
        if (response.sts == 1) {

          let gvar = new GVAR();
          gvar.DicOfDic = response.gvar.dicOfDic || {};
          gvar.DicOfDT = response.gvar.dicOfDT || {};
          return gvar;
        } else {
          throw new Error('Failed to load vehicles. Status code: ' + response.sts);
        }
      })
    );
  }
  getRouteHistories(id: number, start_epoch: any, end_epoch: any): Observable<GVAR> {
  
    return this.http.get<{ sts: number, gvar: any }>(`${this.baseUrl}/Routehistories/${id}/${start_epoch}/${end_epoch}`).pipe(
      map(response => {
        if (response.sts == 1) {

          let gvar = new GVAR();
          gvar.DicOfDic = response.gvar.dicOfDic || {};
          gvar.DicOfDT = response.gvar.dicOfDT || {};
          console.log("gvar==>", gvar.DicOfDT)
          return gvar;
        } else {
          throw new Error('Failed to load vehicles. Status code: ' + response.sts);
        }
      })
    );


  }


  getGeofences(): Observable<GVAR> {
    return this.http.get<{ sts: number, gvar: any }>(`${this.baseUrl}/Geofences`).pipe(
      map(response => {
        if (response.sts == 1) {
          let gvar = new GVAR();
          gvar.DicOfDic = response.gvar.dicOfDic || {};
          gvar.DicOfDT = response.gvar.dicOfDT || {};
          return gvar;
        } else {
          throw new Error('Failed to load geofences. Status code: ' + response.sts);
        }
      })
    );
  }

  addVehicleInformations(vehicleId: number, vehicleDetails: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/vehiclesinformations/`, vehicleDetails);
  }

  addVehicle(vehicle: any): Observable<any> {
    const gvar = new GVAR();

    return this.http.post(`${this.baseUrl}/vehicle`, vehicle);
  }
  addDriver(driver: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/drivers`, driver);
  }
  addRouteHistory( driver: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/Routehistories`, driver);
  }
  updateVehicle(vehicleId: number, vehicle: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/vehicle/${vehicleId}`, vehicle);
  }
  updateVehicleDetails(vehicleId: number, vehicleDetails: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/vehiclesinformations/${vehicleId}`, vehicleDetails);
  }
  updateDriver(driverId: number, driver: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/drivers/${driverId}`, driver);
  }



  deleteVehicle(vehicleId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/vehicle/${vehicleId}`);
  }
  deleteVehicleInformation(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/vehiclesinformations/${id}`);
  }
  deletDriver(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/drivers/${id}`);
  }



}
