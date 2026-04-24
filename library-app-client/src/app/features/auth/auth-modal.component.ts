import { Component, inject, signal } from "@angular/core";
import { LoginRequestDto } from "../../core/dtos/auth/login-request.dto";
import { RegisterRequestDto } from "../../core/dtos/auth/register-request.dto";
import { AuthService } from "../../core/services/auth/auth.service";
import { NzMessageService } from "ng-zorro-antd/message";
import { FormsModule } from "@angular/forms";
import { NzFormModule } from "ng-zorro-antd/form";
import { NzInputModule } from "ng-zorro-antd/input";
import { NzButtonModule } from "ng-zorro-antd/button";
import { NzModalModule, NzModalRef } from "ng-zorro-antd/modal";
import { NzTabsModule } from "ng-zorro-antd/tabs";
import { NzIconModule } from "ng-zorro-antd/icon";
import { CommonModule } from "@angular/common";

@Component({
  selector: 'app-auth-modal',
  standalone: true,
  imports: [CommonModule, FormsModule, NzFormModule, NzInputModule, NzButtonModule, NzModalModule, NzTabsModule, NzIconModule],
  templateUrl: './auth-modal.component.html',
  styleUrls: ['./auth-modal.component.css']
})
export class AuthModalComponent {
    private authService = inject(AuthService);
    private message = inject(NzMessageService);
    private modalRef = inject(NzModalRef);

    tabIndex = 0; 
    passwordVisible = false;
    isLoading = signal<boolean>(false);

    loginData: LoginRequestDto = { email: '', password: '' };
    registerData: RegisterRequestDto = {
    email: '', password: '', passwordAgain: '',
    firstName: '', lastName: '', phoneNumber: '', address: ''
    };

    login(): void {
    this.isLoading.set(true);

    this.authService.login(this.loginData).subscribe({
        next: () => {
            this.isLoading.set(false);
            this.message.success('Добре дошли отново!');
            this.handleCancel();
        },
        error: (err) => {
            this.isLoading.set(false);
            this.message.error(err.error?.detail || 'Невалиден имейл или парола');
      }
    });
  }

  register(): void {
     this.isLoading.set(true);

     this.authService.register(this.registerData).subscribe({
      next: () => {
        this.tabIndex = 0;
        this.loginData.email = this.registerData.email;
        this.isLoading.set(false);
        this.message.success('Регистрацията е успешна! Моля, влезте в профила си.');
        this.resetRegisterModel();
      },
      error: (err) => {
        this.isLoading.set(false);
        this.message.error(err.error?.detail || 'Грешка при регистрация');
      }
    });
  }

  handleCancel(): void {
    this.modalRef.destroy();
  }

  private resetRegisterModel(): void {
    this.registerData = {
      email: '', password: '', passwordAgain: '',
      firstName: '', lastName: '', phoneNumber: '', address: ''
    };
  }
}