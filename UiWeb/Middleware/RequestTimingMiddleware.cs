using System.Diagnostics;

namespace UiWeb.Middleware
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;
        public RequestTimingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context) 
        {
            Console.WriteLine("Strat timing....");
            var time = Stopwatch.StartNew();
            await _next(context);
            time.Stop();

            Console.WriteLine($"{context.Request.Path} timing : {time.ElapsedMilliseconds} ms");
        }
    }
}
