import { Component, computed, inject, OnInit, output, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { TranslatePipe } from '@ngx-translate/core';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { FormsModule } from '@angular/forms';
import { finalize } from 'rxjs';
import { NZ_MODAL_DATA } from 'ng-zorro-antd/modal';
import { AuthorDetailDto } from '../../../../author/dtos/author-detail.dto';
import { AuthorResource } from '../../../../author/resources/author.resource';
import { AuthorDto } from '../../../../author/dtos/author.dto';

@Component({
  selector: 'app-author-form',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FormsModule,
    TranslatePipe,
    NzFormModule,
    NzInputModule,
    NzButtonModule,
  ],
  templateUrl: './author-form.component.html',
  styleUrl: './author-form.component.css',
})
export class AuthorFormComponent implements OnInit {
  readonly modalData = inject<{ author: AuthorDetailDto }>(NZ_MODAL_DATA, { optional: true });
  readonly saved = output<void>();
  readonly cancelled = output<void>();

  private readonly currentAuthor = this.modalData?.author ?? null;
  private readonly fb = inject(FormBuilder);
  private readonly authorResource = inject(AuthorResource);

  readonly isEditMode = computed(() => !!this.currentAuthor);
  readonly isSubmitting = signal(false);

  readonly form = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(200)]],
    biography: [null as string | null, [Validators.maxLength(2000)]],
  });

  ngOnInit(): void {
    if (this.currentAuthor) {
      this.form.patchValue({
        name: this.currentAuthor.name,
        biography: this.currentAuthor.biography,
      });
    }
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const val = this.form.getRawValue();
    const dto: AuthorDto = {
      name: val.name!,
      biography: val.biography ?? '',
    };
    this.isSubmitting.set(true);

    if (this.isEditMode()) {
      this.authorResource.update(this.currentAuthor!.id, dto)
        .pipe(finalize(() => this.isSubmitting.set(false)))
        .subscribe({ next: () => this.saved.emit() });
    } else {
      this.authorResource.create(dto)
        .pipe(finalize(() => this.isSubmitting.set(false)))
        .subscribe({ next: () => this.saved.emit() });
    }
  }

  onCancel(): void {
    this.cancelled.emit();
  }
}
