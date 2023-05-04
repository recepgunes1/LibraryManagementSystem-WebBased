using Library_Management_System.Core.Entity;

namespace Library_Management_System.Entity.Entities;

public class Borrow : EntityBase
{
    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;

    public string BookId { get; set; } = null!;
    public Book Book { get; set; } = null!;

    public bool IsReturned { get; set; } = false;
    public bool IsApproved { get; set; } = false;

    public DateTime BorrowDateTime { get; set; } = DateTime.UtcNow;
    public DateTime ReturnDateTime { get; set; } = default!;
}