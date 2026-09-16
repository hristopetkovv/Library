import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { TranslatePipe, TranslateService } from '@ngx-translate/core';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzSkeletonModule } from 'ng-zorro-antd/skeleton';
import { BookDetailDto } from '../../dtos/book-detail.dto';
import { Category } from '../../enums/category.enum';
import { NZ_MODAL_DATA } from 'ng-zorro-antd/modal';
import { Language } from '../../enums/language.enum';
import { CoverType } from '../../enums/cover-type.enum';
import { ReviewResource } from '../../../review/resources/review.resource';
import { AuthService } from '../../../auth/services/auth.service';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ReviewDto } from '../../../review/dtos/review.dto';
import { finalize } from 'rxjs';
import { NzRateModule } from 'ng-zorro-antd/rate';
import { DatePipe } from '@angular/common';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
 
@Component({
  selector: 'app-book-detail',
  standalone: true,
  imports: [TranslatePipe, NzTagModule, NzDividerModule, NzInputModule, NzSkeletonModule, NzButtonModule, NzPopconfirmModule, NzFormModule, NzRateModule, ReactiveFormsModule, FormsModule, DatePipe],
  templateUrl: './book-detail-modal.component.html',
  styleUrl: './book-detail-modal.component.css',
})
export class BookDetailComponent implements OnInit {
  private readonly translate = inject(TranslateService);
  readonly modalData = inject(NZ_MODAL_DATA);
  private readonly reviewResource = inject(ReviewResource);
  private readonly notification = inject(NzNotificationService);
  private readonly fb = inject(FormBuilder);
  public readonly authService = inject(AuthService);

  readonly book = computed<BookDetailDto>(() => this.modalData.book);

  readonly reviews = signal<ReviewDto[]>([]);
  readonly isLoadingReviews = signal(false);
  readonly isSubmitting = signal(false);
  readonly isAdmin = this.authService.isAdmin;

  readonly currentUserId = computed(() => this.authService.currentUser()?.id);

  readonly reviewForm = this.fb.group({
    content: ['', [Validators.required, Validators.maxLength(1000)]],
    rating: [null as number | null, Validators.required],
  });

  readonly averageRating = computed(() => {
    const r = this.reviews();
    if (!r.length) return 0;
    return r.reduce((sum, r) => sum + r.rating, 0) / r.length;
  });

  readonly placeholderColor = computed(() => {
    const colors = [
      '#2d5a27', '#4a7c43', '#3b6d11', '#1e3d1a',
      '#385f33', '#527a4b', '#2e5426', '#436e3c',
    ];
    return colors[this.book().id % colors.length];
  });

  readonly initials = computed(() =>
    this.book().title
      .split(' ')
      .slice(0, 2)
      .map(w => w[0])
      .join('')
      .toUpperCase()
  );
  readonly isAvailable = computed(() => this.book().availableCopies > 0);
 
  readonly fictionGenres = computed(() =>
    this.book().genres.filter(g => g.genreCategory === Category.Fiction)
  );
 
  readonly nonFictionGenres = computed(() =>
    this.book().genres.filter(g => g.genreCategory === Category.NonFiction)
  );

  languages = Language;
  coverTypes = CoverType;

  ngOnInit(): void {
    this.loadReviews();
  }
 
  genreLabel(genre: { genreName: string; genreNameBg: string }): string {
    return this.translate.getCurrentLang() === 'bg' ? genre.genreName : genre.genreNameBg;
  }

  loadReviews(): void {
    this.isLoadingReviews.set(true);
    this.reviewResource.getByBookId(this.book().id)
      .pipe(finalize(() => this.isLoadingReviews.set(false)))
      .subscribe({ next: r => this.reviews.set(r) });
  }

  submitReview(): void {
    if (this.reviewForm.invalid) {
      this.reviewForm.markAllAsTouched();
      return;
    }

    const val = this.reviewForm.getRawValue();
      this.isSubmitting.set(true);

      this.reviewResource.create({
        bookId: this.book().id,
        content: val.content!,
        rating: val.rating!,
      }).pipe(finalize(() => this.isSubmitting.set(false)))
        .subscribe({
          next: () => {
            this.reviewForm.reset();
            this.loadReviews();
          }
        });
  }

  deleteReview(reviewId: number): void {
    this.reviewResource.delete(reviewId).subscribe({
      next: () => this.loadReviews()
    });
  }

}