import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InvoiceComponent } from './invoice.component';
import { InvoiceFormComponent } from './invoice-form/invoice-form.component';
import { ShowInvoiceComponent } from './show-invoice/show-invoice.component';

const routes: Routes = [
  { path: '', component: InvoiceComponent },
  { path: 'invoices-form', component: InvoiceFormComponent },
  { path: 'invoices-form/:id', component: InvoiceFormComponent },
  { path: 'invoices-show/:id', component: ShowInvoiceComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InvoiceRoutingModule { }
