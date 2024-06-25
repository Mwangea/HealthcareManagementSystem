import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanActivateFn, Router, RouterStateSnapshot } from '@angular/router';
import { AuthService } from './auth.service';


@Injectable({
  providedIn: 'root'
})
export class authGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const roles = route.data['roles'] as Array<string>;
    if (!roles || roles.length === 0 || roles.includes(this.authService.getCurrentUser()?.role)) {
      return true;
    } else {
      this.router.navigate(['/login']);
      return false;
    }
  }
}
