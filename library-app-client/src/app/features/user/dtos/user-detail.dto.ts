import { UserRole } from "../../../core/enums/users/user-role.enum";

export interface UserDetailDto {
    id: number;
    email: string;
    role: UserRole;
    firstName: string;
    lastName: string;
    address: string;
    phoneNumber: string;
}