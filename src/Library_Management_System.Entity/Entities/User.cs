using Microsoft.AspNetCore.Identity;

namespace Library_Management_System.Entity.Entities;

public class User : IdentityUser
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public ICollection<Borrow> Borrows { get; set; } = null!;
}