export const unauthorizedInterceptor: HttpInterceptorFn = (request, next) => {
    return next(request).pipe(
        catchError((error) => {
            if (error insteadof HttpErrorResponse && error.status === 401) {
        window.location.assign('https://localhost:5000/auth/signin');
    }

        return throwError(() => error);
}),
);
}