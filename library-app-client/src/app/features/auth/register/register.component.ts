import { Component, inject, output, signal } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { NzButtonModule } from "ng-zorro-antd/button";
import { NzInputModule } from "ng-zorro-antd/input";
import { AuthService } from "../../../core/services/auth/auth.service";
import { NzMessageService } from "ng-zorro-antd/message";
import { RegisterRequestDto } from "../../../core/dtos/auth/register-request.dto";
import { finalize } from "rxjs";
import { TranslatePipe, TranslateService } from "@ngx-translate/core";

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, NzInputModule, NzButtonModule, TranslatePipe],
  templateUrl: './register.component.html',
  styleUrls: ['../auth-modal.component.css']
})
export class RegisterComponent {
  private authService = inject(AuthService);
  private message = inject(NzMessageService);
  private translate = inject(TranslateService);
  
  isLoading = signal(false);
  registerData: RegisterRequestDto = {
    email: '', password: '', passwordAgain: '',
    firstName: '', lastName: '', phoneNumber: '', address: ''
  };

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
        this.message.success(this.translate.instant("notification.success.register"));
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