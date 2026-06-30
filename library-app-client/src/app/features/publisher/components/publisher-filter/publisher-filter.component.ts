import { Component, OnDestroy, OnInit, computed, output, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Subject, debounceTime, distinctUntilChanged, takeUntil } from 'rxjs';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { TranslatePipe } from '@ngx-translate/core';
 
@Component({
  selector: 'app-publisher-filters',
  standalone: true,
  imports: [FormsModule, NzInputModule, NzIconModule, NzButtonModule, TranslatePipe],
  templateUrl: './publisher-filter.component.html',
  styleUrl: './publisher-filter.component.css',
})
export class PublisherFilterComponent implements OnInit, OnDestroy {
  readonly filterChange = output<string>();
 
  private readonly destroy$ = new Subject<void>();
  private readonly termSubject = new Subject<string>();
 
  readonly term = signal('');
 
  readonly hasActiveFilters = computed(() => !!this.term());
 
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
 
  clearAll(): void {
    this.term.set('');
    this.emit();
  }
 
  private emit(): void {
    this.filterChange.emit(this.term());
  }
}