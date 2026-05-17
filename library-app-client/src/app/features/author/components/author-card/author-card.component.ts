import { Component, computed, input, output } from '@angular/core';
import { TranslatePipe } from '@ngx-translate/core';
import { AuthorListDto } from '../../dtos/author-list.dto';
import { NzTooltipModule } from 'ng-zorro-antd/tooltip';
 
@Component({
  selector: 'app-author-card',
  standalone: true,
  imports: [TranslatePipe, NzTooltipModule],
  templateUrl: './author-card.component.html',
  styleUrl: './author-card.component.css',
})
export class AuthorCardComponent {
  readonly author = input.required<AuthorListDto>();
  readonly cardClick = output<number>();
 
  readonly avatarColor = computed(() => {
    const colors = [
      '#2d5a27', '#4a7c43', '#3b6d11', '#1e3d1a',
      '#385f33', '#527a4b', '#2e5426', '#436e3c',
    ];
    return colors[this.author().id % colors.length];
  });
 
  readonly initials = computed(() =>
    this.author().name
      .split(' ')
      .slice(0, 2)
      .map(w => w[0])
      .join('')
      .toUpperCase()
  );
 
  onClick(): void {
    this.cardClick.emit(this.author().id);
  }
}