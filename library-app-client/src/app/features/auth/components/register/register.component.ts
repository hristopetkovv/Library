import { Component, inject, output, signal } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { NzButtonModule } from "ng-zorro-antd/button";
import { NzInputModule } from "ng-zorro-antd/input";
import { AuthService } from "../../services/auth.service";
import { RegisterRequestDto } from "../../dtos/register-request.dto";
import { finalize } from "rxjs";
import { TranslatePipe, TranslateService } from "@ngx-translate/core";
import { REGEX_PATTERNS } from "../../../../core/constants/regex.constants";
import { NzNotificationService } from "ng-zorro-antd/notification";

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, NzInputModule, NzButtonModule, TranslatePipe],
  templateUrl: './register.component.html',
  styleUrls: ['../auth-modal.component.css']
})
export class RegisterComponent {
  private authService = inject(AuthService);
  private notification = inject(NzNotificationService);
  private translate = inject(TranslateService);
  
  isLoading = signal(false);
  registerData: RegisterRequestDto = {
    email: '', password: '', passwordAgain: '',
    firstName: '', lastName: '', phoneNumber: '', address: ''
  };

  readonly emailPattern = REGEX_PATTERNS.EMAIL;
  readonly passwordPattern = REGEX_PATTERNS.PASSWORD;

  onRegisterSuccess = output<string>();

  register(): void {
     this.isLoading.set(true);

     this.authService.register(this.registerData)
     .pipe(
             finalize(() => this.isLoading.set(false))
           )
     .subscribe({
      next: () => {
        this.onRegisterSuccess.emit(this.registerData.email);
        this.resetRegisterModel();
        this.notification.success(this.translate.instant("notification.success.register"), "");
      }
    });
  }

  private resetRegisterModel(): void {
    this.registerData = {
      email: '', password: '', passwordAgain: '',
      firstName: '', lastName: '', phoneNumber: '', address: ''
    };
  }
}