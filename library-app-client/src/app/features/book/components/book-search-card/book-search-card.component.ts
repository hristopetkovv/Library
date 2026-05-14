import { Component, computed, input, Input, output } from "@angular/core";
import { RouterLink } from "@angular/router";
import { NzTagModule } from "ng-zorro-antd/tag";
import { NzTooltipModule } from "ng-zorro-antd/tooltip";
import { BookListDto } from "../../dtos/book-list.dto";
import { TranslatePipe } from "@ngx-translate/core";

@Component({
  selector: 'app-book-card',
  standalone: true,
  imports: [RouterLink, NzTagModule, NzTooltipModule, TranslatePipe],
  templateUrl: './book-search-card.component.html',
  styleUrl: './book-search-card.component.css',
})
export class BookCardComponent {
  readonly book = input.required<BookListDto>();
  readonly cardClick = output<number>();
 
  readonly isAvailable = computed(() => this.book().availableCopies > 0);
 
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

  onClick(): void {
    this.cardClick.emit(this.book().id);
  }
}