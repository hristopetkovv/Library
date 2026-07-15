import { DatePipe } from "@angular/common";
import { Component, computed, inject, OnInit, signal } from "@angular/core";
import { TranslatePipe } from "@ngx-translate/core";
import { NzTableModule } from "ng-zorro-antd/table";
import { NzTagModule } from "ng-zorro-antd/tag";
import { finalize } from "rxjs";
import { BorrowingResource } from "../../../../borrowing/resource/borrowing.resource";
import { BorrowingBasicDto } from "../../../../borrowing/dtos/borrowing-basic.dto";
import { BorrowingStatus } from "../../../../borrowing/enums/borrowings-status.enum";
import { NzSelectModule } from "ng-zorro-antd/select";
import { FormsModule } from "@angular/forms";

@Component({
  selector: 'app-user-borrowings',
  standalone: true,
  imports: [
    TranslatePipe,
    FormsModule,
    NzTableModule,
    NzTagModule,
    NzSelectModule,
    DatePipe,
  ],
  templateUrl: './user-borrowings.component.html',
})
export class UserBorrowingsComponent implements OnInit {
  private readonly borrowingResource = inject(BorrowingResource);

  readonly borrowings = signal<BorrowingBasicDto[]>([]);
  readonly isLoading = signal(false);
  readonly statusFilter  = signal<BorrowingStatus | null>(null);

  readonly filteredBorrowings = computed(() => {
    const status = this.statusFilter();
    if (!status) return this.borrowings();
    return this.borrowings().filter(b => b.status === status);
    });

  ngOnInit(): void {
    this.isLoading.set(true);

    this.borrowingResource.getMy()
      .pipe(finalize(() => this.isLoading.set(false)))
      .subscribe({ next: b => this.borrowings.set(b) });
  }
}