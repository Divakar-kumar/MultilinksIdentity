import { Component, Input, OnInit } from '@angular/core';
import { DevicesService, DeviceDetail, GetDevicesResponse } from '../../services/devices.service';

@Component({
   selector: 'my-devices',
   templateUrl: './my-devices.component.html'
})

export class MyDevicesComponent {

   title = "My Devices";
   workInProgress: boolean;
   getDevicesResponse: GetDevicesResponse;

   /* The following are used for pagination navigation. */
   @Input() currentOffset: number;
   @Input() currentLimit: number;
   @Input() currentSize: number;

   @Input() devices: DeviceDetail[];

   constructor(private deviceService: DevicesService) {
   }

   ngOnInit() {
      this.workInProgress = true;
      this.deviceService.getDevices()
         .subscribe(data => this.getDevicesResponse = { ...data },
         err => console.error(err),
         () => {
            this.currentOffset = this.getDevicesResponse.offset;
            this.currentLimit = this.getDevicesResponse.limit;
            this.currentSize = this.getDevicesResponse.size;
            this.devices = this.getDevicesResponse.value;   /* value contains devices */
            this.workInProgress = false;
         });
   }
}
