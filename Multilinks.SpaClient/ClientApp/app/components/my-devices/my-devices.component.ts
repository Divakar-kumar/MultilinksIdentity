import { Component, OnInit } from '@angular/core';
import { DeviceDetail } from '../../types/device-detail.type';

@Component({
   selector: 'my-devices',
   templateUrl: './my-devices.component.html'
})

export class MyDevicesComponent {

   title = "My Devices";
   devices: DeviceDetail[];

   constructor() {
   }

   ngOnInit() {
      this.devices = [
         {
            endpointId: "4b483ff1-b280-4752-8cce-c00f66e527cb",
            creatorId: "144345f4-ab8c-46ee-955f-6551802ab665",
            name: "Arduino TV Remote",
            description: "Receive command from the gateway and action the command on the TV"
         },
         {
            endpointId: "4707f12b-b399-4680-9684-0b7b15b2faa9",
            creatorId: "144345f4-ab8c-46ee-955f-6551802ab665",
            name: "Arduino TV Remote Gateway",
            description: "Manage communications between Arduino TV Remote and other endpoints"
         },
         {
            endpointId: "4b483ff1-b280-4752-8cce-c00f66e527cb",
            creatorId: "144345f4-ab8c-46ee-955f-6551802ab665",
            name: "HTC One",
            description: "Multilinks client running on Android mobile device"
         },
         {
            endpointId: "4b483ff1-b280-4752-8cce-c00f66e527cb",
            creatorId: "144345f4-ab8c-46ee-955f-6551802ab665",
            name: "IPhone 7 Plus",
            description: "Multilinks client running on iOS mobile device"
         }
      ];
   }
}
