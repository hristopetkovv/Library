import { Routes } from "@angular/router";

export const BOOKS_ROUTES: Routes = [
    {
        path: '',
        loadComponent: () =>
        import('./components/book-list/book-list.component').then(m => m.BookListComponent)
    }
]