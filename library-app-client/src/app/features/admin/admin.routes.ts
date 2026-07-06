import { Routes } from '@angular/router';
import { roleGuard } from '../../core/guards/role-guard';
import { UserRole } from '../../core/enums/users/user-role.enum';

export const ADMIN_ROUTES: Routes = [
  { path: 'books', canActivate: [roleGuard([UserRole.admin])], loadComponent: () => import('../book/components/admin/book-admin-list/book-admin-list.component').then(m => m.BookAdminListComponent) }
];