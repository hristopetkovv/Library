import { Injectable } from "@angular/core";
import { LoginRequestDto } from "../dtos/login-request.dto";
import { Observable } from "rxjs";
import { RegisterRequestDto } from "../dtos/register-request.dto";
import { AuthResponseDto } from "../dtos/auth-response.dto";
import { BaseResource } from "../../../shared/resources/base.resource";

@Injectable({ providedIn: 'root' })
export class AuthResource extends BaseResource {
    protected readonly baseUrl = 'api/auth';

    login(request: LoginRequestDto): Observable<AuthResponseDto> {
        return this.http.post<AuthResponseDto>(`${this.baseUrl}/login`, request);
    }

    register(request: RegisterRequestDto): Observable<void> {
        return this.http.post<void>(`${this.baseUrl}/register`, request);
    }
}