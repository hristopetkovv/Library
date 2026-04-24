import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { AuthService } from "../services/auth/auth.service";
import { UserRole } from "../enums/users/user-role.enum";

export const roleGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  
  const allowedRoles = route.data['role'] as Array<UserRole>;
  const user = authService.currentUser();

  if (user && allowedRoles.includes(user.role as UserRole)) {
    return true;
  }

  router.navigate(['/books']);

  return false;
};