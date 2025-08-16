import { TestBed } from '@angular/core/testing';
import { HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { of } from 'rxjs';

import { AuthInterceptor } from './auth-interceptor';

describe('AuthInterceptor', () => {
  let interceptor: AuthInterceptor;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    interceptor = new AuthInterceptor();
  });

  it('should be created', () => {
    expect(interceptor).toBeTruthy();
  });

  it('should add Authorization header if token exists', (done) => {
    // Guardar un token de prueba en localStorage
    const testToken = 'test-token';
    localStorage.setItem('token', testToken);

    // Crear una petición de prueba
    const request = new HttpRequest('GET', '/test');
    const handler: HttpHandler = {
      handle: (req: HttpRequest<any>) => {
        expect(req.headers.get('Authorization')).toBe(`Bearer ${testToken}`);
        return of({} as HttpEvent<any>);
      },
    };

    interceptor.intercept(request, handler).subscribe(() => {
      // Limpiar token después de la prueba
      localStorage.removeItem('token');
      done();
    });
  });

  it('should not modify request if no token', (done) => {
    localStorage.removeItem('token');

    const request = new HttpRequest('GET', '/test');
    const handler: HttpHandler = {
      handle: (req: HttpRequest<any>) => {
        expect(req.headers.has('Authorization')).toBeFalse();
        return of({} as HttpEvent<any>);
      },
    };

    interceptor.intercept(request, handler).subscribe(() => done());
  });
});
