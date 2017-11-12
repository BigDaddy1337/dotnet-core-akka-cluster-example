using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebNode.Middlewares
{
    public class LogMiddleware
    {
        private readonly RequestDelegate next;

        public LogMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Console.WriteLine($"{DateTime.Now:T} Incoming request: {context.Request.QueryString}");
            await next.Invoke(context);
        }
    }
}
