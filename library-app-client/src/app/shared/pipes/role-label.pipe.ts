import { Pipe, PipeTransform } from '@angular/core';
import { UserRole } from '../../core/enums/users/user-role.enum';

@Pipe({
  name: 'roleLabel',
  standalone: true,
})
export class RoleLabelPipe implements PipeTransform {
  transform(value: UserRole): string {
    switch (value) {
      case UserRole.admin:
        return 'admin.user.roleAdmin';
      case UserRole.member:
        return 'admin.user.roleMember';
      default:
        return '';
    }
  }
}
