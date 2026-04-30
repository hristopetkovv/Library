import { Component, inject, output, signal } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { NzButtonModule } from "ng-zorro-antd/button";
import { NzIconModule } from "ng-zorro-antd/icon";
import { NzInputModule } from "ng-zorro-antd/input";
import { finalize } from "rxjs";
import { TranslatePipe, TranslateService } from "@ngx-translate/core";
import { NzNotificationService } from "ng-zorro-antd/notification";
import { AuthService } from "../../services/auth.service";
import { LoginRequestDto } from "../../dtos/login-request.dto";
import { REGEX_PATTERNS } from "../../../../core/constants/regex.constants";

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, NzInputModule, NzButtonModule, NzIconModule, TranslatePipe],
  templateUrl: './login.component.html',
  styleUrls: ['../auth-modal.component.css']
})
export class LoginComponent {
  private authService = inject(AuthService);
  private notification = inject(NzNotificationService);
  private translate = inject(TranslateService);

  passwordVisible = false;
  public loginData: LoginRequestDto = { email: '', password: '' };

  readonly emailPattern = REGEX_PATTERNS.EMAIL;
  readonly passwordPattern = REGEX_PATTERNS.PASSWORD;

  isLoading = signal(false);

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
            this.notification.success(this.translate.instant("notification.success.login"), "");
            this.onLoginSuccess.emit();
        }
    });
  }

  forgotPassword() {
    this.onForgot.emit();
  }
}