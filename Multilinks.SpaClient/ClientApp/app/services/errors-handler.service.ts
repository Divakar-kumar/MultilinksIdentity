import { ErrorHandler, Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable()
export class ErrorsHandler implements ErrorHandler {

   handleError(error: Error | HttpErrorResponse) {

      if (error instanceof HttpErrorResponse) {     
         if (!navigator.onLine) {
            console.log("No Internet connection failure");
         }

         console.log(`Unexpected HTTP failure response: ${error.status}`);
      }
      else {
         console.log("Unexpected failure occurred");
      }
      
      console.log("We should log this somewhere if possible");
   }
}