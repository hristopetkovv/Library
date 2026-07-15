import { UserRole } from "../../../core/enums/users/user-role.enum";
import { UserStatus } from "../enums/user-status.enum";

export interface UserListDto {
    id: number;
    email: string;
    fullName: string;
    role: UserRole;
    status: UserStatus;
}