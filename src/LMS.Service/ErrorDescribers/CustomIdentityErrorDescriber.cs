using Microsoft.AspNetCore.Identity;

namespace LMS.Service.ErrorDescribers;

public class CustomIdentityErrorDescriber : IdentityErrorDescriber
{
    public override IdentityError InvalidUserName(string? userName) { return new IdentityError { Code = nameof(InvalidUserName), Description = $"User name '{userName}' is invalid, can only contain digits." }; }
}