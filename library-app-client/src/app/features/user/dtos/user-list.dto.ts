import { UserRole } from "../../../core/enums/users/user-role.enum";

export interface UserListDto {
    id: number;
    email: string;
    firstName: string;
    lastName: string;
    role: UserRole;
}