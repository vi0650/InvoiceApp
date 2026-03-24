import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CustomerComponent } from './customer.component';
import { CustomerFormComponent } from './customer-form/customer-form.component';

const routes: Routes = [
  { path: '', component: CustomerComponent },
  { path: 'customer-form', component: CustomerFormComponent },
  { path: 'customer-form/:id', component: CustomerFormComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CustomerRoutingModule { }
