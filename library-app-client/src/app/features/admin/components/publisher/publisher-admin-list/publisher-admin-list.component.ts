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
import { PublisherFormComponent } from '../publisher-form/publisher-form.component';
import { PublisherResource } from '../../../../publisher/resources/publisher.resource';
import { PublisherListDto } from '../../../../publisher/dtos/publisher-list.dto';

@Component({
  selector: 'app-publisher-admin-list',
  standalone: true,
  imports: [
    TranslatePipe, NzTableModule, NzButtonModule, NzIconModule, NzPopconfirmModule, NzTagModule, LoadingComponent,
  ],
  templateUrl: './publisher-admin-list.component.html',
  styleUrl: './publisher-admin-list.component.css',
})
export class PublisherAdminListComponent implements OnInit {
  private readonly publisherResource = inject(PublisherResource);
  private readonly modal = inject(NzModalService);

  readonly publishers = signal<PublisherListDto[]>([]);
  readonly isLoading = signal(false);

  ngOnInit(): void {
    this.loadPublishers();
  }

  openCreateModal(): void {
    const ref = this.modal.create({
      nzTitle: 'Добавяне на издателство',
      nzContent: PublisherFormComponent,
      nzData: { publisher: null },
      nzWidth: 600,
      nzFooter: null,
    });

    ref.getContentComponent().saved.subscribe(() => {
      ref.close();
      this.loadPublishers();
    });

    ref.getContentComponent().cancelled.subscribe(() => ref.close());
  }

  openEditModal(publisherId: number): void {
    this.publisherResource.getById(publisherId).subscribe({
      next: (publisher) => {
        const ref = this.modal.create({
          nzTitle: 'Редактиране на издателство',
          nzContent: PublisherFormComponent,
          nzData: { publisher },
          nzWidth: 600,
          nzFooter: null,
        });

        ref.getContentComponent().saved.subscribe(() => {
          ref.close();
          this.loadPublishers();
        });

        ref.getContentComponent().cancelled.subscribe(() => ref.close());
      },
    });
  }

  deletePublisher(publisherId: number): void {
    this.publisherResource.delete(publisherId).subscribe({
      next: () => this.loadPublishers(),
    });
  }

  private loadPublishers(): void {
    this.isLoading.set(true);
    this.publisherResource.getAll('')
      .pipe(finalize(() => this.isLoading.set(false)))
      .subscribe({ next: (publishers) => this.publishers.set(publishers) });
  }
}
