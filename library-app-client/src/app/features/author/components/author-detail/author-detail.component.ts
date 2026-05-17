import { Component, computed, inject } from '@angular/core';
import { TranslatePipe } from '@ngx-translate/core';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzModalModule, NZ_MODAL_DATA } from 'ng-zorro-antd/modal';
import { AuthorDetailDto } from '../../dtos/author-detail.dto';
 
@Component({
  selector: 'app-author-detail',
  standalone: true,
  imports: [TranslatePipe, NzTagModule, NzDividerModule, NzModalModule],
  templateUrl: './author-detail.component.html',
  styleUrl: './author-detail.component.css',
})
export class AuthorDetailComponent {
  readonly #modalData = inject(NZ_MODAL_DATA);
  readonly author = computed<AuthorDetailDto>(() => this.#modalData.author);
 
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
 
  readonly availableBooks = computed(() =>
    this.author().books.filter(b => b.availableCopies > 0).length
  );
}