import { Component, computed, inject } from '@angular/core';
import { TranslatePipe } from '@ngx-translate/core';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NZ_MODAL_DATA } from 'ng-zorro-antd/modal';
import { PublisherDetailDto } from '../../dtos/publisher-detail.dto';
 
@Component({
  selector: 'app-publisher-detail',
  standalone: true,
  imports: [TranslatePipe, NzDividerModule],
  templateUrl: './publisher-detail.component.html',
  styleUrl: './publisher-detail.component.css',
})
export class PublisherDetailComponent {
  readonly #modalData = inject(NZ_MODAL_DATA);
  readonly publisher = computed<PublisherDetailDto>(() => this.#modalData.publisher);
 
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
 
  readonly availableBooks = computed(() =>
    this.publisher().books.filter(b => b.availableCopies > 0).length
  );
}