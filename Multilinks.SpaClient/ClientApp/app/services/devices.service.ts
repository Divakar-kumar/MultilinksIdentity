import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

export interface DeviceDetail {

   /* The properties in this class should match those of Endpoint object in
    * the backend (i.e. device == endpoint).
    */
   endpointId: string;    /* String representation of GUID. */
   name: string;
   description: string;
}

export interface GetDevicesResponse {

   offset: number;
   limit: number;
   size: number;

   value: DeviceDetail[];
}

@Injectable()
export class DevicesService {

   constructor(private http: HttpClient) {
   }

   getDevices() {
      /* TODO: If current user is an admin, get all devices.
               Else get only devices created by this user.
               Get all devices for now. */
      return this.http.get<GetDevicesResponse>('https://localhost:44302/api/devices/');
   }
}

