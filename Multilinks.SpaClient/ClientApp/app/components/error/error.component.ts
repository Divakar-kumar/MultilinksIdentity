import { Component, OnInit } from '@angular/core';
import { ErrorMessage } from '../../models/error-message.model';
import { ActivatedRoute } from '@angular/router';

@Component({
   selector: 'error',
   templateUrl: './error.component.html'
})

export class ErrorComponent {

   errorMessage: ErrorMessage = { errorType: "", errorCode: ""};

   constructor(private routes: ActivatedRoute) {
      /* TODO: There appeared to be something weird in the way the error page is rendered.
       * It will first display Client Error 3 before quickly changed to it's correct error message.  */
   }

   ngOnInit() {

      this.routes.params.subscribe(params => {
         if (params["type"] == null || params["type"] == undefined) {
            this.errorMessage.errorType = "Error";
            this.errorMessage.errorCode = "Oops, something unexpected went wrong.";
         }
         else {
            this.errorMessage.errorType = params["type"];
            this.errorMessage.errorCode = params["code"];
         }
      });
   }
}
