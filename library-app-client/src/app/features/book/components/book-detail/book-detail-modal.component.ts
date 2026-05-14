import { Component, computed, inject, input } from '@angular/core';
import { TranslatePipe, TranslateService } from '@ngx-translate/core';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzSkeletonModule } from 'ng-zorro-antd/skeleton';
import { BookDetailDto } from '../../dtos/book-detail.dto';
import { Category } from '../../enums/category.enum';
import { NZ_MODAL_DATA } from 'ng-zorro-antd/modal';
import { Language } from '../../enums/language.enum';
import { CoverType } from '../../enums/cover-type.enum';
 
@Component({
  selector: 'app-book-detail',
  standalone: true,
  imports: [TranslatePipe, NzTagModule, NzDividerModule, NzSkeletonModule],
  templateUrl: './book-detail-modal.component.html',
  styleUrl: './book-detail-modal.component.css',
})
export class BookDetailComponent {
  private readonly translate = inject(TranslateService);
  readonly modalData = inject(NZ_MODAL_DATA);

  readonly book = computed<BookDetailDto>(() => this.modalData.book);

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
    this.book().genres.filter(g => g.category === Category.Fiction)
  );
 
  readonly nonFictionGenres = computed(() =>
    this.book().genres.filter(g => g.category === Category.NonFiction)
  );

  languages = Language;
  coverTypes = CoverType;
 
  genreLabel(genre: { name: string; nameBg: string }): string {
    return this.translate.getCurrentLang() === 'bg' ? genre.nameBg : genre.name;
  }
}