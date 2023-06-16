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

        var usedFullNames = new HashSet<string>();
        var fakeAuthor = new Faker<Author>()
            .CustomInstantiator(f =>
            {
                string fullName;
                do
                {
                    fullName = f.Name.FullName();
                } while (usedFullNames.Contains(fullName));

                usedFullNames.Add(fullName);

                return new Author
                {
                    FullName = fullName,
                    BackStory = f.Lorem.Paragraph(),
                    CreatedId = Guid.Empty.ToString()
                };
            });

        var usedDepartmentNames = new HashSet<string>();
        var fakeCategory = new Faker<Category>()
            .CustomInstantiator(f =>
            {
                string departmentName;
                do
                {
                    departmentName = f.Commerce.Department();
                } while (usedDepartmentNames.Contains(departmentName));

                usedDepartmentNames.Add(departmentName);

                return new Category
                {
                    Name = departmentName,
                    BackStory = f.Lorem.Paragraph(),
                    CreatedId = Guid.Empty.ToString()
                };
            });

        var usedCompanyNames = new HashSet<string>();
        var fakePublisher = new Faker<Publisher>()
            .CustomInstantiator(f =>
            {
                string companyName;
                do
                {
                    companyName = f.Company.CompanyName();
                } while (usedCompanyNames.Contains(companyName));

                usedCompanyNames.Add(companyName);

                return new Publisher
                {
                    Name = companyName,
                    BackStory = f.Lorem.Paragraph(),
                    CreatedId = Guid.Empty.ToString()
                };
            });

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