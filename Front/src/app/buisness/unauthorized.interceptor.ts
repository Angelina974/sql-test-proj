import { HttpErrorResponse, HttpInterceptorFn } from "@angular/common/http";
import { catchError, throwError } from "rxjs";

export const unauthorizedInterceptor: HttpInterceptorFn = (request, next) => {
    return next(request).pipe(
        catchError((error) => {
            if (error instanceof HttpErrorResponse && error.status === 401) {
                window.location.assign(`https://localhost:5000/auth/signin?redirectUri=${window.location}`);
            }

            return throwError(() => error);
        }),
    );
}