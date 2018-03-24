import { Component, Input } from '@angular/core';
import { DeviceDetail } from '../../../services/devices.service';

@Component({
   selector: 'device-detail',
   templateUrl: './device-detail.component.html'
})

export class DeviceDetailComponent {

   @Input() device: DeviceDetail;
}
