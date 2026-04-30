import { Component, inject, signal } from "@angular/core";
import { RouterLink } from "@angular/router";
import { NzButtonModule } from "ng-zorro-antd/button";
import { NzMenuModule } from "ng-zorro-antd/menu";
import { AuthService } from "../../../features/auth/services/auth.service";
import { UserRole } from "../../../core/enums/users/user-role.enum";
import { NzLayoutModule } from "ng-zorro-antd/layout";
import { NzModalService } from "ng-zorro-antd/modal";
import { AuthModalComponent } from "../../../features/auth/components/auth-modal.component";
import { TranslatePipe, TranslateService } from "@ngx-translate/core";
import { bg_BG, en_US, NzI18nService } from "ng-zorro-antd/i18n";
import { NzDropdownModule } from "ng-zorro-antd/dropdown";
import { NzIconModule } from "ng-zorro-antd/icon";

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink, NzMenuModule, NzButtonModule, NzLayoutModule, TranslatePipe, NzDropdownModule, NzIconModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  public authService = inject(AuthService);
  private modalService = inject(NzModalService);
  private translate = inject(TranslateService);
  private nzI18n = inject(NzI18nService);
  public lang = signal<string>(localStorage.getItem('lang') || 'bg');

  userRole = UserRole;
  isLangOpen = false;
  isUserOpen = false;

  openAuthModal(): void {
    this.modalService.create({
      nzContent: AuthModalComponent,
      nzFooter: null,
      nzCentered: true,
      nzWidth: 450,
      nzClassName: 'auth-modal-wrapper',
      nzMaskClosable: true, 
      nzOnCancel: (instance) => instance.closeModal()
    });
  }

  switchLanguage(lang: string) {
    this.translate.use(lang);
    this.lang.set(lang);

    localStorage.setItem('lang', lang);

    this.updateZorroLocale(lang);
  }

  private updateZorroLocale(lang: string): void {
    if (lang === 'bg') {
      this.nzI18n.setLocale(bg_BG);
    } else if (lang === 'en') {
      this.nzI18n.setLocale(en_US);
    } else {
      this.nzI18n.setLocale(bg_BG);
    }
  }
}