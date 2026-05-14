import { ApplicationConfig, importProvidersFrom, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './core/interceptors/auth-interceptor';
import { bg_BG, provideNzI18n } from 'ng-zorro-antd/i18n';
import { registerLocaleData } from '@angular/common';
import en from '@angular/common/locales/en';
import { FormsModule } from '@angular/forms';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { errorInterceptor } from './core/interceptors/error-interceptor';
import { provideTranslateService } from "@ngx-translate/core";
import { provideTranslateHttpLoader } from "@ngx-translate/http-loader";
import { NzConfig, provideNzConfig } from 'ng-zorro-antd/core/config';
import { provideLottieOptions } from 'ngx-lottie';
import player from 'lottie-web';

registerLocaleData(en);

const ngZorroConfig: NzConfig = {
  notification: { nzPlacement: 'topRight', nzDuration: 2000 }
};

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideHttpClient(withInterceptors([authInterceptor, errorInterceptor])),
    provideNzI18n(bg_BG),
    importProvidersFrom(
      FormsModule, 
      NzModalModule),
    provideTranslateService({
      loader: provideTranslateHttpLoader({prefix:'/i18n/', suffix:'.json'}),
      fallbackLang: 'bg',
      lang: 'bg'
    }),
    provideNzConfig(ngZorroConfig),
    provideLottieOptions({
      player: () => player,
    })
  ],
};

