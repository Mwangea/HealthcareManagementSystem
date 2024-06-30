import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { RoleGuard } from './role.guard';

describe('RoleGuard', () => {
  let guard: RoleGuard;
  let executeGuard: CanActivateFn;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(RoleGuard);
    executeGuard = (...guardParameters) =>
      guard.canActivate(...guardParameters);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
