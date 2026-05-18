import { Routes } from '@angular/router';
 
export const PUBLISHERS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./components/publisher-list/publisher-list.component').then(m => m.PublisherListComponent),
  },
];