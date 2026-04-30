import { Injectable } from "@angular/core";
import { BaseResource } from "../../../shared/resources/base.resource";
import { SearchBooksFilterDto } from "../dtos/search-books-filter.dto";
import { Observable } from "rxjs";
import { BookListDto } from "../dtos/book-list.dto";

@Injectable({ providedIn: 'root' })
export class BookResource extends BaseResource {
    protected readonly baseUrl = `api/books`;

    getAll(filter?: SearchBooksFilterDto): Observable<BookListDto[]> {
        return this.http.get<BookListDto[]>(`${this.baseUrl}${this.composeQueryString(filter)}`);
    }
}