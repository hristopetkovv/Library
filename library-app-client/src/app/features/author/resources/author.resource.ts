import { Injectable } from "@angular/core";
import { BaseResource } from "../../../shared/resources/base.resource";
import { Observable } from "rxjs";
import { AuthorListDto } from "../dtos/author-list.dto";
import { AuthorDetailDto } from "../dtos/author-detail.dto";
import { AuthorDto } from "../dtos/author.dto";

@Injectable({ providedIn: 'root' })
export class AuthorResource extends BaseResource {
    protected readonly baseUrl = `api/authors`;

    getAll(authorName: string): Observable<AuthorListDto[]> {
        return this.http.get<AuthorListDto[]>(`${this.baseUrl}?authorName=${authorName}`);
    }

    getById(id: number): Observable<AuthorDetailDto> {
        return this.http.get<AuthorDetailDto>(`${this.baseUrl}/${id}`)
    }

    create(request: AuthorDto): Observable<void> {
        return this.http.post<void>(this.baseUrl, request);
    }

    update(id: number, request: AuthorDto): Observable<void> {
        return this.http.put<void>(`${this.baseUrl}/${id}`, request);
    }

    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${id}`);
    }
}