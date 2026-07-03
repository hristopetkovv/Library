import { Component, OnInit, inject, signal } from '@angular/core';
import { TranslatePipe } from '@ngx-translate/core';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { finalize } from 'rxjs';
import { LoadingComponent } from '../../../../../shared/components/loading/loading.component';
import { BookResource } from '../../../resources/book.resource';
import { BookListDto } from '../../../dtos/book-list.dto';
import { BookFormComponent } from '../book-form/book-form.component';
 
@Component({
  selector: 'app-book-admin-list',
  standalone: true,
  imports: [
    TranslatePipe, NzTableModule, NzButtonModule, NzIconModule, NzPopconfirmModule, NzTagModule, LoadingComponent,
  ],
  templateUrl: './book-admin-list.component.html',
  styleUrl: './book-admin-list.component.css',
})
export class BookAdminListComponent implements OnInit {
  private readonly bookResource = inject(BookResource);
  private readonly modal = inject(NzModalService);
 
  readonly books = signal<BookListDto[]>([]);
  readonly isLoading = signal(false);
 
  ngOnInit(): void {
    this.loadBooks();
  }
 
  openCreateModal(): void {
    const ref = this.modal.create({
      nzTitle: 'Добавяне на книга',
      nzContent: BookFormComponent,
      nzData: { book: null },
      nzWidth: 800,
      nzFooter: null,
    });
 
    ref.getContentComponent().saved.subscribe(() => {
      ref.close();
      this.loadBooks();
    });
 
    ref.getContentComponent().cancelled.subscribe(() => ref.close());
  }
 
  openEditModal(bookId: number): void {
    this.bookResource.getById(bookId).subscribe({
      next: (book) => {
        const ref = this.modal.create({
          nzTitle: 'Редактиране на книга',
          nzContent: BookFormComponent,
          nzData: { book },
          nzWidth: 800,
          nzFooter: null,
        });
 
        ref.getContentComponent().saved.subscribe(() => {
          ref.close();
          this.loadBooks();
        });
 
        ref.getContentComponent().cancelled.subscribe(() => ref.close());
      },
    });
  }
 
  deleteBook(bookId: number): void {
    this.bookResource.delete(bookId).subscribe({
      next: () => this.loadBooks(),
    });
  }
 
  private loadBooks(): void {
    this.isLoading.set(true);
    this.bookResource.getAll({})
      .pipe(finalize(() => this.isLoading.set(false)))
      .subscribe({ next: (books) => this.books.set(books) });
  }
}