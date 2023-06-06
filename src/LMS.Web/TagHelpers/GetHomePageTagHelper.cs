using LMS.Service.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace LMS.Web.TagHelpers;

public class GetHomePageTagHelper : TagHelper
{
    private readonly IUserService _userService;

    public string Text { get; set; }


    public GetHomePageTagHelper(IUserService userService, IHostEnvironment hostEnvironment)
    {
        _userService = userService;
        Text = hostEnvironment.ApplicationName;
    }

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "a";

        var role = _userService.GetCurrentUserRole().GetAwaiter().GetResult();

        if (role == "lecturer" || role == "student")
        {
            output.Attributes.SetAttribute("href", "/Home/Book/List");
        }
        else if (role == "admin")
        {
            output.Attributes.SetAttribute("href", "/Admin/Home/Index");
        }
        else
        {
            output.Attributes.SetAttribute("href", "/Account/Auth/Login");
        }

        output.Content.SetContent(Text);
        return base.ProcessAsync(context, output);
    }
}