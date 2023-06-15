using System.Security.Claims;
using LMS.Data.UnitOfWorks;
using LMS.Entity.Entities;
using Microsoft.AspNetCore.Authentication;

namespace LMS.Service.ClaimProviders;

public class BookBorrowingClaimProvider : IClaimsTransformation
{
    private readonly IUnitOfWork _unitOfWork;

    public BookBorrowingClaimProvider(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var identityUser = principal.Identity as ClaimsIdentity;

        if (identityUser == null)
        {
            return principal;
        }

        if (!identityUser.IsAuthenticated)
        {
            return principal;
        }

        var role = identityUser.FindFirst(ClaimTypes.Role);
        if (role?.Value == "admin")
        {
            return principal;
        }

        var userId = identityUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        
        var bookLimit = Convert.ToInt32(identityUser.FindFirst("MaxBooks")!.Value);
        
        var borrowedBooks =
            await _unitOfWork.GetRepository<Borrow>()
                .GetAllAsync(p => p.UserId == userId && !p.IsReturned && !p.IsApproved);

        var amountOfBorrowed = borrowedBooks.Count;
        
        var waitingToReturn = borrowedBooks.Any(p => (p.ReturnDateTime - p.BorrowDateTime).TotalDays <= 0);
        
        Console.WriteLine(amountOfBorrowed);
        Console.WriteLine(waitingToReturn);
        
        if (amountOfBorrowed >= bookLimit && !waitingToReturn)
        {
            var borrowable = new Claim("borrowable", "false");
            identityUser.AddClaim(borrowable);
        }

        return principal;
    }
}