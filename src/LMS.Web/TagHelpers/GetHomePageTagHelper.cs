using System.Security.Claims;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace LMS.Web.TagHelpers;

public class GetHomePageTagHelper : TagHelper
{
    private readonly HttpContext _httpContext;


    public GetHomePageTagHelper(IHttpContextAccessor httpContextAccessor, IHostEnvironment hostEnvironment)
    {
        _httpContext = httpContextAccessor.HttpContext!;
        Text = hostEnvironment.ApplicationName;
    }

    public string Text { get; set; }

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "a";

        if (_httpContext.User.Identity is not ClaimsIdentity claimsIdentity) return Task.CompletedTask;

        var role = string.Join("", claimsIdentity.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value));


        if (role == "lecturer" || role == "student")
            output.Attributes.SetAttribute("href", "/Home/Book/List");
        else if (role == "admin")
            output.Attributes.SetAttribute("href", "/Admin/Home/Index");
        else
            output.Attributes.SetAttribute("href", "/Account/Auth/Login");

        output.Content.SetContent(Text);
        return base.ProcessAsync(context, output);
    }
}