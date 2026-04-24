import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { LoginRequestDto } from "../../dtos/auth/login-request.dto";
import { AuthResponseDto } from "../../dtos/auth/auth-response.dto";
import { Observable } from "rxjs";
import { RegisterRequestDto } from "../../dtos/auth/register-request.dto";

@Injectable({ providedIn: 'root' })
export class AuthResource {
    private http = inject(HttpClient);

    private readonly baseUrl = 'api/auth';

    login(request: LoginRequestDto): Observable<AuthResponseDto> {
        return this.http.post<AuthResponseDto>(`${this.baseUrl}/login`, request);
    }

    register(request: RegisterRequestDto): Observable<void> {
        return this.http.post<void>(`${this.baseUrl}/register`, request);
    }
}