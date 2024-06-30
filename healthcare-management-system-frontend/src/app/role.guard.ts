import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { UserService } from '../_service/user.service';

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {

  constructor(private userService: UserService, private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const expectedRole = route.data['expectedRole'];
    const token = localStorage.getItem('token');

    if (token) {
      const decodedToken = this.userService.decodeToken(token);

      if (decodedToken && decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] === expectedRole) {
        return true;
      }
    }

    this.router.navigate(['/login']);
    return false;
  }

}
