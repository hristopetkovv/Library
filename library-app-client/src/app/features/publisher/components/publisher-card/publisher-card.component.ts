import { Component, computed, input, output } from '@angular/core';
import { TranslatePipe } from '@ngx-translate/core';
import { NzTooltipModule } from 'ng-zorro-antd/tooltip';
import { PublisherListDto } from '../../dtos/publisher-list.dto';
 
@Component({
  selector: 'app-publisher-card',
  standalone: true,
  imports: [TranslatePipe, NzTooltipModule],
  templateUrl: './publisher-card.component.html',
  styleUrl: './publisher-card.component.css',
})
export class PublisherCardComponent {
  readonly publisher = input.required<PublisherListDto>();
  readonly cardClick = output<number>();
 
  readonly avatarColor = computed(() => {
    const colors = [
      '#2d5a27', '#4a7c43', '#3b6d11', '#1e3d1a',
      '#385f33', '#527a4b', '#2e5426', '#436e3c',
    ];
    return colors[this.publisher().id % colors.length];
  });
 
  readonly initials = computed(() =>
    this.publisher().name
      .split(' ')
      .slice(0, 2)
      .map(w => w[0])
      .join('')
      .toUpperCase()
  );
 
  onClick(): void {
    this.cardClick.emit(this.publisher().id);
  }
}