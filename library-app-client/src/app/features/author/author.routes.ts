import { Routes } from '@angular/router';
 
export const AUTHORS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./components/author-list/author-list.component').then(m => m.AuthorListComponent),
  },
];
 