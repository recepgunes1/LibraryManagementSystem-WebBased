#nullable disable
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace LMS.Web.TagHelpers;

[HtmlTargetElement(Attributes = "is-active-route")]
public class ActiveRouteTagHelper : TagHelper
{
    [HtmlAttributeName("asp-controller")] public string Controller { get; set; }

    [HtmlAttributeName("asp-action")] public string Action { get; set; }

    [ViewContext] [HtmlAttributeNotBound] public ViewContext ViewContext { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        base.Process(context, output);

        var currentController = (string)ViewContext.RouteData.Values["Controller"]!;
        var currentAction = (string)ViewContext.RouteData.Values["Action"]!;

        if (Controller == currentController && Action == currentAction)
        {
            var classAttr = output.Attributes.FirstOrDefault(a => a.Name == "class");
            if (classAttr == null)
            {
                classAttr = new TagHelperAttribute("class", "text-decoration-underline");
                output.Attributes.Add(classAttr);
            }
            else
            {
                output.Attributes.SetAttribute("class",
                    classAttr.Value == null
                        ? "text-decoration-underline"
                        : classAttr.Value + " text-decoration-underline");
            }
        }
    }
}