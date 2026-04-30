import { Component, computed, inject, OnInit, signal } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { NzPaginationModule } from "ng-zorro-antd/pagination";
import { NzSelectModule } from "ng-zorro-antd/select";
import { BookResource } from "../../resources/book.resource";
import { BookListDto } from "../../dtos/book-list.dto";
import { GenreOption } from "../../dtos/genre-option";
import { SearchBooksFilterDto } from "../../dtos/search-books-filter.dto";
import { finalize } from "rxjs";
import { BookCardComponent } from "../book-search-card/book-search-card.component";
import { BookFiltersComponent } from "../book-filter/book-filter.component";
import { NzEmptyModule } from "ng-zorro-antd/empty";
import { NzSpinModule } from "ng-zorro-antd/spin";
import { TranslatePipe, TranslateService } from "@ngx-translate/core";

type SortOption = 'titleAsc' | 'titleDesc' | 'author';

@Component({
  selector: 'app-book-list',
  standalone: true,
  imports: [FormsModule, NzPaginationModule, NzSelectModule, NzEmptyModule, NzSpinModule, BookCardComponent, BookFiltersComponent, TranslatePipe],
  templateUrl: './book-list.component.html',
  styleUrl: './book-list.component.css',
})
export class BookListComponent implements OnInit {
  private readonly bookResource = inject(BookResource);
  private readonly translate = inject(TranslateService);
 
  readonly books = signal<BookListDto[]>([]);
  readonly isLoading = signal(false);
  readonly sortBy = signal<SortOption>('titleAsc');
  readonly pageIndex = signal(1);
  readonly pageSize = signal(20);
  readonly genres = signal<GenreOption[]>([]);
 
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
 
  private loadBooks(): void {
    this.isLoading.set(true);
    this.bookResource.getAll(this.currentFilter)
    .pipe(
             finalize(() => this.isLoading.set(false))
           )
    .subscribe({
      next: (books) => {
        this.books.set(books);
        this.extractGenres(books);
      }
    });
  }
 
  private extractGenres(books: BookListDto[]): void {
    const counts = new Map<string, number>();
    books.forEach(b => b.genres?.forEach(g => counts.set(g, (counts.get(g) ?? 0) + 1)));
    this.genres.set(
      Array.from(counts.entries())
        .map(([name, count], id) => ({ id, name, count }))
        .sort((a, b) => b.count - a.count)
    );
  }
    
}