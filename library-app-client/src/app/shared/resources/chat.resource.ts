import { Injectable } from "@angular/core";
import { ChatResponseDto } from "../models/chat/chat-response.dto";
import { Observable } from "rxjs";
import { BaseResource } from "./base.resource";

@Injectable({ providedIn: 'root' })
export class ChatResource extends BaseResource {
  protected readonly baseUrl = 'api/chat';

  sendMessage(message: string, language: string): Observable<ChatResponseDto> {
    return this.http.post<ChatResponseDto>(this.baseUrl, { message, language });
  }
}