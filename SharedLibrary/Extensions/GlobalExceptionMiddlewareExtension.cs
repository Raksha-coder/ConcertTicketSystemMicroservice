using Microsoft.AspNetCore.Builder;
using SharedLibrary.Middleware;


namespace SharedLibrary.Extensions
{
    public static class GlobalExceptionMiddlewareExtension
    {
        public static IApplicationBuilder UseGlobalException(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        }
    }
}
