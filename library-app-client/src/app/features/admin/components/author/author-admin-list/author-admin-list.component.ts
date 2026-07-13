import { Component, OnInit, inject, signal } from '@angular/core';
import { TranslatePipe } from '@ngx-translate/core';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { finalize } from 'rxjs';
import { LoadingComponent } from '../../../../../shared/components/loading/loading.component';
import { AuthorResource } from '../../../../author/resources/author.resource';
import { AuthorListDto } from '../../../../author/dtos/author-list.dto';
import { AuthorFormComponent } from '../author-form/author-form.component';

@Component({
  selector: 'app-author-admin-list',
  standalone: true,
  imports: [
    TranslatePipe, NzTableModule, NzButtonModule, NzIconModule, NzPopconfirmModule, NzTagModule, LoadingComponent,
  ],
  templateUrl: './author-admin-list.component.html',
  styleUrl: './author-admin-list.component.css',
})
export class AuthorAdminListComponent implements OnInit {
  private readonly authorResource = inject(AuthorResource);
  private readonly modal = inject(NzModalService);

  readonly authors = signal<AuthorListDto[]>([]);
  readonly isLoading = signal(false);

  ngOnInit(): void {
    this.loadAuthors();
  }

  openCreateModal(): void {
    const ref = this.modal.create({
      nzTitle: 'Добавяне на автор',
      nzContent: AuthorFormComponent,
      nzData: { author: null },
      nzWidth: 700,
      nzFooter: null,
    });

    ref.getContentComponent().saved.subscribe(() => {
      ref.close();
      this.loadAuthors();
    });

    ref.getContentComponent().cancelled.subscribe(() => ref.close());
  }

  openEditModal(authorId: number): void {
    this.authorResource.getById(authorId).subscribe({
      next: (author) => {
        const ref = this.modal.create({
          nzTitle: 'Редактиране на автор',
          nzContent: AuthorFormComponent,
          nzData: { author },
          nzWidth: 700,
          nzFooter: null,
        });

        ref.getContentComponent().saved.subscribe(() => {
          ref.close();
          this.loadAuthors();
        });

        ref.getContentComponent().cancelled.subscribe(() => ref.close());
      },
    });
  }

  deleteAuthor(authorId: number): void {
    this.authorResource.delete(authorId).subscribe({
      next: () => this.loadAuthors(),
    });
  }

  private loadAuthors(): void {
    this.isLoading.set(true);
    this.authorResource.getAll('')
      .pipe(finalize(() => this.isLoading.set(false)))
      .subscribe({ next: (authors) => this.authors.set(authors) });
  }
}
