import { Component, OnDestroy, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Subject, debounceTime, distinctUntilChanged, finalize, takeUntil } from 'rxjs';
import { TranslatePipe } from '@ngx-translate/core';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { LoadingComponent } from '../../../../../shared/components/loading/loading.component';
import { UserResource } from '../../../../user/resources/user-resource';
import { UserListDto } from '../../../../user/dtos/user-list.dto';
import { SearchUsersFilterDto } from '../../../../user/dtos/search-users-filter.dto';
import { UserChangeRoleComponent } from '../user-change-role/user-change-role.component';
import { RoleLabelPipe } from '../../../../../shared/pipes/role-label.pipe';
import { NzTooltipModule } from 'ng-zorro-antd/tooltip';

@Component({
  selector: 'app-user-admin-list',
  standalone: true,
  imports: [
    FormsModule,
    NzTableModule,
    NzButtonModule,
    NzInputModule,
    NzIconModule,
    NzPopconfirmModule,
    NzTagModule,
    NzTooltipModule,
    LoadingComponent,
    TranslatePipe,
    RoleLabelPipe,
  ],
  templateUrl: './user-admin-list.component.html',
  styleUrl: './user-admin-list.component.css',
})
export class UserAdminListComponent implements OnInit, OnDestroy {
  private readonly userResource = inject(UserResource);
  private readonly modal = inject(NzModalService);

  private readonly destroy$ = new Subject<void>();
  private readonly emailSubject = new Subject<string>();
  private readonly fullNameSubject = new Subject<string>();

  readonly users = signal<UserListDto[]>([]);
  readonly isLoading = signal(false);
  readonly emailFilter = signal('');
  readonly fullNameFilter = signal('');

  ngOnInit(): void {
    this.emailSubject.pipe(
      debounceTime(350),
      distinctUntilChanged(),
      takeUntil(this.destroy$)
    ).subscribe(() => this.loadUsers());

    this.fullNameSubject.pipe(
      debounceTime(350),
      distinctUntilChanged(),
      takeUntil(this.destroy$)
    ).subscribe(() => this.loadUsers());

    this.loadUsers();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onEmailChange(value: string): void {
    this.emailFilter.set(value);
    this.emailSubject.next(value);
  }

  onFullNameChange(value: string): void {
    this.fullNameFilter.set(value);
    this.fullNameSubject.next(value);
  }

  deleteUser(userId: number): void {
    this.userResource.delete(userId).subscribe({
      next: () => this.loadUsers(),
    });
  }

  openChangeRoleModal(user: UserListDto): void {
    const ref = this.modal.create({
      nzTitle: 'Промяна на роля',
      nzContent: UserChangeRoleComponent,
      nzData: { user },
      nzWidth: 450,
      nzFooter: null,
    });

    ref.getContentComponent().saved.subscribe(() => {
      ref.close();
      this.loadUsers();
    });

    ref.getContentComponent().cancelled.subscribe(() => ref.close());
  }

  activateUser(userId: number): void {
    this.userResource.activate(userId).subscribe({
      next: () => this.loadUsers(),
    });
  }

  deactivateUser(userId: number): void {
    this.userResource.deactivate(userId).subscribe({
      next: () => this.loadUsers(),
    });
  }

  private loadUsers(): void {
    this.isLoading.set(true);

    const filter: SearchUsersFilterDto = {
      email: this.emailFilter() || null,
      fullName: this.fullNameFilter() || null,
    };
    
    this.userResource.getAll(filter)
      .pipe(finalize(() => this.isLoading.set(false)))
      .subscribe({ next: (users) => this.users.set(users) });
  }
}
