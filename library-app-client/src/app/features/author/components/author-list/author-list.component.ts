import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged, finalize, Subject, takeUntil } from 'rxjs';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzPaginationModule } from 'ng-zorro-antd/pagination';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { TranslatePipe, TranslateService } from '@ngx-translate/core';
import { AuthorResource } from '../../resources/author.resource';
import { LoadingComponent } from '../../../../shared/components/loading/loading.component';
import { AuthorListDto } from '../../dtos/author-list.dto';
import { AuthorCardComponent } from '../author-card/author-card.component';
import { AuthorDetailComponent } from '../author-detail/author-detail.component';
 
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
  ],
  templateUrl: './author-list.component.html',
  styleUrl: './author-list.component.css',
})
export class AuthorListComponent implements OnInit {
  private readonly authorResource = inject(AuthorResource);
  private readonly modal = inject(NzModalService);
  private readonly translate = inject(TranslateService);
 
  private readonly destroy$ = new Subject<void>();
  private readonly termSubject = new Subject<string>();
 
  readonly authors = signal<AuthorListDto[]>([]);
  readonly isLoading = signal(false);
  readonly term = signal('');
  readonly pageIndex = signal(1);
  readonly pageSize = signal(24);
 
  readonly totalAuthors = computed(() => this.authors().length);
 
  readonly pagedAuthors = computed(() => {
    const start = (this.pageIndex() - 1) * this.pageSize();
    return this.authors().slice(start, start + this.pageSize());
  });
 
  ngOnInit(): void {
    this.termSubject.pipe(
      debounceTime(350),
      distinctUntilChanged(),
      takeUntil(this.destroy$)
    ).subscribe(() => this.loadAuthors());
 
    this.loadAuthors();
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
 
  onAuthorClick(authorId: number): void {
    this.authorResource.getById(authorId).subscribe({
      next: (author) => {
        this.modal.create({
          nzTitle: this.translate.instant('book.detail.modalTitle'),
          nzContent: AuthorDetailComponent,
          nzData: { author },
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
 
  private loadAuthors(): void {
    this.isLoading.set(true);
    this.authorResource.getAll(this.term())
      .pipe(finalize(() => this.isLoading.set(false)))
      .subscribe({
        next: (authors) => this.authors.set(authors),
      });
  }
}