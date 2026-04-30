import { Component, inject, viewChild } from "@angular/core";
import { NzModalModule, NzModalRef, NzModalService } from "ng-zorro-antd/modal";
import { NzTabsModule } from "ng-zorro-antd/tabs";
import { LoginComponent } from "./login/login.component";
import { RegisterComponent } from "./register/register.component";
import { TranslatePipe, TranslateService } from "@ngx-translate/core";
import { ForgottenPasswordModalComponent } from "../../user/components/forgotten-password/forgotten-password-modal.component";

@Component({
  selector: 'app-auth-modal',
  standalone: true,
  imports: [NzModalModule, NzTabsModule, LoginComponent, RegisterComponent, TranslatePipe],
  templateUrl: './auth-modal.component.html',
  styleUrls: ['./auth-modal.component.css']
})
export class AuthModalComponent {
  private loginComp = viewChild(LoginComponent);

  private modalRef = inject(NzModalRef);
  private modalService = inject(NzModalService);
  private translate = inject(TranslateService);

  tabIndex = 0;

  handleRegisterSuccess(email: string) {
    setTimeout(() => {
      const login = this.loginComp();
      if (login) {
        login.loginData.email = email;
      }
    }, 100);
    
    this.tabIndex = 0; 
  }

  handleForgot() {
    this.closeModal();
    
    this.modalService.create({
      nzTitle: this.translate.instant('user.forgottenPassword.title'),
      nzClassName: 'center-modal-title',
      nzContent: ForgottenPasswordModalComponent,
      nzFooter: null,
      nzCentered: true,
      nzWidth: 400
    });
  }

   closeModal(): void {
    this.modalRef.destroy();
  }
}