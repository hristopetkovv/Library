import { Component, computed, inject, input, OnDestroy, OnInit, output, signal } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { NzButtonModule } from "ng-zorro-antd/button";
import { NzCheckboxModule } from "ng-zorro-antd/checkbox";
import { NzCollapseModule } from "ng-zorro-antd/collapse";
import { NzIconModule } from "ng-zorro-antd/icon";
import { NzInputModule } from "ng-zorro-antd/input";
import { NzSwitchModule } from "ng-zorro-antd/switch";
import { SearchBooksFilterDto } from "../../dtos/search-books-filter.dto";
import { debounceTime, distinctUntilChanged, Subject, takeUntil } from "rxjs";
import { CoverType } from "../../enums/cover-type.enum";
import { Language } from "../../enums/language.enum";
import { TranslatePipe, TranslateService } from "@ngx-translate/core";
import { GenreDto } from "../../dtos/genre.dto";
import { Category } from "../../enums/category.enum";

@Component({
  selector: 'app-book-filters',
  standalone: true,
  imports: [
    FormsModule,
    NzCheckboxModule,
    NzSwitchModule,
    NzInputModule,
    NzIconModule,
    NzCollapseModule,
    NzButtonModule,
    TranslatePipe
  ],
  templateUrl: './book-filter.component.html',
  styleUrl: './book-filter.component.css',
})
export class BookFiltersComponent implements OnInit, OnDestroy {
  private readonly translate = inject(TranslateService);

  readonly genres = input<GenreDto[]>([]);
  readonly fictionGenres = computed(() =>
    this.genres().filter(g => g.genreCategory === Category.Fiction)
  );
  readonly nonFictionGenres = computed(() =>
    this.genres().filter(g => g.genreCategory === Category.NonFiction)
  );
  
  readonly currentLang = computed(() => this.translate.getCurrentLang());

  readonly filterChange = output<SearchBooksFilterDto>();
 
  private readonly destroy$ = new Subject<void>();
  private readonly termSubject = new Subject<string>();
 
  readonly term = signal('');
  readonly availableOnly = signal(false);
  readonly selectedGenreIds = signal<number[]>([]);
  readonly selectedLanguages = signal<Language[]>([]);
  readonly selectedCoverTypes = signal<CoverType[]>([]);
 
  readonly languages = Object.values(Language).filter(v => typeof v === 'number') as Language[];
  readonly coverTypes = Object.values(CoverType).filter(v => typeof v === 'number') as CoverType[];

  readonly languageEnum = Language;
  readonly coverTypeEnum = CoverType;
 
  readonly hasActiveFilters = computed(() =>
    !!(
      this.term() ||
      this.availableOnly() ||
      this.selectedGenreIds().length ||
      this.selectedLanguages().length ||
      this.selectedCoverTypes().length
    )
  );
 
  ngOnInit(): void {
    this.termSubject.pipe(
      debounceTime(350),
      distinctUntilChanged(),
      takeUntil(this.destroy$)
    ).subscribe(() => this.emit());
  }
 
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
 
  onTermChange(value: string): void {
    this.term.set(value);
    this.termSubject.next(value);
  }
 
  onGenreChange(id: number, checked: boolean): void {
    this.selectedGenreIds.update(ids =>
      checked ? [...ids, id] : ids.filter(g => g !== id)
    );
    this.emit();
  }

  genreLabel(genre: GenreDto): string {
    return this.currentLang() === 'bg' ? genre.genreNameBg : genre.genreName;
  }
 
  onLanguageChange(lang: Language, checked: boolean): void {
    this.selectedLanguages.update(langs =>
      checked ? [...langs, lang] : langs.filter(l => l !== lang)
    );
    this.emit();
  }
 
  onCoverTypeChange(type: CoverType, checked: boolean): void {
    this.selectedCoverTypes.update(types =>
      checked ? [...types, type] : types.filter(c => c !== type)
    );
    this.emit();
  }
 
  clearAll(): void {
    this.term.set('');
    this.availableOnly.set(false);
    this.selectedGenreIds.set([]);
    this.selectedLanguages.set([]);
    this.selectedCoverTypes.set([]);
    this.emit();
  }
 
  emit(): void {
    this.filterChange.emit({
      term: this.term() || null,
      availableOnly: this.availableOnly() || null,
      genreIds: this.selectedGenreIds().length ? this.selectedGenreIds() : null,
      language: this.selectedLanguages().length === 1 ? this.selectedLanguages()[0] : null,
      coverType: this.selectedCoverTypes().length === 1 ? this.selectedCoverTypes()[0] : null,
    });
  }
}