import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GstRoutingModule } from './gst-routing.module';
import { GstComponent } from './gst.component';
import { GstFormComponent } from './gst-form/gst-form.component';
import { NgUIModule } from '../../shared/ng-ui.module';
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    GstComponent,
    GstFormComponent
  ],
  imports: [
    CommonModule,
    GstRoutingModule,
    NgUIModule,
    ReactiveFormsModule
  ]
})
export class GstModule { }
