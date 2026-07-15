import { Routes } from '@angular/router';
import { roleGuard } from '../../core/guards/role-guard';
import { UserRole } from '../../core/enums/users/user-role.enum';

export const ADMIN_ROUTES: Routes = [
  { path: 'authors', canActivate: [roleGuard([UserRole.admin])], loadComponent: () => import('./components/author/author-admin-list/author-admin-list.component').then(m => m.AuthorAdminListComponent) },
  { path: 'books', canActivate: [roleGuard([UserRole.admin])], loadComponent: () => import('./components/book/book-admin-list/book-admin-list.component').then(m => m.BookAdminListComponent) },
  { path: 'publishers', canActivate: [roleGuard([UserRole.admin])], loadComponent: () => import('./components/publisher/publisher-admin-list/publisher-admin-list.component').then(m => m.PublisherAdminListComponent) },
  { path: 'users', canActivate: [roleGuard([UserRole.admin])], loadComponent: () => import('./components/user/user-admin-list/user-admin-list.component').then(m => m.UserAdminListComponent) },
];