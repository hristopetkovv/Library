import { Component, computed, inject, OnInit, output, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { TranslatePipe } from '@ngx-translate/core';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { FormsModule } from '@angular/forms';
import { finalize } from 'rxjs';
import { NZ_MODAL_DATA } from 'ng-zorro-antd/modal';
import { PublisherDetailDto } from '../../../../publisher/dtos/publisher-detail.dto';
import { PublisherResource } from '../../../../publisher/resources/publisher.resource';
import { PublisherDto } from '../../../../publisher/dtos/publisher.dto';

@Component({
  selector: 'app-publisher-form',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FormsModule,
    TranslatePipe,
    NzFormModule,
    NzInputModule,
    NzButtonModule,
  ],
  templateUrl: './publisher-form.component.html',
  styleUrl: './publisher-form.component.css',
})
export class PublisherFormComponent implements OnInit {
  readonly modalData = inject<{ publisher: PublisherDetailDto }>(NZ_MODAL_DATA, { optional: true });
  readonly saved = output<void>();
  readonly cancelled = output<void>();

  private readonly currentPublisher = this.modalData?.publisher ?? null;
  private readonly fb = inject(FormBuilder);
  private readonly publisherResource = inject(PublisherResource);

  readonly isEditMode = computed(() => !!this.currentPublisher);
  readonly isSubmitting = signal(false);

  readonly form = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(255)]],
  });

  ngOnInit(): void {
    if (this.currentPublisher) {
      this.form.patchValue({
        name: this.currentPublisher.name,
      });
    }
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const val = this.form.getRawValue();
    const dto: PublisherDto = { name: val.name! };
    this.isSubmitting.set(true);

    if (this.isEditMode()) {
      this.publisherResource.update(this.currentPublisher!.id, dto)
        .pipe(finalize(() => this.isSubmitting.set(false)))
        .subscribe({ next: () => this.saved.emit() });
    } else {
      this.publisherResource.create(dto)
        .pipe(finalize(() => this.isSubmitting.set(false)))
        .subscribe({ next: () => this.saved.emit() });
    }
  }

  onCancel(): void {
    this.cancelled.emit();
  }
}
