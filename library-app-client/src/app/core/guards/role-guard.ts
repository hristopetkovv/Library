import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { AuthService } from "../../features/auth/services/auth.service";
import { UserRole } from "../enums/users/user-role.enum";

export const roleGuard = (roles: UserRole[]): CanActivateFn => {
  return () => {
    const authService = inject(AuthService);
    const router = inject(Router);

    if (roles.some(role => authService.hasRole(role))) {
      return true;
    }

    router.navigate(['/']);
    return false;
  };
};