import { Injectable } from "@angular/core";
import { BaseResource } from "../../../shared/resources/base.resource";
import { Observable } from "rxjs";
import { BorrowingBasicDto } from "../dtos/borrowing-basic.dto";
import { SearchBorrowingsFilterDto } from "../dtos/search-borrowing-filter.dto";
import { BorrowingDetailDto } from "../dtos/borrowing-detail.dto";
import { CreateBorrowingDto } from "../dtos/create-borrowing.dto";
import { BorrowingStatus } from "../enums/borrowings-status.enum";

@Injectable({ providedIn: 'root' })
export class BorrowingResource extends BaseResource {
  protected readonly baseUrl = 'api/borrowings';

  getMy(): Observable<BorrowingBasicDto[]> {
    return this.http.get<BorrowingBasicDto[]>(`${this.baseUrl}/my`);
  }

  getByUserId(userId: number, status: BorrowingStatus): Observable<BorrowingBasicDto[]> {
    return this.http.get<BorrowingBasicDto[]>(`${this.baseUrl}/user/${userId}?status=${status}`);
  }

  getAll(filter: SearchBorrowingsFilterDto): Observable<BorrowingDetailDto[]> {
    return this.http.get<BorrowingDetailDto[]>(`${this.baseUrl}/all${this.composeQueryString(filter)}`);
  }

  borrow(dto: CreateBorrowingDto): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/borrow`, dto);
  }

  returnBook(borrowingId: number): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${borrowingId}/return`, {});
  }
}