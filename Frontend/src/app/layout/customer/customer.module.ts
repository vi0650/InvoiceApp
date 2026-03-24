import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CustomerRoutingModule } from './customer-routing.module';
import { CustomerComponent } from './customer.component';
import { CustomerFormComponent } from './customer-form/customer-form.component';
import { NgUIModule } from '../../shared/ng-ui.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    CustomerComponent,
    CustomerFormComponent
  ],
  imports: [
    CommonModule,
    CustomerRoutingModule,
    NgUIModule,
    FormsModule,
    ReactiveFormsModule
  ]
})
export class CustomerModule { }
