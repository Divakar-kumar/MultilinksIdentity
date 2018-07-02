import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import { ErrorObservable } from 'rxjs/observable/ErrorObservable';
import { catchError, retry } from 'rxjs/operators';

import { GetDevicesResponse } from '../models/get-device-response.model';
import { ErrorsHandler } from './errors-handler.service';
import { ErrorMessage } from '../models/error-message.model';
import 'rxjs/add/operator/catch';
import 'rxjs/add/Observable/throw';

@Injectable()
export class DevicesService {

   constructor(private http: HttpClient, private errorsHandler: ErrorsHandler) {
   }

    getDevices(limit: number, offset: number): Observable<GetDevicesResponse> {
        var resourceUrl = "https://localhost:44302/api/devices";

        if (limit != 0) {
            resourceUrl = `https://localhost:44302/api/devices?limit=${limit}&offset=${offset}`;
        }

        /* TODO: If current user is an admin, get all devices.
        Else get only devices created by this user.
        Get all devices for now. */
        return this.http.get< GetDevicesResponse >(resourceUrl)
            .pipe(
               //retry(3),   /* TODO: Retry doesn't work (is it due to self signed SSL?) */
               catchError (this.handleError)  /* Then handle the error */
            );
         
   }

    private handleError(error: Error | HttpErrorResponse){
        return new ErrorObservable(this.errorsHandler.handleCaughtError(error));
    }
}

