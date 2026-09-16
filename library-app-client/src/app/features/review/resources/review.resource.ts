import { Injectable } from "@angular/core";
import { BaseResource } from "../../../shared/resources/base.resource";
import { Observable } from "rxjs";
import { ReviewDto } from "../dtos/review.dto";
import { CreateReviewDto } from "../dtos/create-review.dto";

@Injectable({ providedIn: 'root' })
export class ReviewResource extends BaseResource {
    protected readonly baseUrl = `api/reviews`;

    getByBookId(bookId: number): Observable<ReviewDto[]> {
        return this.http.get<ReviewDto[]>(`${this.baseUrl}/book/${bookId}`);
    }

    create(dto: CreateReviewDto): Observable<void> {
        return this.http.post<void>(this.baseUrl, dto);
    }

    delete(reviewId: number): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${reviewId}`);
    }
}