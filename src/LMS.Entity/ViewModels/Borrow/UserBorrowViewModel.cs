namespace LMS.Entity.ViewModels.Borrow;

public class UserBorrowViewModel
{
    public string Id { get; set; } = null!;
    public string Book { get; set; } = null!;
    public string Status { get; set; } = null!;
    public bool ButtonVisibility { get; set; }
    public DateTime BorrowDateTime { get; set; }
    public DateTime ReturnDateTime { get; set; }
}