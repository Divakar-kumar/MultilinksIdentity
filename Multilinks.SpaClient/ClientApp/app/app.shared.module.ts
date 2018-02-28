import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { RecentActivitiesComponent } from './components/recent-activities/recent-activities.component';
import { HeaderComponent } from './components/shared/header/header.component';
import { FooterComponent } from './components/shared/footer/footer.component';

@NgModule({
   declarations: [
      AppComponent,
      RecentActivitiesComponent,
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
         { path: '**', redirectTo: 'recent-activities' }
      ])
   ]
})
export class AppModuleShared {
}
