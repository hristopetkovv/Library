import { UserLoginInfoDto } from "../users/user-login-info.dto";

export interface AuthResponseDto {
    token: string;
    user: UserLoginInfoDto;
}