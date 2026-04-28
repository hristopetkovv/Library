import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable({ providedIn: 'root' })
export class ForgottenPasswordResource {
    private http = inject(HttpClient);

    private readonly baseUrl = 'api/forgottenpasswords';

    forgotPassword(email: string): Observable<void> {
        return this.http.post<void>(`${this.baseUrl}`, email);
    }
}