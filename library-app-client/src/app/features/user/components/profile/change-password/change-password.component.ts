import { Component, inject, signal } from "@angular/core";
import { ReactiveFormsModule, FormBuilder, Validators, AbstractControl } from "@angular/forms";
import { TranslatePipe } from "@ngx-translate/core";
import { NzButtonModule } from "ng-zorro-antd/button";
import { NzFormModule } from "ng-zorro-antd/form";
import { NzIconModule } from "ng-zorro-antd/icon";
import { NzInputModule } from "ng-zorro-antd/input";
import { NzNotificationService } from "ng-zorro-antd/notification";
import { finalize } from "rxjs";
import { UserResource } from "../../../resources/user-resource";

@Component({
  selector: 'app-change-password',
  standalone: true,
  imports: [ReactiveFormsModule, TranslatePipe, NzFormModule, NzInputModule, NzButtonModule, NzIconModule],
  templateUrl: './change-password.component.html',
})
export class ChangePasswordComponent {
  private readonly fb = inject(FormBuilder);
  private readonly userResource = inject(UserResource);
  private readonly notification = inject(NzNotificationService);

  readonly isSubmitting = signal(false);
  readonly showCurrentPassword = signal(false);
  readonly showNewPassword = signal(false);
  readonly showConfirmPassword = signal(false);

  readonly form = this.fb.group({
    currentPassword: ['', Validators.required],
    newPassword: ['', [Validators.required, Validators.minLength(6)]],
    confirmPassword: ['', Validators.required],
  }, { validators: this.passwordMatchValidator });

  private passwordMatchValidator(form: AbstractControl) {
    const newPassword = form.get('newPassword')?.value;
    const confirmPassword = form.get('confirmPassword')?.value;
    return newPassword === confirmPassword ? null : { passwordMismatch: true };
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const val = this.form.getRawValue();
    this.isSubmitting.set(true);

    this.userResource.changePassword({
      currentPassword: val.currentPassword!,
      newPassword: val.newPassword!,
    }).pipe(finalize(() => this.isSubmitting.set(false)))
      .subscribe({
        next: () => {
          this.notification.success('', 'Паролата е сменена успешно');
          this.form.reset();
        }
      });
  }
}