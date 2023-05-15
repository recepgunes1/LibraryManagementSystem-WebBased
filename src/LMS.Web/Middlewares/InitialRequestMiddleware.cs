namespace LMS.Web.Middlewares;

public class InitialRequestMiddleware
{
    private readonly RequestDelegate _next;

    public InitialRequestMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        const string loginPath = "/Account/Auth/Login";

        if (!context.User.Identity!.IsAuthenticated &&
            !context.Request.Path.Equals(loginPath, StringComparison.OrdinalIgnoreCase))
            context.Response.Redirect(loginPath);
        else
            await _next(context);
    }
}

public static class InitialRequestMiddlewareExtension
{
    public static IApplicationBuilder UseInitialRequest(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<InitialRequestMiddleware>();
    }
}