import { Component, computed, inject, signal } from "@angular/core";
import { TranslatePipe } from "@ngx-translate/core";
import { NzIconModule } from "ng-zorro-antd/icon";
import { AuthService } from "../../../auth/services/auth.service";
import { Router } from "@angular/router";
import { UserDetailComponent } from "./user-detail/user-detail.component";
import { UserBorrowingsComponent } from "./user-borrowings/user-borrowings.component";

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [TranslatePipe, NzIconModule, UserDetailComponent, UserBorrowingsComponent],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css',
})
export class ProfileComponent {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  readonly activeTab = signal<'details' | 'borrowings'>('details');
  readonly currentUser = this.authService.currentUser;

  readonly initials = computed(() => {
    const user = this.currentUser();
    if (!user) return '';
    return `${user.firstName[0]}${user.lastName[0]}`.toUpperCase();
  });

  constructor() {
    if (!this.authService.currentUser()) {
      this.router.navigate(['/books']);
    }
  }
}