using System.Security.Claims;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace LMS.Web.TagHelpers;

public class GetFullNameTagHelper : TagHelper
{
    private readonly HttpContext _httpContext;


    public GetFullNameTagHelper(IHttpContextAccessor httpContextAccessor)
    {
        _httpContext = httpContextAccessor.HttpContext!;
    }


    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (_httpContext.User.Identity is not ClaimsIdentity claimsIdentity) return Task.CompletedTask;

        var name = string.Join("", claimsIdentity.Claims
            .Where(c => c.Type == ClaimTypes.Email)
            .Select(c => c.Value));

        output.Content.SetHtmlContent(name);
        return base.ProcessAsync(context, output);
    }
}