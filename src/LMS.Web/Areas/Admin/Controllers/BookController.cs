using LMS.Entity.ViewModels.Book;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class BookController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Update(string id)
    {
        return View();
    }

    public IActionResult Delete(string id)
    {
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Create()
    {
        return View();
    }

    public IActionResult Get()
    {
        var data = new List<object>();
        for (var i = 0; i < 100; i++)
        {
            var book = new IndexBookViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = $"Name{i}",
                Author = $"Author{i}",
                Category = $"Category{i}",
                Publisher = $"Publisher{i}"
            };
            data.Add(new
            {
                Name = $"Name{i}", Author = $"Author{i}", Category = $"Category{i}", Publisher = $"Publisher{i}",
                UpdateLink = Url.Action("Update", "Book", new { Area = "Admin", id = book.Id }),
                DeleteLink = Url.Action("Delete", "Book", new { Area = "Admin", id = book.Id })
            });
        }

        return Json(data);
    }
}