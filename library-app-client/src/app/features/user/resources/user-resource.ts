import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { BaseResource } from "../../../shared/resources/base.resource";
import { SearchUsersFilterDto } from "../dtos/search-users-filter.dto";
import { UserListDto } from "../dtos/user-list.dto";
import { UserDetailDto } from "../dtos/user-detail.dto";
import { UpdateUserDto } from "../dtos/update-user.dto";
import { ChangeUserRoleDto } from "../dtos/change-user-role.dto";

@Injectable({ providedIn: 'root' })
export class UserResource  extends BaseResource{
    protected readonly baseUrl = 'api/users';

    getAll(filter?: SearchUsersFilterDto): Observable<UserListDto[]> {
        return this.http.get<UserListDto[]>(`${this.baseUrl}${this.composeQueryString(filter)}`);
    }
    
    getById(id: number): Observable<UserDetailDto> {
        return this.http.get<UserDetailDto>(`${this.baseUrl}/${id}`)
    }

    update(request: UpdateUserDto): Observable<UserDetailDto> {
        return this.http.put<UserDetailDto>(`${this.baseUrl}`, request);
    }

    changeRole(id: number, request: ChangeUserRoleDto): Observable<void> {
        return this.http.put<void>(`${this.baseUrl}/${id}/role`, request);
    }

    activate(id: number): Observable<void> {
        return this.http.put<void>(`${this.baseUrl}/${id}/activate`, {});
    }

    deactivate(id: number): Observable<void> {
        return this.http.put<void>(`${this.baseUrl}/${id}/deactivate`, {});
    }

    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${id}`);
    }
}