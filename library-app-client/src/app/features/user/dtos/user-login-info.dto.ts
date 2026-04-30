import { UserRole } from "../../../core/enums/users/user-role.enum";

export interface UserLoginInfoDto {
    firstName: string;
    lastName: string;
    email: string;
    role: UserRole;
}