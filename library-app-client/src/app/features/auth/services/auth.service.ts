import { computed, inject, Injectable, signal } from "@angular/core";
import { Router } from "@angular/router";
import { AuthResource } from "../resources/auth.resource";
import { LoginRequestDto } from "../dtos/login-request.dto";
import { tap } from "rxjs";
import { RegisterRequestDto } from "../dtos/register-request.dto";
import { UserLoginInfoDto } from "../../user/dtos/user-login-info.dto";
import { AuthResponseDto } from "../dtos/auth-response.dto";
import { UserRole } from "../../../core/enums/users/user-role.enum";

@Injectable({ providedIn: 'root' })
export class AuthService {
    private readonly authResource = inject(AuthResource);
    private router = inject(Router);

    currentUser = signal<UserLoginInfoDto | null>(this.getUserFromStorage());
    isAuthenticated = computed(() => !!this.currentUser());
    isAdmin = computed(() => this.currentUser()?.role === UserRole.admin);

    login(request: LoginRequestDto) {
        return this.authResource.login(request).pipe(
            tap((response: AuthResponseDto) => {
              this.saveSession(response.token, response.user);
              this.currentUser.set(response.user);  
            })
        );
    }

    logout() {
        localStorage.removeItem('token');
        localStorage.removeItem('user');
        
        this.currentUser.set(null);
        this.router.navigate(["/books"]);
    }

    register(request: RegisterRequestDto) {
        return this.authResource.register(request);
    }

    getToken(): string | null {
        return localStorage.getItem('token');
    }

    hasRole(role: UserRole): boolean {
        return this.currentUser()?.role === role;
    }

    isTokenExpired(token: string) {
        try {
            if (token) {
                const payload = JSON.parse(atob(token.split('.')[1]));
                return payload.exp * 1000 < Date.now();
            }
            return true;
        } catch {
            return true;
        }
    }

    private saveSession(token: string, user: UserLoginInfoDto) {
        localStorage.setItem('token', token);
        localStorage.setItem('user', JSON.stringify(user));
    }

    private getUserFromStorage(): UserLoginInfoDto | null {
        const userJson = localStorage.getItem('user');

        return userJson ? JSON.parse(userJson) : null;
    }
}