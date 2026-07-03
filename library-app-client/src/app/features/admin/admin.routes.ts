import { Routes } from '@angular/router';

export const ADMIN_ROUTES: Routes = [
  { path: 'books', loadComponent: () => import('../book/components/admin/book-admin-list/book-admin-list.component').then(m => m.BookAdminListComponent) }
];