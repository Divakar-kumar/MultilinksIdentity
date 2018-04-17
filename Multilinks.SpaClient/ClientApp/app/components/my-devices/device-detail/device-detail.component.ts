import { Component, Input } from '@angular/core';

import { IDeviceDetail } from '../../../interfaces/device-detail.interface';

@Component({
   selector: 'device-detail',
   templateUrl: './device-detail.component.html'
})

export class DeviceDetailComponent {

   @Input() device: IDeviceDetail;
}
