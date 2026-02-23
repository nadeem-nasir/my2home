import { Injectable } from '@angular/core';
import { Component } from '@angular/core';
import { ToasterConfig } from 'angular2-toaster';
import 'style-loader!angular2-toaster/toaster.css';
import { NbGlobalLogicalPosition, NbGlobalPhysicalPosition, NbGlobalPosition, NbToastrService } from '@nebular/theme';
import { NbToastStatus } from '@nebular/theme/components/toastr/model';

@Injectable({
  providedIn: 'root'
})
export class NotificationService
{
  config: ToasterConfig;
  index = 1;
  destroyByClick = true;
  duration = 2000;
  hasIcon = true;
  position: NbGlobalPosition = NbGlobalPhysicalPosition.TOP_RIGHT;
  preventDuplicates = false;
  status: NbToastStatus = NbToastStatus.SUCCESS;

  constructor(private toastr: NbToastrService)
  {

  }
  showSuccess(title: string, body: string)
  {
    const config ={
      status: NbToastStatus.SUCCESS,
      destroyByClick: this.destroyByClick,
      duration: this.duration,
      hasIcon: this.hasIcon,
      position: this.position,
      preventDuplicates: this.preventDuplicates,
    };
    const titleContent = title ? `. ${title}` : '';
    this.index += 1;
    this.toastr.show(
      body,
      `Toast ${this.index}${titleContent}`,
      config);
  }
  showDanger(title: string, body: string)
  {
    const config = {
      status: NbToastStatus.DANGER,
      destroyByClick: this.destroyByClick,
      duration: this.duration,
      hasIcon: this.hasIcon,
      position: this.position,
      preventDuplicates: this.preventDuplicates,
    };
    const titleContent = title ? `. ${title}` : '';
    this.index += 1;
    this.toastr.show(
      body,
      `Toast ${this.index}${titleContent}`,
      config);
  }
}
