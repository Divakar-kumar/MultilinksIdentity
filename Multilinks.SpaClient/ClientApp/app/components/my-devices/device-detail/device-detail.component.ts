import { Component, Input } from '@angular/core';

import { DeviceDetail } from '../../../models/device-detail.model';

@Component({
   selector: 'device-detail',
   templateUrl: './device-detail.component.html'
})

export class DeviceDetailComponent {

   @Input()
   device: DeviceDetail = new DeviceDetail;
}
