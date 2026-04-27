import { Component, inject, output, signal } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { NzButtonModule } from "ng-zorro-antd/button";
import { NzIconModule } from "ng-zorro-antd/icon";
import { NzInputModule } from "ng-zorro-antd/input";
import { LoginRequestDto } from "../../../core/dtos/auth/login-request.dto";
import { AuthService } from "../../../core/services/auth/auth.service";
import { NzMessageService } from "ng-zorro-antd/message";
import { finalize } from "rxjs";
import { TranslatePipe, TranslateService } from "@ngx-translate/core";

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, NzInputModule, NzButtonModule, NzIconModule, TranslatePipe],
  templateUrl: './login.component.html',
  styleUrls: ['../auth-modal.component.css']
})
export class LoginComponent {
  private authService = inject(AuthService);
  private message = inject(NzMessageService);
  private translate = inject(TranslateService);

  isLoading = signal(false);
  passwordVisible = false;
  public loginData: LoginRequestDto = { email: '', password: '' };

  onLoginSuccess = output<void>();
  onForgot = output<void>();

  login(): void {
    this.isLoading.set(true);

    this.authService.login(this.loginData)
    .pipe(
        finalize(() => this.isLoading.set(false))
      )
    .subscribe({
        next: () => {
            this.message.success(this.translate.instant("notification.success.login"));
            this.onLoginSuccess.emit();
        }
    });
  }

  forgotPassword() {
    this.onForgot.emit();
  }
}