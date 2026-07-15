import { UserRole } from "../../../core/enums/users/user-role.enum";

export interface UserLoginInfoDto {
    id: number;
    firstName: string;
    lastName: string;
    email: string;
    role: UserRole;
}