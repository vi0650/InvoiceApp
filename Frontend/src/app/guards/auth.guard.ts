import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";
import { AuthService } from "../core/services/auth.service";


@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {

  constructor(private router: Router, private auth: AuthService) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {

    if (!this.auth.isLoggedIn()) {
      this.router.createUrlTree(['/login']);
      return false;
    }
    const expectedRole = route.data['role'];

    const user = this.auth.getUserData();

    if (expectedRole && expectedRole !== user?.role) {
      this.router.navigate(['/login']);
      return false;
    }
    return true;
  }
}