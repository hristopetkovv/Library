import { Component, OnDestroy, OnInit, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged, finalize, Subject, takeUntil } from 'rxjs';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzPaginationModule } from 'ng-zorro-antd/pagination';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { TranslatePipe, TranslateService } from '@ngx-translate/core';
import { PublisherCardComponent } from '../publisher-card/publisher-card.component';
import { PublisherDetailComponent } from '../publisher-detail/publisher-detail.component';
import { LoadingComponent } from '../../../../shared/components/loading/loading.component';
import { PublisherResource } from '../../resources/publisher.resource';
import { PublisherListDto } from '../../dtos/publisher-list.dto';
 
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
  ],
  templateUrl: './publisher-list.component.html',
  styleUrl: './publisher-list.component.css',
})
export class PublisherListComponent implements OnInit, OnDestroy {
  private readonly publisherResource = inject(PublisherResource);
  private readonly modal = inject(NzModalService);
  private readonly translate = inject(TranslateService);
 
  private readonly destroy$ = new Subject<void>();
  private readonly termSubject = new Subject<string>();
 
  readonly publishers = signal<PublisherListDto[]>([]);
  readonly isLoading = signal(false);
  readonly term = signal('');
  readonly pageIndex = signal(1);
  readonly pageSize = signal(24);
 
  readonly totalPublishers = computed(() => this.publishers().length);
 
  readonly pagedPublishers = computed(() => {
    const start = (this.pageIndex() - 1) * this.pageSize();
    return this.publishers().slice(start, start + this.pageSize());
  });
 
  ngOnInit(): void {
    this.termSubject.pipe(
      debounceTime(350),
      distinctUntilChanged(),
      takeUntil(this.destroy$)
    ).subscribe(() => this.loadPublishers());
 
    this.loadPublishers();
  }
 
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
 
  onTermChange(value: string): void {
    this.term.set(value);
    this.termSubject.next(value);
    this.pageIndex.set(1);
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
 
  onPageChange(page: number): void {
    this.pageIndex.set(page);
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }
 
  private loadPublishers(): void {
    this.isLoading.set(true);
    this.publisherResource.getAll(this.term())
      .pipe(finalize(() => this.isLoading.set(false)))
      .subscribe({
        next: (publishers) => this.publishers.set(publishers),
      });
  }
}