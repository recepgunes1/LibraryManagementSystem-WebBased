using Bogus;
using LMS.Data.Context;
using LMS.Entity.Entities;

namespace LMS.Data.Seeding;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        if (context.Authors.Any() && context.Categories.Any() && context.Publishers.Any() && context.Books.Any())
            return;

        context.Database.EnsureCreated();

        var fakeAuthor = new Faker<Author>()
            .RuleFor(a => a.FullName, f => f.Name.FullName())
            .RuleFor(a => a.BackStory, f => f.Lorem.Paragraph())
            .RuleFor(b => b.CreatedId, Guid.Empty.ToString());

        var fakeCategory = new Faker<Category>()
            .RuleFor(c => c.Name, f => f.Commerce.Department())
            .RuleFor(c => c.BackStory, f => f.Lorem.Paragraph())
            .RuleFor(b => b.CreatedId, Guid.Empty.ToString());

        var fakePublisher = new Faker<Publisher>()
            .RuleFor(p => p.Name, f => f.Company.CompanyName())
            .RuleFor(p => p.BackStory, f => f.Lorem.Paragraph())
            .RuleFor(b => b.CreatedId, Guid.Empty.ToString());

        var authors = fakeAuthor.Generate(10);
        var categories = fakeCategory.Generate(10);
        var publishers = fakePublisher.Generate(10);

        var coverImage = new Image()
        {
            FolderName = "books",
            FileName = "cover.jpg",
            CreatedId = Guid.Empty.ToString()
        };

        var fakeBook = new Faker<Book>()
            .RuleFor(b => b.Name, f => f.Commerce.ProductName())
            .RuleFor(b => b.Isbn, f => f.Commerce.Ean13())
            .RuleFor(b => b.Summary, f => f.Lorem.Paragraph(10))
            .RuleFor(b => b.Pages, f => f.Random.Number(50, 1000))
            .RuleFor(b => b.Amount, f => f.Random.Number(1, 100))
            .RuleFor(b => b.PublishedDateTime, f => f.Date.Past(20))
            .RuleFor(b => b.ImageId, coverImage.Id)
            .RuleFor(b => b.AuthorId, f => f.PickRandom(authors).Id)
            .RuleFor(b => b.CategoryId, f => f.PickRandom(categories).Id)
            .RuleFor(b => b.PublisherId, f => f.PickRandom(publishers).Id)
            .RuleFor(b => b.CreatedId, Guid.Empty.ToString())
            .RuleFor(b => b.Borrows, new List<Borrow>());

        var books = fakeBook.Generate(100);

        context.Authors.AddRange(authors);

        context.Categories.AddRange(categories);

        context.Publishers.AddRange(publishers);

        context.Images.Add(coverImage);

        context.Books.AddRange(books);

        context.SaveChanges();
    }
}