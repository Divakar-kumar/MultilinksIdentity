import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import { ErrorObservable } from 'rxjs/observable/ErrorObservable';
import { catchError, retry } from 'rxjs/operators';

import { GetDevicesResponse } from '../models/get-device-response.model';

@Injectable()
export class DevicesService {

   constructor(private http: HttpClient) {
   }

   getDevices(limit: number, offset: number) {
      return this.getDevicesWithOptions(limit, offset);
   }

   private getDevicesWithOptions(limit: number, offset: number) {
      var resourceUrl = "https://localhost:44302/api/devices";

      if (limit != 0) {
         resourceUrl = `https://localhost:44302/api/devices?limit=${limit}&offset=${offset}`;
      }

      /* TODO: If current user is an admin, get all devices.
      Else get only devices created by this user.
      Get all devices for now. */
      return this.http.get<GetDevicesResponse>(resourceUrl)
         .pipe(
         //retry(3),   /* TODO: Retry doesn't work (is it due to self signed SSL?) */
         catchError(this.handleError)  /* Then handle the error */
         );
   }

   private handleError(error: HttpErrorResponse) {
      if (error.error instanceof ErrorEvent) {
         // A client-side or network error occurred. Handle it accordingly.
         console.error('An error occurred:', error.error.message);
      } else {
         // The backend returned an unsuccessful response code.
         // The response body may contain clues as to what went wrong,
         console.error(
            `Backend returned code ${error.status}, ` +
            `body was: ${error.error}`);
      }
      // return an ErrorObservable with a user-facing error message
      return new ErrorObservable(
         'Something bad happened; please try again later.');
   }
}

