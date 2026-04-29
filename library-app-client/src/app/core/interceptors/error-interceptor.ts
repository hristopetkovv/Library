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
      
      let errorTitle = translate.instant("error.title");
      let errorMessage = translate.instant("error.message");

      if (error.error?.detail) {
        errorMessage = translate.instant(`validation.${error.error?.detail}`);
      }

       switch (error.status) {
        case 400:
            errorTitle = translate.instant("error.validation");
            
            const validationErrors = error.error?.errors;
            if (validationErrors) {
              errorMessage = Object.values(validationErrors)
                .flat()
                .map((key:any) => translate.instant(`validation.${key}`))
                .join('<br>');
            }

            if (errorMessage.startsWith("validation.")) {
              errorMessage = translate.instant("error.validationMessage");
            }

            break;
          case 401:
            errorTitle = translate.instant("error.unauthorized");

            if (errorMessage.startsWith("validation.")) {
              errorMessage = translate.instant("error.unauthorizedMessage");
            }

            authService.logout();
            
            break;
          case 403:
            errorTitle = translate.instant("error.forbidden");

            if (errorMessage.startsWith("validation.")) {
              errorMessage = translate.instant("error.forbiddenMessage");
            }

            break;
          case 404:
            errorTitle = translate.instant("error.notFound");

            if (errorMessage.startsWith("validation.")) {
              errorMessage = translate.instant("error.notFoundMessage");
            }

            break;
        }

      if (errorMessage.startsWith("validation.")) {
        errorMessage = translate.instant("error.detail");
      }

      notification.error(errorTitle, errorMessage, { nzDuration: 5000 });

      return throwError(() => error);
    })
  );
};