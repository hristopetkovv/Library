import { Component, computed, inject, output, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TranslatePipe } from '@ngx-translate/core';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { finalize } from 'rxjs';
import { NZ_MODAL_DATA } from 'ng-zorro-antd/modal';
import { UserListDto } from '../../../../user/dtos/user-list.dto';
import { UserResource } from '../../../../user/resources/user-resource';
import { ChangeUserRoleDto } from '../../../../user/dtos/change-user-role.dto';
import { UserRole } from '../../../../../core/enums/users/user-role.enum';

@Component({
  selector: 'app-user-change-role',
  standalone: true,
  imports: [
    FormsModule,
    TranslatePipe,
    NzFormModule,
    NzSelectModule,
    NzButtonModule
  ],
  templateUrl: './user-change-role.component.html',
  styleUrl: './user-change-role.component.css',
})
export class UserChangeRoleComponent {
  readonly modalData = inject<{ user: UserListDto }>(NZ_MODAL_DATA);
  readonly saved = output<void>();
  readonly cancelled = output<void>();

  private readonly userResource = inject(UserResource);

  readonly user = computed(() => this.modalData.user);
  readonly isSubmitting = signal(false);

  readonly roles = [
    { value: UserRole.admin, labelKey: 'admin.user.roleAdmin' },
    { value: UserRole.member, labelKey: 'admin.user.roleMember' },
  ];

  selectedRole: UserRole = this.modalData.user.role;

  onSubmit(): void {
    this.isSubmitting.set(true);
    const dto: ChangeUserRoleDto = { newRole: this.selectedRole };

    this.userResource.changeRole(this.user().id, dto)
      .pipe(finalize(() => this.isSubmitting.set(false)))
      .subscribe({ next: () => this.saved.emit() });
  }

  onCancel(): void {
    this.cancelled.emit();
  }
}
