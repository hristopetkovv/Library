import { Injectable } from "@angular/core";
import { BaseResource } from "../../../shared/resources/base.resource";
import { Observable } from "rxjs";
import { BorrowingBasicDto } from "../dtos/borrowing-basic.dto";

@Injectable({ providedIn: 'root' })
export class BorrowingResource extends BaseResource {
  protected readonly baseUrl = 'api/borrowings';

  getMy(): Observable<BorrowingBasicDto[]> {
    return this.http.get<BorrowingBasicDto[]>(`${this.baseUrl}/my`);
  }
}