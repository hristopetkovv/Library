import { UserLoginInfoDto } from "../../user/dtos/user-login-info.dto";

export interface AuthResponseDto {
    token: string;
    user: UserLoginInfoDto;
}