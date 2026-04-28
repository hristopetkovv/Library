import { Component, inject, signal } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { TranslatePipe, TranslateService } from "@ngx-translate/core";
import { NzButtonModule } from "ng-zorro-antd/button";
import { NzInputModule } from "ng-zorro-antd/input";
import { NzModalRef } from "ng-zorro-antd/modal";
import { ForgottenPasswordResource } from "../../../core/resources/user/forgotten-password.resource";
import { finalize } from "rxjs";
import { NzNotificationService } from "ng-zorro-antd/notification";
import { REGEX_PATTERNS } from "../../../core/constants/regex.constants";

@Component({
  selector: 'app-forgotten-password-modal',
  standalone: true,
  imports: [FormsModule, NzInputModule, NzButtonModule, TranslatePipe],
  templateUrl: './forgotten-password-modal.component.html',
  styleUrls: ['./forgotten-password-modal.component.css']
})
export class ForgottenPasswordModalComponent {
  private modalRef = inject(NzModalRef);
  private resource = inject(ForgottenPasswordResource);
  private notification = inject(NzNotificationService);
  private translate = inject(TranslateService);

  readonly emailPattern = REGEX_PATTERNS.EMAIL;

  email = '';
  isLoading = signal(false);

  onSubmit() {
    this.isLoading.set(true);
    this.resource.forgotPassword(this.email)
    .pipe(finalize(() => this.isLoading.set(false)))
    .subscribe({
          next: () => {
            this.notification.info(this.translate.instant("notification.info.forgottenPasswordTitle"), this.translate.instant("notification.info.forgottenPassword"));
            this.modalRef.destroy();
        }
    });
  }
}