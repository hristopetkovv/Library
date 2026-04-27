import { Component, inject, viewChild } from "@angular/core";
import { NzModalModule, NzModalRef } from "ng-zorro-antd/modal";
import { NzTabsModule } from "ng-zorro-antd/tabs";
import { LoginComponent } from "./login/login.component";
import { RegisterComponent } from "./register/register.component";
import { TranslatePipe } from "@ngx-translate/core";

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
    // this.modalService.create({ nzContent: ForgotPasswordComponent });
  }

   closeModal(): void {
    this.modalRef.destroy();
  }
}