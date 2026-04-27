import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject, Injector } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { AuthService } from '../services/auth/auth.service';
import { TranslateService } from '@ngx-translate/core';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const notification = inject(NzNotificationService);
  const authService = inject(AuthService);
  const injector = inject(Injector);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      const translate = injector.get(TranslateService);
      let errorTitle = error.error?.title || translate.instant("error.title");
      let errorMessage = error.error?.detail || translate.instant("error.detail");

       switch (error.status) {
        case 400:
          const validationErrors = error.error?.errors;
            if (validationErrors) {
              errorMessage = Object.values(validationErrors).flat().join('<br>');
            }

            break;
          case 401:
            errorTitle = translate.instant("error.unauthorized");
            authService.logout();
            break;
          case 403:
            errorTitle = translate.instant("error.forbidden");
            break;
          case 404:
            errorTitle = translate.instant("error.notFound");
            errorMessage = translate.instant("error.notFoundMessage");
            break;
        }

      notification.error(errorTitle, errorMessage, { nzPlacement: 'topRight', nzDuration: 5000 });

      return throwError(() => error);
    })
  );
};