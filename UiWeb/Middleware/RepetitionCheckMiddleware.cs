using UiWeb.Services;
using System.Security.Claims;

namespace UiWeb.Middleware
{
    public class RepetitionCheckMiddleware
    {
        private readonly RequestDelegate _next;

        public RepetitionCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, RepetitionCheckerService repetitionChecker)
        {
            if (context.Request.Method == "GET" && context.User.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out var userId))
                {
                    var lastCheckKey = $"LastRepetitionCheck_{userId}";
                    var lastCheck = context.Session.GetString(lastCheckKey);

                    if (string.IsNullOrEmpty(lastCheck) || DateTime.Parse(lastCheck).Date < DateTime.Today)
                    {
                        await repetitionChecker.CheckAndGenerateRepetitions(userId);
                        context.Session.SetString(lastCheckKey, DateTime.Now.ToString());
                    }
                }
            }

            await _next(context);
        }
    }
}