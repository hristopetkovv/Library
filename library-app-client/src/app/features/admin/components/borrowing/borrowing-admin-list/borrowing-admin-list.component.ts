import { DatePipe, NgTemplateOutlet } from "@angular/common";
import { Component, OnInit, OnDestroy, inject, signal, computed } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { TranslatePipe, TranslateService } from "@ngx-translate/core";
import { NzButtonModule } from "ng-zorro-antd/button";
import { NzGridModule } from "ng-zorro-antd/grid";
import { NzIconModule } from "ng-zorro-antd/icon";
import { NzInputModule } from "ng-zorro-antd/input";
import { NzPopconfirmModule } from "ng-zorro-antd/popconfirm";
import { NzTableModule } from "ng-zorro-antd/table";
import { NzTabsModule } from "ng-zorro-antd/tabs";
import { NzTagModule } from "ng-zorro-antd/tag";
import { NzTooltipModule } from "ng-zorro-antd/tooltip";
import { Subject, debounceTime, distinctUntilChanged, takeUntil, finalize } from "rxjs";
import { BorrowingDetailDto } from "../../../../borrowing/dtos/borrowing-detail.dto";
import { SearchBorrowingsFilterDto } from "../../../../borrowing/dtos/search-borrowing-filter.dto";
import { BorrowingStatus } from "../../../../borrowing/enums/borrowings-status.enum";
import { BorrowingResource } from "../../../../borrowing/resource/borrowing.resource";
import { BorrowBookFormComponent } from "../borrow-book-form/borrow-book-form.component";
import { NzModalService } from "ng-zorro-antd/modal";

@Component({
  selector: 'app-borrowing-admin-list',
  standalone: true,
  imports: [
    FormsModule,
    TranslatePipe,
    NzTableModule,
    NzTabsModule,
    NzInputModule,
    NzButtonModule,
    NzIconModule,
    NzTagModule,
    NzGridModule,
    NzPopconfirmModule,
    NzTooltipModule,
    NgTemplateOutlet,
    DatePipe,
  ],
  templateUrl: './borrowing-admin-list.component.html',
  styleUrl: './borrowing-admin-list.component.css',
})
export class BorrowingAdminListComponent implements OnInit, OnDestroy {
  private readonly borrowingResource = inject(BorrowingResource);
  private readonly translate = inject(TranslateService);
  private readonly modal = inject(NzModalService);
  
  private readonly destroy$ = new Subject<void>();
  private readonly filterSubject = new Subject<void>();

  readonly BorrowingStatus = BorrowingStatus;

  readonly isLoading = signal(false);
  readonly allBorrowings = signal<BorrowingDetailDto[]>([]);
  readonly today = new Date().toISOString();

  readonly filter = signal<SearchBorrowingsFilterDto>({});

  readonly activeBorrowings = computed(() =>
    this.allBorrowings().filter(b => b.status === BorrowingStatus.Borrowed)
  );

  readonly overdueBorrowings = computed(() =>
    this.allBorrowings().filter(b => b.status === BorrowingStatus.Overdue)
  );

  readonly returnedBorrowings = computed(() =>
    this.allBorrowings().filter(b => b.status === BorrowingStatus.Returned)
  );

  ngOnInit(): void {
    this.filterSubject.pipe(
      debounceTime(350),
      distinctUntilChanged(),
      takeUntil(this.destroy$)
    ).subscribe(() => this.loadBorrowings());

    this.loadBorrowings();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onFilterChange(field: keyof SearchBorrowingsFilterDto, value: string): void {
    this.filter.update(f => ({ ...f, [field]: value || null }));
    this.filterSubject.next();
  }

  returnBook(id: number): void {
    this.borrowingResource.returnBook(id).subscribe({
      next: () => this.loadBorrowings(),
    });
  }

  openBorrowModal(): void {
  const ref = this.modal.create({
    nzTitle: this.translate.instant('admin.borrowing.borrowTitle'),
    nzContent: BorrowBookFormComponent,
    nzWidth: 500,
    nzFooter: null,
  });

  ref.getContentComponent().saved.subscribe(() => {
    ref.close();
    this.loadBorrowings();
  });

  ref.getContentComponent().cancelled.subscribe(() => ref.close());
}

  private loadBorrowings(): void {
    this.isLoading.set(true);
    this.borrowingResource.getAll(this.filter())
      .pipe(finalize(() => this.isLoading.set(false)))
      .subscribe({ next: b => this.allBorrowings.set(b) });
  }
}