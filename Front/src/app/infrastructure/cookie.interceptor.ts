export const cookieInterceptor: HttpInterceptorFn = (request, next) => {
    if (request.url.starWith('https://localhost:5000/')) {
        const newRequest = request.clone({
            withCredentials: true
        });
        return next(newRequest);
    }

    return next(request);
}