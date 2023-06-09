using System.Security.Claims;

namespace LMS.Web.Middlewares;

public class InitialRequestMiddleware
{
    private readonly RequestDelegate _next;

    public InitialRequestMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path != "/")
        {
            _next(context);
            return Task.CompletedTask;
        }

        if (context.User.Identity == null)
        {
            _next(context);
            return Task.CompletedTask;
        }

        const string adminPage = "/Admin/Home/Index";
        const string memberPage = "/Home/Book/List";

        var authenticationPaths = new List<string>
        {
            "/Account/Auth/Login",
            "/Account/Auth/Register",
            "/Account/Auth/ResetPassword"
        };

        if (!context.User.Identity.IsAuthenticated)
            if (!authenticationPaths.Contains(context.Request.Path.Value!))
            {
                Console.WriteLine("worked here in if authenticationPaths.Contains");
                context.Response.Redirect("/Account/Auth/Login");
                return Task.CompletedTask;
            }

        if (context.User.Identity is not ClaimsIdentity claimsIdentity) return Task.CompletedTask;

        var role = string.Join("", claimsIdentity.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value));

        switch (role)
        {
            case "admin":
                context.Response.Redirect(adminPage);
                return Task.CompletedTask;
            case "lecturer" or "student":
                context.Response.Redirect(memberPage);
                return Task.CompletedTask;
            default:
                _next(context);
                return Task.CompletedTask;
        }
    }
}

public static class InitialRequestMiddlewareExtension
{
    public static IApplicationBuilder UseInitialRequest(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<InitialRequestMiddleware>();
    }
}