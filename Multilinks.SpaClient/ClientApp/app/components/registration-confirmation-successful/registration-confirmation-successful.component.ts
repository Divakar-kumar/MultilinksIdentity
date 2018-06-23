import { Component, OnInit } from '@angular/core';

@Component({
   selector: 'registration-confirmation-successful',
   templateUrl: './registration-confirmation-successful.component.html'
})

export class RegistrationConfirmationSuccessfulComponent {

   ngOnInit() {
      alert("We should redirect the user to the login page. But we can do that once the login implementation is done.");
   }
}
