import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UsersRoutingModule } from './users-routing.module';
import { NgUIModule } from '../../shared/ng-ui.module';
import { UserFormComponent } from './user-form/user-form.component';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { UsersComponent } from './users.component';


@NgModule({
  declarations: [
    UserFormComponent,
    UsersComponent
  ],
  imports: [
    CommonModule,
    UsersRoutingModule,
    NgUIModule,
    FormsModule,
    ReactiveFormsModule,
  ]
})
export class UsersModule { }
