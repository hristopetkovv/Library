import { Component, input } from "@angular/core";
import { AnimationOptions, LottieComponent } from "ngx-lottie";

@Component({
  selector: 'app-loading',
  standalone: true,
  imports: [LottieComponent],
  template: `
    <div class="loading-wrapper">
      <ng-lottie
        [options]="lottieOptions"
        width="220px"
        height="220px"
      />
      @if (message()) {
        <p class="loading-text">{{ message() }}</p>
      }
    </div>
  `,
  styles: [`
    .loading-wrapper {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 40px 0;
    }
    .loading-text {
      font-size: 14px;
      color: #4a7c43;
    }
  `]
})
export class LoadingComponent {
  readonly message = input<string>('');

  readonly lottieOptions: AnimationOptions = {
    path: '/animations/bookshelf.json',
    loop: true,
    autoplay: true,
  };
}