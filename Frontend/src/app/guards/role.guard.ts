import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, UrlTree } from "@angular/router";
import { AuthService } from "../core/services/auth.service";
import { Role } from "../core/models/user.model";


@Injectable({ providedIn: 'root' })
export class RoleGuard implements CanActivate {

  constructor(private router: Router, private auth: AuthService) { }

  canActivate(route: ActivatedRouteSnapshot): boolean | UrlTree {
    const allowedRoles: Role[] = route.data['roles'];
    const user = this.auth.getUserData();

    if (!user || !allowedRoles?.includes(user.role)) {
      return true;
    }

    return false;
  }
}
