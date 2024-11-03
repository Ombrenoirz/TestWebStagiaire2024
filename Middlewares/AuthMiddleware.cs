public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        bool isAuthenticated = context.Session.TryGetValue("UserId", out _);

        var protectedPaths = new[] { "/", "/Home", "/Home/Index", "/Auth", "/Auth/Logout" };
        var authPaths = new[] { "/Auth", "/Auth/Login", "/Auth/Register" };

        if (!isAuthenticated && protectedPaths.Any(path => context.Request.Path == path))
        {
            context.Response.Redirect("/Auth/Login");
            return;
        }

        if (isAuthenticated && authPaths.Any(path => context.Request.Path == path))
        {
            context.Response.Redirect("/Home");
            return;
        }

        await _next(context);
    }
}
