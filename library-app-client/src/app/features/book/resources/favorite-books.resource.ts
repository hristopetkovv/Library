import { Injectable } from "@angular/core";
import { BaseResource } from "../../../shared/resources/base.resource";
import { Observable } from "rxjs";
import { UserFavoriteBookDto } from "../../user/dtos/user-favorite-book.dto";

@Injectable({ providedIn: 'root' })
export class FavoriteBooksResource extends BaseResource {
    protected readonly baseUrl = `api/favoritebooks`;

    getMine(): Observable<UserFavoriteBookDto[]> {
        return this.http.get<UserFavoriteBookDto[]>(`${this.baseUrl}/mine`);
    }

    add(bookId: number): Observable<void> {
        return this.http.post<void>(`${this.baseUrl}/${bookId}`, {});
    }

    remove(bookId: number): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${bookId}`);
    }
}