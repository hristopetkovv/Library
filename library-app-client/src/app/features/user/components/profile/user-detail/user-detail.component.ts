import { Component, inject, OnInit, signal } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { TranslatePipe } from "@ngx-translate/core";
import { NzButtonModule } from "ng-zorro-antd/button";
import { NzFormModule } from "ng-zorro-antd/form";
import { NzGridModule } from "ng-zorro-antd/grid";
import { NzInputModule } from "ng-zorro-antd/input";
import { finalize } from "rxjs";
import { LoadingComponent } from "../../../../../shared/components/loading/loading.component";
import { AuthService } from "../../../../auth/services/auth.service";
import { UserResource } from "../../../resources/user-resource";
import { NzNotificationService } from "ng-zorro-antd/notification";
import { REGEX_PATTERNS } from "../../../../../core/constants/regex.constants";

@Component({
  selector: 'app-user-detail',
  standalone: true,
  imports: [ReactiveFormsModule, TranslatePipe, NzFormModule, NzInputModule, NzButtonModule, NzGridModule, LoadingComponent],
  templateUrl: './user-detail.component.html',
})
export class UserDetailComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly userResource = inject(UserResource);
  private readonly notification = inject(NzNotificationService);

  readonly isLoading = signal(false);
  readonly isSubmitting = signal(false);

  readonly form = this.fb.group({
    email: ['', [Validators.required, Validators.pattern(REGEX_PATTERNS.EMAIL)]],
    firstName: ['', [Validators.required, Validators.maxLength(50)]],
    lastName: ['', [Validators.required, Validators.maxLength(50)]],
    address: ['', [Validators.maxLength(500)]],
    phoneNumber: ['', [Validators.maxLength(20)]],
  });

  ngOnInit(): void {
    const currentUser = this.authService.currentUser();
    if (!currentUser) return;

    this.isLoading.set(true);
    this.userResource.getById(currentUser.id)
      .pipe(finalize(() => this.isLoading.set(false)))
      .subscribe({
        next: (user) => this.form.patchValue({
          email: user.email,
          firstName: user.firstName,
          lastName: user.lastName,
          address: user.address ?? '',
          phoneNumber: user.phoneNumber ?? '',
        }),
      });
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    const val = this.form.getRawValue();

    this.userResource.update({
      email: val.email!,
      firstName: val.firstName!,
      lastName: val.lastName!,
      address: val.address ?? '',
      phoneNumber: val.phoneNumber ?? '',
    }).pipe(finalize(() => this.isSubmitting.set(false)))
      .subscribe({
        next: () => {
          this.notification.success('', 'Профилът е обновен успешно');
          const user = this.authService.currentUser();
          if (user) {
            this.authService.currentUser.set({
              ...user,
              email: val.email!,
              firstName: val.firstName!,
              lastName: val.lastName!,
            });
          }
        },
      });
  }
}