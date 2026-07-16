import { Component, OnInit, output, inject, signal } from "@angular/core";
import { ReactiveFormsModule, FormBuilder, Validators } from "@angular/forms";
import { TranslatePipe } from "@ngx-translate/core";
import { NzButtonModule } from "ng-zorro-antd/button";
import { NzFormModule } from "ng-zorro-antd/form";
import { NzSelectModule } from "ng-zorro-antd/select";
import { finalize } from "rxjs";
import { BookListDto } from "../../../../book/dtos/book-list.dto";
import { BookResource } from "../../../../book/resources/book.resource";
import { BorrowingResource } from "../../../../borrowing/resource/borrowing.resource";
import { UserListDto } from "../../../../user/dtos/user-list.dto";
import { UserResource } from "../../../../user/resources/user-resource";

@Component({
  selector: 'app-borrow-book-form',
  standalone: true,
  imports: [ReactiveFormsModule, TranslatePipe, NzFormModule, NzSelectModule, NzButtonModule],
  templateUrl: './borrow-book-form.component.html'
})
export class BorrowBookFormComponent implements OnInit {
  readonly saved = output<void>();
  readonly cancelled = output<void>();

  private readonly fb = inject(FormBuilder);
  private readonly borrowingResource = inject(BorrowingResource);
  private readonly bookResource = inject(BookResource);
  private readonly userResource = inject(UserResource);

  readonly isSubmitting = signal(false);
  readonly books = signal<BookListDto[]>([]);
  readonly users = signal<UserListDto[]>([]);

  readonly form = this.fb.group({
    bookId: [null as number | null, Validators.required],
    userId: [null as number | null, Validators.required],
    dueDate: [null as string | null, Validators.required],
  });

  ngOnInit(): void {
    this.bookResource.getAll({}).subscribe({ next: b => this.books.set(b) });
    this.userResource.getAll({}).subscribe({ next: u => this.users.set(u) });
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const val = this.form.getRawValue();
    this.isSubmitting.set(true);

    this.borrowingResource.borrow({
      bookId: val.bookId!,
      userId: val.userId!
    }).pipe(finalize(() => this.isSubmitting.set(false)))
      .subscribe({ next: () => this.saved.emit() });
  }

  onCancel(): void {
    this.cancelled.emit();
  }
}