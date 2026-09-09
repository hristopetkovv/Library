import { Component, inject, signal } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { TranslatePipe, TranslateService } from "@ngx-translate/core";
import { NzButtonModule } from "ng-zorro-antd/button";
import { NzIconModule } from "ng-zorro-antd/icon";
import { finalize } from "rxjs";
import { AuthService } from "../../../features/auth/services/auth.service";
import { ChatResource } from "../../resources/chat.resource";
import { ChatMessage } from "../../models/chat/chat-message.dto";

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [FormsModule, TranslatePipe, NzButtonModule, NzIconModule],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.css',
})
export class ChatComponent {
  private readonly chatResource = inject(ChatResource);
  private readonly translateService = inject(TranslateService);
  private readonly authService = inject(AuthService);

  readonly isOpen = signal(false);
  readonly isLoading = signal(false);
  readonly inputMessage = signal('');

  readonly messages = signal<ChatMessage[]>([]);

  readonly isAuthenticated = this.authService.isAuthenticated;

  sendMessage(): void {
    const message = this.inputMessage().trim();
    if (!message || this.isLoading()) return;

    this.messages.update(msgs => [...msgs, { text: message, isUser: true }]);
    this.inputMessage.set('');
    this.isLoading.set(true);

    this.chatResource.sendMessage(message, this.translateService.getCurrentLang())
      .pipe(finalize(() => this.isLoading.set(false)))
      .subscribe({
        next: (response) => {
          this.messages.update(msgs => [...msgs, { text: response.response, isUser: false }]);
        },
        error: () => {
          this.messages.update(msgs => [...msgs, {
            text: this.translateService.instant('chat.error'),
            isUser: false
          }]);
        }
      });
  }

  onKeyDown(event: KeyboardEvent): void {
    if (event.key === 'Enter' && !event.shiftKey) {
      event.preventDefault();
      this.sendMessage();
    }
  }

  toggleChat(): void {
    this.isOpen.set(!this.isOpen());
  }
}