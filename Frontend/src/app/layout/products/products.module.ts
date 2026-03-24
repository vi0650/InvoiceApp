import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductsRoutingModule } from './products-routing.module';
import { NgUIModule } from '../../shared/ng-ui.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ProductsComponent } from './products.component';
import { ProductFormComponent } from './product-form/product-form.component';


@NgModule({
  declarations: [
    ProductsComponent,
    ProductFormComponent
  ],
  imports: [
    CommonModule,
    ProductsRoutingModule,
    NgUIModule,
    FormsModule,
    ReactiveFormsModule
  ]
})
export class ProductsModule { }
