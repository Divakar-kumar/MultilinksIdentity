import { DeviceDetail } from './device-detail.model';

export class GetDevicesResponse {

   offset: number;
   limit: number;
   size: number;

   value: DeviceDetail[];
}