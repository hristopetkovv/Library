import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { LibraryStatsDto } from "../../../features/library/dtos/library-stats.dto";

@Injectable({ providedIn: 'root' })
export class LibraryResource {
    private http = inject(HttpClient);

    private readonly baseUrl = 'api/libraries';

    getLibraryStats(): Observable<LibraryStatsDto> {
        return this.http.get<LibraryStatsDto>(`${this.baseUrl}/stats`);
    }
}