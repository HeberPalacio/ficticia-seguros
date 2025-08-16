import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { PersonasComponent } from './components/personas/personas.component';
import { AuthGuard } from './guards/auth-guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'personas', component: PersonasComponent, canActivate: [AuthGuard] },
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: '**', redirectTo: 'login', pathMatch: 'full' },
];
