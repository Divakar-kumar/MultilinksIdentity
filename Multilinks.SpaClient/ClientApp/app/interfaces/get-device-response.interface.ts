import { IDeviceDetail } from './device-detail.interface';

export interface IGetDevicesResponse {

   offset: number;
   limit: number;
   size: number;

   value: IDeviceDetail[];
}