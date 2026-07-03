import { Injectable } from "@angular/core";
import { BaseResource } from "../../../shared/resources/base.resource";
import { SearchBooksFilterDto } from "../dtos/search-books-filter.dto";
import { Observable } from "rxjs";
import { BookListDto } from "../dtos/book-list.dto";
import { BookDetailDto } from "../dtos/book-detail.dto";
import { CreateBookDto } from "../dtos/create-book.dto";
import { UpdateBookDto } from "../dtos/update-book.dto";

@Injectable({ providedIn: 'root' })
export class BookResource extends BaseResource {
    protected readonly baseUrl = `api/books`;

    getAll(filter?: SearchBooksFilterDto): Observable<BookListDto[]> {
        return this.http.get<BookListDto[]>(`${this.baseUrl}${this.composeQueryString(filter)}`);
    }

    getById(id: number): Observable<BookDetailDto> {
        return this.http.get<BookDetailDto>(`${this.baseUrl}/${id}`)
    }

    create(request: CreateBookDto): Observable<BookDetailDto> {
        return this.http.post<BookDetailDto>(this.baseUrl, request);
    }

    update(id: number, request: UpdateBookDto): Observable<BookDetailDto> {
        if (request.coverImage) {
            const formData = new FormData();

            Object.entries(request).forEach(([key, value]) => {
                if (key === 'coverImage' && value instanceof File) {
                    formData.append(key, value);
                } else if (key === 'genreIds' && Array.isArray(value)) {
                    value.forEach(id => formData.append('genreIds', String(id)));
                } else if (value !== null && value !== undefined) {
                    formData.append(key, String(value));
                }
            });

            return this.http.put<BookDetailDto>(`${this.baseUrl}/${id}`, formData);
        }
 
        return this.http.put<BookDetailDto>(`${this.baseUrl}/${id}`, request);
    }

    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${id}`);
    }
}