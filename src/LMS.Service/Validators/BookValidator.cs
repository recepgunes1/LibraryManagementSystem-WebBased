using FluentValidation;
using LMS.Entity.Entities;

namespace LMS.Service.Validators;

public class BookValidator : AbstractValidator<Book>
{
    public BookValidator()
    {
        RuleFor(p => p.Isbn)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("ISBN cannot be empty.")
            .Length(13).WithMessage("ISBN length should be 13.")
            .Must(IsbnShouldBeNumber).WithMessage("ISBN should be a number.")
            .Must(IsbnCannotStartWithZero).WithMessage("ISBN cannot start with 0.");
    }

    private bool IsbnShouldBeNumber(string isbn)
    {
        return long.TryParse(isbn, out _);
    }

    private bool IsbnCannotStartWithZero(string isbn)
    {
        return isbn[0] != '0';
    }
}