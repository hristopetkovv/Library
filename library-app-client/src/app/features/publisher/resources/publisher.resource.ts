import { Injectable } from "@angular/core";
import { BaseResource } from "../../../shared/resources/base.resource";
import { Observable } from "rxjs";
import { PublisherListDto } from "../dtos/publisher-list.dto";
import { PublisherDetailDto } from "../dtos/publisher-detail.dto";
import { PublisherDto } from "../dtos/publisher.dto";

@Injectable({ providedIn: 'root' })
export class PublisherResource extends BaseResource {
    protected readonly baseUrl = `api/publishers`;

    getAll(publisherName: string): Observable<PublisherListDto[]> {
        return this.http.get<PublisherListDto[]>(`${this.baseUrl}?publisherName=${publisherName}`);
    }

    getById(id: number): Observable<PublisherDetailDto> {
        return this.http.get<PublisherDetailDto>(`${this.baseUrl}/${id}`)
    }

    create(request: PublisherDto): Observable<void> {
        return this.http.post<void>(this.baseUrl, request);
    }

    update(id: number, request: PublisherDto): Observable<void> {
        return this.http.put<void>(`${this.baseUrl}/${id}`, request);
    }

    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${id}`);
    }
}