import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class DevicesService {

   constructor(private http: HttpClient) {
   }

   getDevices() {
      /* TODO: If current user is an admin, get all devices.
               Else get only devices created by this user.
               Get all devices for now. */
      return this.http.get('https://localhost:44302/api/devices/');
   }
}
