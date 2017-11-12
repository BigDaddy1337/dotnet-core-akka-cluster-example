using Microsoft.AspNetCore.Builder;

namespace WebNode.Middlewares
{
    public static class MiddlewareExtension
    {
        public static IApplicationBuilder UseLog(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogMiddleware>();
        }
    }
}
