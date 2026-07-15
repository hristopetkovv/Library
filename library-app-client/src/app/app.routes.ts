import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'books', pathMatch: 'full' },
  { path: 'books', loadChildren: () => import('./features/book/book.routes').then(m => m.BOOKS_ROUTES) },
  { path: 'authors', loadChildren: () => import('./features/author/author.routes').then(m => m.AUTHORS_ROUTES) },
  { path: 'publishers', loadChildren: () => import('./features/publisher/publisher.routes').then(m => m.PUBLISHERS_ROUTES) },
  { path: 'admin', loadChildren: () => import('./features/admin/admin.routes').then(m => m.ADMIN_ROUTES) },
  { path: 'profile', loadComponent: () => import('./features/user/components/profile/profile.component').then(m => m.ProfileComponent) }
];
