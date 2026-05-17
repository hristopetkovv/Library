import { Injectable } from "@angular/core";
import { BaseResource } from "../../../shared/resources/base.resource";
import { Observable } from "rxjs";
import { AuthorListDto } from "../dtos/author-list.dto";
import { AuthorDetailDto } from "../dtos/author-detail.dto";

@Injectable({ providedIn: 'root' })
export class AuthorResource extends BaseResource {
    protected readonly baseUrl = `api/authors`;

    getAll(authorName: string): Observable<AuthorListDto[]> {
        return this.http.get<AuthorListDto[]>(`${this.baseUrl}?authorName=${authorName}`);
    }

    getById(id: number): Observable<AuthorDetailDto> {
        return this.http.get<AuthorDetailDto>(`${this.baseUrl}/${id}`)
    }
}