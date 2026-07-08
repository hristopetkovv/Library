import { Routes } from '@angular/router';
import { roleGuard } from '../../core/guards/role-guard';
import { UserRole } from '../../core/enums/users/user-role.enum';

export const ADMIN_ROUTES: Routes = [
  { path: 'books', canActivate: [roleGuard([UserRole.admin])], loadComponent: () => import('../book/components/admin/book-admin-list/book-admin-list.component').then(m => m.BookAdminListComponent) },
  { path: 'publishers', canActivate: [roleGuard([UserRole.admin])], loadComponent: () => import('../publisher/components/admin/publisher-admin-list/publisher-admin-list.component').then(m => m.PublisherAdminListComponent) },
  { path: 'authors', canActivate: [roleGuard([UserRole.admin])], loadComponent: () => import('../author/components/admin/author-admin-list/author-admin-list.component').then(m => m.AuthorAdminListComponent) }
];