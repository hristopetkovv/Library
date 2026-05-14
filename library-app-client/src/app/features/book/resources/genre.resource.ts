import { Injectable } from "@angular/core";
import { BaseResource } from "../../../shared/resources/base.resource";
import { Observable } from "rxjs";
import { GenreDto } from "../dtos/genre.dto";

@Injectable({ providedIn: 'root' })
export class GenreResource extends BaseResource {
    protected readonly baseUrl = `api/genres`;

    getAll(): Observable<GenreDto[]> {
        return this.http.get<GenreDto[]>(`${this.baseUrl}`);
    }
}