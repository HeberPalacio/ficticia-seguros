import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  error: string = '';

  constructor(private authService: AuthService, private router: Router) {}

  login() {
    this.error = '';

    if (!this.username || !this.password) {
      this.error = 'Debes ingresar usuario y contraseña';
      return;
    }

    this.authService.login(this.username, this.password).subscribe({
      next: () => this.router.navigate(['/personas']),
      error: (err) => {
        console.error(err);
        this.error = err.error?.message || 'Usuario o contraseña incorrecta';
      },
    });
  }
}
