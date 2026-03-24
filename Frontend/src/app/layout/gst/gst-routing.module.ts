import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GstComponent } from './gst.component';
import { GstFormComponent } from './gst-form/gst-form.component';

const routes: Routes = [
  { path: '', component: GstComponent },
  { path: 'gst-form', component: GstFormComponent },
  { path: 'gst-form/:id', component: GstFormComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GstRoutingModule { }
