import { Component, Input } from '@angular/core';
import { DeviceDetail } from '../../shared/device-detail.type';

@Component({
   selector: 'device-detail',
   templateUrl: './device-detail.component.html'
})

export class DeviceDetailComponent {

   @Input() device: DeviceDetail;
}
