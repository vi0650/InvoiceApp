import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from './layout.component';
import { AuthGuard } from '../guards/auth.guard';
import { Role } from '../core/models/user.model';
import { RoleGuard } from '../guards/role.guard';

const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: 'users',
        canActivate: [RoleGuard],
        data: { role: Role.SuperAdmin },
        loadChildren: () =>
          import('./users/users.module').then(m => m.UsersModule)
      },
      {
        path: 'customers',
        canActivate: [RoleGuard],
        data: { role: [Role.User, Role.SuperAdmin] },
        loadChildren: () =>
          import('./customer/customer.module').then(m => m.CustomerModule)
      },
      {
        path: 'products',
        canActivate: [RoleGuard],
        data: { role: [Role.User, Role.SuperAdmin] },
        loadChildren: () =>
          import('./products/products.module').then(m => m.ProductsModule)
      },
      {
        path: 'invoices',
        canActivate: [RoleGuard],
        data: { role: [Role.User, Role.SuperAdmin, Role.Employee] },
        loadChildren: () =>
          import('./invoice/invoice.module').then(m => m.InvoiceModule)
      },
      {
        path: 'dashboard',
        canActivate: [RoleGuard],
        data: { role: [Role.User, Role.SuperAdmin, Role.Employee] },
        loadChildren: () =>
          import('./dashboard/dashboard.module').then(m => m.DashboardModule)
      },
      {
        path: 'gst',
        canActivate: [RoleGuard],
        data: { role: [Role.SuperAdmin] },
        loadChildren: () =>
          import('./gst/gst.module').then(m => m.GstModule)
      },
      {
        path: 'settings',
        canActivate: [RoleGuard],
        data: { role: [Role.SuperAdmin] },
        loadChildren: () =>
          import('./settings/settings.module').then(m => m.SettingsModule)
      },
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      }
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LayoutRoutingModule { }
