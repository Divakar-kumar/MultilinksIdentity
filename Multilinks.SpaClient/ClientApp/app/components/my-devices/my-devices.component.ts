import { Component, Input, OnInit } from '@angular/core';
import { DevicesService, DeviceDetail, GetDevicesResponse } from '../../services/devices.service';

import * as _ from 'underscore';

@Component({
   selector: 'my-devices',
   templateUrl: './my-devices.component.html'
})

export class MyDevicesComponent
{
   title = "My Devices";
   workInProgress: boolean;
   getDevicesResponse: GetDevicesResponse;

   /* The following are used for pagination navigation. */
   paginationProperties: PaginationProperties;

   @Input() devices: DeviceDetail[];

   constructor(private deviceService: DevicesService)
   {
      this.paginationProperties = new PaginationProperties();
   }

   ngOnInit()
   {
      this.workInProgress = true;
      this.deviceService.getDevices()
         .subscribe(data => this.getDevicesResponse = { ...data },
         err => console.error(err),
         () => {
            this.updatePaginationDetails(this.getDevicesResponse.offset, this.getDevicesResponse.limit, this.getDevicesResponse.size);
            this.devices = this.getDevicesResponse.value;   /* Value contains devices. */
            this.workInProgress = false;
         });
   }

   updatePaginationDetails(offset: number, limit: number, size: number)
   {
      this.paginationProperties.pageSize = limit;
      this.paginationProperties.totalItems = size;
      this.paginationProperties.totalPages = Math.ceil(size / limit);
      this.paginationProperties.currentPage = (offset / limit) + 1;

      if (this.paginationProperties.totalPages <= 10) {
         /* Less than 10 total pages so show all. */
         this.paginationProperties.startPage = 1;
         this.paginationProperties.endPage = this.paginationProperties.totalPages;
      }
      else {
         /* More than 10 total pages so calculate start and end pages. */
         if (this.paginationProperties.currentPage <= 6) {
            this.paginationProperties.startPage = 1;
            this.paginationProperties.endPage = 10;
         } else if (this.paginationProperties.currentPage + 4 >= this.paginationProperties.totalPages) {
            this.paginationProperties.startPage = this.paginationProperties.totalPages - 9;
            this.paginationProperties.endPage = this.paginationProperties.totalPages;
         } else {
            this.paginationProperties.startPage = this.paginationProperties.currentPage - 5;
            this.paginationProperties.endPage = this.paginationProperties.currentPage + 4;
         }
      }

      /* Calculate start and end item indexes. */
      this.paginationProperties.startIndex = (this.paginationProperties.currentPage - 1) * this.paginationProperties.pageSize;
      this.paginationProperties.endIndex = Math.min(this.paginationProperties.startIndex + this.paginationProperties.pageSize - 1, this.paginationProperties.totalItems - 1);

      /* Create an array of pages to ng-repeat in the pager control. */
      this.paginationProperties.pages = _.range(this.paginationProperties.startPage, this.paginationProperties.endPage + 1);
   }

   setPage(page: number)
   {
      if (this.paginationProperties.currentPage == page) return;

      if (page < 1) return;

      if (page > this.paginationProperties.totalPages) return;

      this.workInProgress = true;

      this.deviceService.getSpecificPageOfDevices(this.paginationProperties.pageSize, this.getFirstItemIndexOfPage(page))
         .subscribe(data => this.getDevicesResponse = { ...data },
         err => console.error(err),
         () => {
            this.updatePaginationDetails(this.getDevicesResponse.offset, this.getDevicesResponse.limit, this.getDevicesResponse.size);
            this.devices = this.getDevicesResponse.value;   /* value contains devices */
            this.workInProgress = false;
         });
   }

   getFirstItemIndexOfPage(page: number)
   {
      /* Return the 0-indexed first item of current page. */
      return (page * this.paginationProperties.pageSize) - this.paginationProperties.pageSize;
   }
}

export class PaginationProperties
{
   totalItems: number;
   currentPage: number;
   pageSize: number;
   totalPages: number;
   startPage: number;
   endPage: number;
   startIndex: number;
   endIndex: number;
   pages: number[]
}
