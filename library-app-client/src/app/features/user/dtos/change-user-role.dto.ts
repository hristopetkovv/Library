import { UserRole } from "../../../core/enums/users/user-role.enum";

export interface ChangeUserRoleDto {
    newRole: UserRole;
}