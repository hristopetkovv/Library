import { Injectable } from "@angular/core";
import { BaseResource } from "../../../shared/resources/base.resource";
import { Observable } from "rxjs";
import { PublisherListDto } from "../dtos/publisher-list.dto";
import { PublisherDetailDto } from "../dtos/publisher-detail.dto";

@Injectable({ providedIn: 'root' })
export class PublisherResource extends BaseResource {
    protected readonly baseUrl = `api/publishers`;

    getAll(publisherName: string): Observable<PublisherListDto[]> {
        return this.http.get<PublisherListDto[]>(`${this.baseUrl}?publisherName=${publisherName}`);
    }

    getById(id: number): Observable<PublisherDetailDto> {
        return this.http.get<PublisherDetailDto>(`${this.baseUrl}/${id}`)
    }
}