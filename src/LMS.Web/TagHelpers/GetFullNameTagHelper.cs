using LMS.Service.Services.Abstracts;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace LMS.Web.TagHelpers;

public class GetFullNameTagHelper : TagHelper
{
    private readonly IUserService _userService;

    public GetFullNameTagHelper(IUserService userService)
    {
        _userService = userService;
    }


    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.Content.SetHtmlContent(_userService.GetCurrentUserFullNameAsync().Result);
        return base.ProcessAsync(context, output);
    }
}