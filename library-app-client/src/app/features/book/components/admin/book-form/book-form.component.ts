import { Component, computed, inject, input, OnInit, output, signal } from '@angular/core';
import { AbstractControl, FormBuilder, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { TranslatePipe } from '@ngx-translate/core';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzUploadModule } from 'ng-zorro-antd/upload';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { FormsModule } from '@angular/forms';
import { finalize } from 'rxjs';
import { BookDetailDto } from '../../../dtos/book-detail.dto';
import { BookResource } from '../../../resources/book.resource';
import { AuthorResource } from '../../../../author/resources/author.resource';
import { PublisherResource } from '../../../../publisher/resources/publisher.resource';
import { GenreResource } from '../../../resources/genre.resource';
import { AuthorListDto } from '../../../../author/dtos/author-list.dto';
import { PublisherListDto } from '../../../../publisher/dtos/publisher-list.dto';
import { GenreDto } from '../../../dtos/genre.dto';
import { Language } from '../../../enums/language.enum';
import { CoverType } from '../../../enums/cover-type.enum';
import { UpdateBookDto } from '../../../dtos/update-book.dto';
import { CreateBookDto } from '../../../dtos/create-book.dto';
 
const isbnValidator = (control: AbstractControl): ValidationErrors | null => {
  const val = control.value?.replace(/-/g, '') ?? '';
  return /^\d{10}(\d{3})?$/.test(val) ? null : { invalidIsbn: true };
};
 
@Component({
  selector: 'app-book-form',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FormsModule,
    TranslatePipe,
    NzFormModule,
    NzInputModule,
    NzInputNumberModule,
    NzSelectModule,
    NzButtonModule,
    NzUploadModule,
    NzIconModule
  ],
  templateUrl: './book-form.component.html',
  styleUrl: './book-form.component.css',
})
export class BookFormComponent implements OnInit {
  readonly book = input<BookDetailDto | null>(null); // null = create, populated = edit
  readonly saved = output<void>();
  readonly cancelled = output<void>();
 
  private readonly fb = inject(FormBuilder);
  private readonly bookResource = inject(BookResource);
  private readonly authorResource = inject(AuthorResource);
  private readonly publisherResource = inject(PublisherResource);
  private readonly genreResource = inject(GenreResource);
 
  readonly isEditMode = computed(() => !!this.book());
  readonly isSubmitting = signal(false);
 
  readonly authors = signal<AuthorListDto[]>([]);
  readonly publishers = signal<PublisherListDto[]>([]);
  readonly genres = signal<GenreDto[]>([]);
 
  readonly fictionGenres = computed(() => this.genres().filter(g => g.category === 1));
  readonly nonFictionGenres = computed(() => this.genres().filter(g => g.category === 2));
 
  readonly languages = Object.entries(Language)
    .filter(([, v]) => typeof v === 'number')
    .map(([key, v]) => ({ value: v as number, labelKey: `enums.language.${key}` }));
 
  readonly coverTypes = Object.entries(CoverType)
    .filter(([, v]) => typeof v === 'number')
    .map(([key, v]) => ({ value: v as number, labelKey: `enums.coverType.${key}` }));
 
  readonly currentYear = new Date().getFullYear();
 
  // Upload за edit mode
  coverImageFile = signal<File | null>(null);
  coverPreviewUrl = signal<string | null>(null);
 
  readonly form = this.fb.group({
    title: ['', [Validators.required, Validators.maxLength(300)]],
    authorId: [null as number | null, [Validators.required, Validators.min(1)]],
    publisherId: [null as number | null, [Validators.required, Validators.min(1)]],
    isbn: ['', [Validators.required, isbnValidator]],
    description: [null as string | null, [Validators.maxLength(2000)]],
    pages: [null as number | null, [Validators.required, Validators.min(1)]],
    language: [null as number | null, Validators.required],
    coverType: [null as number | null, Validators.required],
    publicationYear: [null as number | null, [Validators.required, Validators.min(1001), Validators.max(this.currentYear)]],
    totalCopies: [null as number | null, [Validators.required, Validators.min(0)]],
    availableCopies: [null as number | null, [Validators.required, Validators.min(0)]],
    genreIds: [[] as number[], Validators.required],
  });
 
  ngOnInit(): void {
    this.loadDropdowns();
 
    const book = this.book();
    if (book) {
      this.form.patchValue({
        title: book.title,
        authorId: book.author.id,
        publisherId: book.publisher.id,
        isbn: book.isbn,
        description: book.description,
        pages: book.pages,
        language: book.language,
        coverType: book.coverType,
        publicationYear: book.publicationYear,
        totalCopies: book.totalCopies,
        availableCopies: book.availableCopies,
        genreIds: book.genres.map(g => g.id),
      });
 
      if (book.coverImageUrl) {
        this.coverPreviewUrl.set(book.coverImageUrl);
      }
    }
  }
 
  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    if (!file) return;
 
    this.coverImageFile.set(file);
    const reader = new FileReader();
    reader.onload = () => this.coverPreviewUrl.set(reader.result as string);
    reader.readAsDataURL(file);
  }
 
  removeCover(): void {
    this.coverImageFile.set(null);
    this.coverPreviewUrl.set(null);
  }
 
  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
 
    const val = this.form.getRawValue();
    this.isSubmitting.set(true);
 
    if (this.isEditMode()) {
      const dto: UpdateBookDto = {
        title: val.title!,
        authorId: val.authorId!,
        publisherId: val.publisherId!,
        isbn: val.isbn!,
        description: val.description ?? null,
        pages: val.pages!,
        language: val.language!,
        coverType: val.coverType!,
        publicationYear: val.publicationYear!,
        totalCopies: val.totalCopies!,
        availableCopies: val.availableCopies!,
        genreIds: val.genreIds ?? [],
        coverImage: this.coverImageFile(),
      };
 
      this.bookResource.update(this.book()!.id, dto)
        .pipe(finalize(() => this.isSubmitting.set(false)))
        .subscribe({ next: () => this.saved.emit() });
    } else {
      const dto: CreateBookDto = {
        title: val.title!,
        authorId: val.authorId!,
        publisherId: val.publisherId!,
        isbn: val.isbn!,
        description: val.description ?? null,
        pages: val.pages!,
        language: val.language!,
        coverType: val.coverType!,
        publicationYear: val.publicationYear!,
        totalCopies: val.totalCopies!,
        availableCopies: val.availableCopies!,
        genreIds: val.genreIds ?? [],
      };
 
      this.bookResource.create(dto)
        .pipe(finalize(() => this.isSubmitting.set(false)))
        .subscribe({ next: () => this.saved.emit() });
    }
  }
 
  onCancel(): void {
    this.cancelled.emit();
  }
 
  private loadDropdowns(): void {
    this.authorResource.getAll('').subscribe({ next: a => this.authors.set(a) });
    this.publisherResource.getAll('').subscribe({ next: p => this.publishers.set(p) });
    this.genreResource.getAll().subscribe({ next: g => this.genres.set(g) });
  }
}