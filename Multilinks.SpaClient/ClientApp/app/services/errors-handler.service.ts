import { ErrorHandler, Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { ErrorMessage } from '../models/error-message.model';

@Injectable()
export class ErrorsHandler implements ErrorHandler {

   private errorMessage: ErrorMessage;

   constructor() {

      this.errorMessage = new ErrorMessage();
   }

   /* This will be called by an uncaught error */
   handleError(error: Error | HttpErrorResponse) {

      if (error instanceof HttpErrorResponse) {     
         if (!navigator.onLine) {
            this.errorMessage.errorType = "Unexpected Connection Error"
            this.errorMessage.errorCode = 0;
            console.log("No Internet connection failure");
            return;
         }

         this.errorMessage.errorType = "Unexpected HTTP Error";
         this.errorMessage.errorCode = error.status;
         console.log(`Unexpected HTTP failure response: ${error.status}`);
      }
      else {
         this.errorMessage.errorType = "Unexpected Client Error";
         this.errorMessage.errorCode = 0;
         console.log("Unexpected failure occurred");
      }

      this.logError(error);
   }

   /* This will be called when an error is caught */
   handleCaughtError(error: Error | HttpErrorResponse) {

      if (error instanceof HttpErrorResponse) {
         if (!navigator.onLine) {
            this.errorMessage.errorType = "No Connection"
            this.errorMessage.errorCode = 1;
            return this.errorMessage;
         }

         this.errorMessage.errorType = "HTTP Error";
         this.errorMessage.errorCode = error.status;
      }
      else {
         this.errorMessage.errorType = "Client Error";
         this.errorMessage.errorCode = 1;
      }

      this.logError(error);

      return this.errorMessage;
   }

   logError(error: Error | HttpErrorResponse) {

      console.log("We should log this error somewhere on the server.");
   }
}
