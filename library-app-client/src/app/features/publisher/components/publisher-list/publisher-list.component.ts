import { Component, OnDestroy, OnInit, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged, finalize, takeUntil } from 'rxjs';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzPaginationModule } from 'ng-zorro-antd/pagination';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { TranslatePipe, TranslateService } from '@ngx-translate/core';
import { PublisherCardComponent } from '../publisher-card/publisher-card.component';
import { PublisherDetailComponent } from '../publisher-detail/publisher-detail.component';
import { LoadingComponent } from '../../../../shared/components/loading/loading.component';
import { PublisherResource } from '../../resources/publisher.resource';
import { PublisherListDto } from '../../dtos/publisher-list.dto';
import { SortSelectComponent } from '../../../../shared/components/sort-select/sort-select.component';
import { PublisherFilterComponent } from '../publisher-filter/publisher-filter.component';
import { SortOptionItem } from '../../../../shared/models/sort-option-item.model';
 
type SortOption = 'name-asc' | 'name-desc' | 'books-desc' | 'books-asc';

@Component({
  selector: 'app-publisher-list',
  standalone: true,
  imports: [
    FormsModule,
    NzPaginationModule,
    NzEmptyModule,
    TranslatePipe,
    PublisherCardComponent,
    LoadingComponent,
    SortSelectComponent,
    PublisherFilterComponent
  ],
  templateUrl: './publisher-list.component.html',
  styleUrl: './publisher-list.component.css',
})
export class PublisherListComponent implements OnInit {
  private readonly publisherResource = inject(PublisherResource);
  private readonly modal = inject(NzModalService);
  private readonly translate = inject(TranslateService);
 
  readonly publishers = signal<PublisherListDto[]>([]);
  readonly isLoading = signal(false);
  readonly sortBy = signal<SortOption>('name-asc');
  readonly pageIndex = signal(1);
  readonly pageSize = signal(24);

  private currentFilter = '';

  readonly sortOptions: SortOptionItem<SortOption>[] = [
    { value: 'name-asc', labelKey: 'publisher.sort.nameAsc' },
    { value: 'name-desc', labelKey: 'publisher.sort.nameDesc' },
    { value: 'books-desc', labelKey: 'publisher.sort.booksDesc' },
    { value: 'books-asc', labelKey: 'publisher.sort.booksAsc' },
  ];

  readonly sorted = computed(() =>
    [...this.publishers()].sort((a, b) => {
      switch (this.sortBy()) {
        case 'name-asc': return a.name.localeCompare(b.name, 'bg');
        case 'name-desc': return b.name.localeCompare(a.name, 'bg');
        case 'books-desc': return b.booksCount - a.booksCount;
        case 'books-asc': return a.booksCount - b.booksCount;
        default: return 0;
      }
    })
  );
 
  readonly totalPublishers = computed(() => this.publishers().length);
 
  readonly pagedPublishers = computed(() => {
    const start = (this.pageIndex() - 1) * this.pageSize();
    return this.sorted().slice(start, start + this.pageSize());
  });
 
  ngOnInit(): void {
    this.loadPublishers();
  }
 
  onPublisherClick(publisherId: number): void {
    this.publisherResource.getById(publisherId).subscribe({
      next: (publisher) => {
        this.modal.create({
          nzTitle: this.translate.instant('publisher.detail.modalTitle'),
          nzContent: PublisherDetailComponent,
          nzData: { publisher },
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
    this.loadPublishers();
  }

  onSortChange(value: SortOption): void {
    this.sortBy.set(value);
    this.pageIndex.set(1);
  }
 
  onPageChange(page: number): void {
    this.pageIndex.set(page);
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }
 
  private loadPublishers(): void {
    this.isLoading.set(true);
    this.publisherResource.getAll(this.currentFilter)
      .pipe(finalize(() => this.isLoading.set(false)))
      .subscribe({
        next: (publishers) => this.publishers.set(publishers),
      });
  }
}