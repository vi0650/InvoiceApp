import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';

const routes: Routes = [
  {
    path: 'login',
    loadChildren: () =>
      import('./login/login.module').then(m => m.LoginModule),
  },
  {
    path: 'layout',
    canActivate: [AuthGuard],
    loadChildren: () =>
      import('./layout/layout.module').then(m => m.LayoutModule)
  },
  { path: '**', redirectTo: '/login' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
