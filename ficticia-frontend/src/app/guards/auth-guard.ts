import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private router: Router, private authService: AuthService) {}

  canActivate(): boolean {
    const token = this.authService.obtenerToken();
    console.log('Token actual:', token);
    console.log('¿Está logueado?', this.authService.estaLogueado());

    if (this.authService.estaLogueado()) {
      return true;
    }

    this.router.navigate(['/login']);
    return false;
  }
}
