import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'books', pathMatch: 'full' },
  { path: 'books', loadChildren: () => import('./features/book/book.routes').then(m => m.BOOKS_ROUTES) },
  { path: 'authors', loadChildren: () => import('./features/author/author.routes').then(m => m.AUTHORS_ROUTES) }
];
