import { Component, computed, inject, OnInit, signal } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { NzPaginationModule } from "ng-zorro-antd/pagination";
import { NzSelectModule } from "ng-zorro-antd/select";
import { BookResource } from "../../resources/book.resource";
import { BookListDto } from "../../dtos/book-list.dto";
import { SearchBooksFilterDto } from "../../dtos/search-books-filter.dto";
import { finalize } from "rxjs";
import { BookCardComponent } from "../book-search-card/book-search-card.component";
import { BookFiltersComponent } from "../book-filter/book-filter.component";
import { NzEmptyModule } from "ng-zorro-antd/empty";
import { TranslatePipe, TranslateService } from "@ngx-translate/core";
import { LoadingComponent } from "../../../../shared/components/loading/loading.component";
import { GenreDto } from "../../dtos/genre.dto";
import { GenreResource } from "../../resources/genre.resource";
import { NzModalService } from "ng-zorro-antd/modal";
import { BookDetailDto } from "../../dtos/book-detail.dto";
import { BookDetailComponent } from "../book-detail/book-detail-modal.component";

type SortOption = 'titleAsc' | 'titleDesc' | 'author';

@Component({
  selector: 'app-book-list',
  standalone: true,
  imports: [FormsModule, NzPaginationModule, NzSelectModule, NzEmptyModule, BookCardComponent, BookFiltersComponent, LoadingComponent, TranslatePipe],
  templateUrl: './book-list.component.html',
  styleUrl: './book-list.component.css',
})
export class BookListComponent implements OnInit {
  private readonly bookResource = inject(BookResource);
  private readonly genreResource = inject(GenreResource);
  private readonly modal = inject(NzModalService);
  private readonly translate = inject(TranslateService);
 
  readonly books = signal<BookListDto[]>([]);
  readonly isLoading = signal(false);
  readonly sortBy = signal<SortOption>('titleAsc');
  readonly pageIndex = signal(1);
  readonly pageSize = signal(20);
  readonly genres = signal<GenreDto[]>([]);
  readonly selectedBook = signal<BookDetailDto | null>(null);
  readonly modalLoading = signal(false);
 
  private currentFilter: SearchBooksFilterDto = {};
 
  readonly sortOptions: { value: SortOption; }[] = [
    { value: 'titleAsc' },
    { value: 'titleDesc' },
    { value: 'author' }
  ];
 
  readonly sorted = computed(() =>
    [...this.books()].sort((a, b) => {
      switch (this.sortBy()) {
        case 'titleAsc': return a.title.localeCompare(b.title, 'bg');
        case 'titleDesc': return b.title.localeCompare(a.title, 'bg');
        case 'author': return a.authorName.localeCompare(b.authorName, 'bg');
        default: return 0;
      }
    })
  );
 
  readonly pagedBooks = computed(() => {
    const start = (this.pageIndex() - 1) * this.pageSize();
    return this.sorted().slice(start, start + this.pageSize());
  });
 
  readonly totalBooks = computed(() => this.books().length);
 
  ngOnInit(): void {
    this.loadGenres();
    this.loadBooks();
  }
 
  onFilterChange(filter: SearchBooksFilterDto): void {
    this.currentFilter = filter;
    this.pageIndex.set(1);
    this.loadBooks();
  }
 
  onSortChange(): void {
    this.pageIndex.set(1);
  }
 
  onPageChange(page: number): void {
    this.pageIndex.set(page);
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  onBookClick(bookId: number): void {
  this.modalLoading.set(true);
 
  this.bookResource.getById(bookId)
  .pipe(
    finalize(() => this.modalLoading.set(false))
  )
  .subscribe({
    next: (book) => {
      this.selectedBook.set(book);
      this.openModal();
      }
    });
  }
 
  private loadBooks(): void {
    this.isLoading.set(true);
    
    this.bookResource.getAll(this.currentFilter)
    .pipe(
             finalize(() => this.isLoading.set(false))
           )
    .subscribe({
      next: (books) => {
        this.books.set(books);
      }
    });
  }

  private loadGenres(): void {
    this.genreResource.getAll().subscribe({
      next: (genres) => this.genres.set(genres)
    });
  }

  private openModal(): void {
  const book = this.selectedBook();
  if (!book) return;
 
  this.modal.create({
      nzTitle: this.translate.instant('book.detail.modalTitle'),
      nzContent: BookDetailComponent,
      nzData: { book },           // предаваш данните
      nzWidth: 720,
      nzFooter: null,
      nzBodyStyle: { padding: '24px' },
    });
  }
}