import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { AuthGuard } from './auth-guard';
import { Api } from '../services/api';

describe('AuthGuard', () => {
  let guard: AuthGuard;
  let routerSpy: jasmine.SpyObj<Router>;
  let apiSpy: jasmine.SpyObj<Api>;

  beforeEach(() => {
    const router = jasmine.createSpyObj('Router', ['navigate']);
    const api = jasmine.createSpyObj('Api', ['isLoggedIn']);

    TestBed.configureTestingModule({
      providers: [
        AuthGuard,
        { provide: Router, useValue: router },
        { provide: Api, useValue: api },
      ],
    });

    guard = TestBed.inject(AuthGuard);
    routerSpy = TestBed.inject(Router) as jasmine.SpyObj<Router>;
    apiSpy = TestBed.inject(Api) as jasmine.SpyObj<Api>;
  });

  it('debería permitir acceso si está logueado', () => {
    apiSpy.isLoggedIn.and.returnValue(true);
    expect(guard.canActivate()).toBeTrue();
    expect(routerSpy.navigate).not.toHaveBeenCalled();
  });

  it('debería redirigir al login si no está logueado', () => {
    apiSpy.isLoggedIn.and.returnValue(false);
    expect(guard.canActivate()).toBeFalse();
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/login']);
  });
});
