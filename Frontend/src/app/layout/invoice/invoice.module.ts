import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { NgUIModule } from '../../shared/ng-ui.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatRecycleRows } from '@angular/material/table';
import { InvoiceComponent } from './invoice.component';
import { InvoiceRoutingModule } from './invoice-routing.module';
import { InvoiceFormComponent } from './invoice-form/invoice-form.component';
import { ShowInvoiceComponent } from './show-invoice/show-invoice.component';

@NgModule({
  declarations: [
    InvoiceComponent,
    InvoiceFormComponent,
    ShowInvoiceComponent
  ],
  imports: [
    CommonModule,
    InvoiceRoutingModule,
    NgUIModule,
    FormsModule,
    ReactiveFormsModule,
    MatRecycleRows
  ]
})
export class InvoiceModule { }
