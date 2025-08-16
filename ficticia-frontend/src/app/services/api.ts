// src/app/services/api.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';

export interface LoginResponse {
  token: string;
  expiracion?: string;
}

export interface Persona {
  id: number;
  nombreCompleto: string;
  identificacion: string;
  edad: number | null;
  genero: string;
  estadoActivo: boolean;
  maneja: boolean;
  usaLentes: boolean;
  diabetico: boolean;
  enfermedadOtra?: string;
}

@Injectable({
  providedIn: 'root',
})
export class Api {
  private baseUrl = 'http://localhost:5209/api';

  constructor(private http: HttpClient) {}

  // --------------------------
  // AUTH
  // --------------------------
  login(usuario: string, password: string): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(`${this.baseUrl}/Auth/login`, { usuario, password })
      .pipe(
        tap((res) => {
          if (res?.token) {
            localStorage.setItem('token', res.token);
          }
        }),
        catchError(this.handleError)
      );
  }

  logout(): void {
    localStorage.removeItem('token');
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  // --------------------------
  // JWT UTILITIES
  // --------------------------
  private decodeToken(token: string | null): any | null {
    if (!token) return null;
    try {
      const payload = token.split('.')[1];
      const json = atob(payload.replace(/-/g, '+').replace(/_/g, '/'));
      return JSON.parse(json);
    } catch {
      return null;
    }
  }

  getUsername(): string | null {
    const dec = this.decodeToken(this.getToken());
    return dec?.sub ?? dec?.unique_name ?? null;
  }

  getUserRoles(): string[] {
    const dec = this.decodeToken(this.getToken());
    if (!dec) return [];

    const roles: string[] = [];
    if (dec.role)
      roles.push(...(Array.isArray(dec.role) ? dec.role : [dec.role]));
    if (dec.roles)
      roles.push(...(Array.isArray(dec.roles) ? dec.roles : [dec.roles]));

    const roleClaim =
      'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
    if (dec[roleClaim])
      roles.push(
        ...(Array.isArray(dec[roleClaim]) ? dec[roleClaim] : [dec[roleClaim]])
      );

    return Array.from(new Set(roles));
  }

  isUserInRole(role: string): boolean {
    return this.getUserRoles().includes(role);
  }

  private getTokenExpirationDate(token: string | null): Date | null {
    const dec = this.decodeToken(token);
    if (!dec || !dec.exp) return null;
    const date = new Date(0);
    date.setUTCSeconds(dec.exp);
    return date;
  }

  isTokenExpired(token: string | null = null, offsetSeconds = 0): boolean {
    const t = token ?? this.getToken();
    if (!t) return true;
    const date = this.getTokenExpirationDate(t);
    if (!date) return false;
    return date.valueOf() <= new Date().valueOf() + offsetSeconds * 1000;
  }

  isLoggedIn(): boolean {
    return !this.isTokenExpired();
  }

  // --------------------------
  // PERSONAS (CRUD)
  // --------------------------
  // El interceptor se encargará de enviar el token en todas las requests
  getPersonas(): Observable<Persona[]> {
    return this.http
      .get<Persona[]>(`${this.baseUrl}/Personas`)
      .pipe(catchError(this.handleError));
  }

  getPersona(id: number): Observable<Persona> {
    return this.http
      .get<Persona>(`${this.baseUrl}/Personas/${id}`)
      .pipe(catchError(this.handleError));
  }

  crearPersona(persona: Partial<Persona>): Observable<Persona> {
    return this.http
      .post<Persona>(`${this.baseUrl}/Personas`, persona)
      .pipe(catchError(this.handleError));
  }

  actualizarPersona(id: number, persona: Partial<Persona>): Observable<any> {
    return this.http
      .put(`${this.baseUrl}/Personas/${id}`, persona)
      .pipe(catchError(this.handleError));
  }

  eliminarPersona(id: number): Observable<any> {
    return this.http
      .delete(`${this.baseUrl}/Personas/${id}`)
      .pipe(catchError(this.handleError));
  }

  // --------------------------
  // ERROR HANDLING
  // --------------------------
  private handleError = (error: HttpErrorResponse) => {
    let mensaje = 'Ocurrió un error';
    if (error.error instanceof ErrorEvent) {
      mensaje = error.error.message;
    } else {
      if (error.status === 0) mensaje = 'No se pudo conectar con el servidor';
      else if (error.status === 401)
        mensaje = 'No autorizado. Por favor inicie sesión.';
      else if (error.status === 403)
        mensaje = 'Acceso prohibido. No tiene permisos.';
      else if (error.error?.message) mensaje = error.error.message;
      else if (typeof error.error === 'string' && error.error.length)
        mensaje = error.error;
      else mensaje = `Error ${error.status}: ${error.message}`;
    }
    return throwError(() => ({ status: error.status, message: mensaje }));
  };
}
