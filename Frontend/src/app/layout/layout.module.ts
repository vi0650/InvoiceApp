import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutRoutingModule } from './layout-routing.module';
import { LayoutComponent } from './layout.component';
import { NgUIModule } from '../shared/ng-ui.module';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatRecycleRows } from "@angular/material/table";

@NgModule({
  declarations: [
    LayoutComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    LayoutRoutingModule,
    MatRecycleRows,
    NgUIModule
],
exports:[]
})
export class LayoutModule { }
