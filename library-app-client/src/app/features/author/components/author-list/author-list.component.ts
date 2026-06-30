import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { finalize } from 'rxjs';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzPaginationModule } from 'ng-zorro-antd/pagination';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { TranslatePipe, TranslateService } from '@ngx-translate/core';
import { AuthorResource } from '../../resources/author.resource';
import { LoadingComponent } from '../../../../shared/components/loading/loading.component';
import { AuthorListDto } from '../../dtos/author-list.dto';
import { AuthorCardComponent } from '../author-card/author-card.component';
import { AuthorDetailComponent } from '../author-detail/author-detail.component';
import { SortOptionItem } from '../../../../shared/models/sort-option-item.model';
import { SortSelectComponent } from '../../../../shared/components/sort-select/sort-select.component';
import { AuthorFilterComponent } from '../author-filter/author-filter.component';
 
type SortOption = 'name-asc' | 'name-desc' | 'books-desc' | 'books-asc';

@Component({
  selector: 'app-author-list',
  standalone: true,
  imports: [
    FormsModule,
    NzPaginationModule,
    NzEmptyModule,
    TranslatePipe,
    AuthorCardComponent,
    LoadingComponent,
    SortSelectComponent,
    AuthorFilterComponent
  ],
  templateUrl: './author-list.component.html',
  styleUrl: './author-list.component.css',
})
export class AuthorListComponent implements OnInit {
  private readonly authorResource = inject(AuthorResource);
  private readonly modal = inject(NzModalService);
  private readonly translate = inject(TranslateService);
 
  readonly authors = signal<AuthorListDto[]>([]);
  readonly isLoading = signal(false);
  readonly sortBy = signal<SortOption>('name-asc');
  readonly pageIndex = signal(1);
  readonly pageSize = signal(24);

  private currentFilter = '';
 
  readonly totalAuthors = computed(() => this.authors().length);
 
  readonly pagedAuthors = computed(() => {
    const start = (this.pageIndex() - 1) * this.pageSize();
    return this.sorted().slice(start, start + this.pageSize());
  });

  readonly sortOptions: SortOptionItem<SortOption>[] = [
    { value: 'name-asc', labelKey: 'author.sort.nameAsc' },
    { value: 'name-desc', labelKey: 'author.sort.nameDesc' },
    { value: 'books-desc', labelKey: 'author.sort.booksDesc' },
    { value: 'books-asc', labelKey: 'author.sort.booksAsc' },
  ];

  readonly sorted = computed(() =>
    [...this.authors()].sort((a, b) => {
      switch (this.sortBy()) {
        case 'name-asc': return a.name.localeCompare(b.name, 'bg');
        case 'name-desc': return b.name.localeCompare(a.name, 'bg');
        case 'books-desc': return b.booksCount - a.booksCount;
        case 'books-asc': return a.booksCount - b.booksCount;
        default: return 0;
      }
    })
  );
 
  ngOnInit(): void {
    this.loadAuthors();
  }
 
  onAuthorClick(authorId: number): void {
    this.authorResource.getById(authorId).subscribe({
      next: (author) => {
        this.modal.create({
          nzTitle: this.translate.instant('author.detail.modalTitle'),
          nzContent: AuthorDetailComponent,
          nzData: { author },
          nzWidth: 640,
          nzFooter: null,
          nzBodyStyle: { padding: '24px' },
        });
      },
    });
  }

  onFilterChange(term: string): void {
    this.currentFilter = term;
    this.pageIndex.set(1);
    this.loadAuthors();
  }
 
  onPageChange(page: number): void {
    this.pageIndex.set(page);
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  onSortChange(value: SortOption): void {
    console.log(value)
    this.sortBy.set(value);
    this.pageIndex.set(1);
  }
 
  private loadAuthors(): void {
    this.isLoading.set(true);
    this.authorResource.getAll(this.currentFilter)
      .pipe(finalize(() => this.isLoading.set(false)))
      .subscribe({
        next: (authors) => this.authors.set(authors),
      });
  }
}