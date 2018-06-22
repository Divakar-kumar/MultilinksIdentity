import { Injectable } from '@angular/core';

@Injectable()
export class AuthService {

   registerUser() {
      window.location.href = "https://localhost:44300/account/register";
   }

   loginUser() {
      alert("Redirect user to login page");
   }
}

