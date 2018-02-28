import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { RecentActivitiesComponent } from './components/recent-activities/recent-activities.component';
import { MyDevicesComponent } from './components/my-devices/my-devices.component';
import { MyLinksComponent } from './components/my-links/my-links.component';
import { MyServicesComponent } from './components/my-services/my-services.component';
import { HeaderComponent } from './components/shared/header/header.component';
import { FooterComponent } from './components/shared/footer/footer.component';

@NgModule({
   declarations: [
      AppComponent,
      RecentActivitiesComponent,
      MyDevicesComponent,
      MyLinksComponent,
      MyServicesComponent,
      HeaderComponent,
      FooterComponent
   ],
   imports: [
      CommonModule,
      HttpModule,
      FormsModule,
      RouterModule.forRoot([
         { path: '', redirectTo: 'recent-activities', pathMatch: 'full' },
         { path: 'recent-activities', component: RecentActivitiesComponent },
         { path: 'my-devices', component: MyDevicesComponent },
         { path: 'my-links', component: MyLinksComponent },
         { path: 'my-services', component: MyServicesComponent },
         { path: '**', redirectTo: 'recent-activities' }
      ])
   ]
})
export class AppModuleShared {
}
