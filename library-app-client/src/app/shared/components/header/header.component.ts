import { Component, inject } from "@angular/core";
import { RouterLink } from "@angular/router";
import { NzButtonModule } from "ng-zorro-antd/button";
import { NzMenuModule } from "ng-zorro-antd/menu";
import { AuthService } from "../../../core/services/auth/auth.service";
import { UserRole } from "../../../core/enums/users/user-role.enum";
import { NzLayoutModule } from "ng-zorro-antd/layout";
import { NzModalService } from "ng-zorro-antd/modal";
import { AuthModalComponent } from "../../../features/auth/auth-modal.component";

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink, NzMenuModule, NzButtonModule, NzLayoutModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  public authService = inject(AuthService);
  private modalService = inject(NzModalService);

  userRole = UserRole;

  openAuthModal(): void {
    this.modalService.create({
      nzContent: AuthModalComponent,
      nzFooter: null,
      nzCentered: true,
      nzWidth: 450,
      nzClassName: 'auth-modal-wrapper',
      nzMaskClosable: true, 
      nzOnCancel: (instance) => instance.handleCancel()
    });
  }
}