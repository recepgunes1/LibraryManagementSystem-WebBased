using LMS.Entity.Entities;
using Microsoft.AspNetCore.Identity;

namespace LMS.Service.Validators;

public class UserValidator : IUserValidator<User>
{
    public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
    {
        var errors = new List<IdentityError>();
        if (user.UserName!.StartsWith("0"))
        {
            errors.Add(new IdentityError { Code = "UserNameStartsWithZero", Description = "UserName can not start with zero(0)" });
        }
        if (user.UserName!.Length != 9)
        {
            errors.Add(new IdentityError { Code = "UserNameLength", Description = "UserName length should be 9." });
        }
        return Task.FromResult(errors.Any() ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success);
    }
}