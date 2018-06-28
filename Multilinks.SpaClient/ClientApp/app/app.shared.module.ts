import { NgModule, ErrorHandler } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { RegistrationConfirmationSuccessfulComponent } from './components/registration-confirmation-successful/registration-confirmation-successful.component';
import { HomeComponent } from './components/home/home.component';
import { MyDevicesComponent } from './components/my-devices/my-devices.component';
import { HeaderComponent } from './components/shared/header/header.component';
import { FooterComponent } from './components/shared/footer/footer.component';
import { DeviceDetailComponent } from './components/my-devices/device-detail/device-detail.component';
import { LoadingComponent } from './components/shared/loading/loading.component';

import { DevicesService } from './services/devices.service';
import { ErrorsHandler } from './services/errors-handler.service';
import { ErrorComponent } from './components/error/error.component';
import { AuthService } from './services/auth.service';

@NgModule({
   declarations: [
      AppComponent,
      RegistrationConfirmationSuccessfulComponent,
      HomeComponent,
      MyDevicesComponent,
      ErrorComponent,
      HeaderComponent,
      FooterComponent,
      DeviceDetailComponent,
      LoadingComponent
   ],
   imports: [
      CommonModule,
      HttpClientModule,
      FormsModule,
      RouterModule.forRoot([
         { path: '', redirectTo: 'home', pathMatch: 'full' },
         { path: 'registration-confirmation-successful', component: RegistrationConfirmationSuccessfulComponent },
         { path: 'home', component: HomeComponent },
         { path: 'my-devices', component: MyDevicesComponent },
         { path: 'error', component: ErrorComponent },
         { path: '**', redirectTo: 'error' }
      ])
   ],
   providers: [
      { provide: ErrorHandler, useClass: ErrorsHandler },
      DevicesService,
      AuthService,
      ErrorsHandler
   ]
})
export class AppModuleShared {
}
