namespace LMS.Entity.ViewModels.Borrow;

public class IndexBorrowViewModel
{
    public string Id { get; set; } = null!;
    public string User { get; set; } = null!;
    public string Book { get; set; } = null!;
    public string Status { get; set; } = null!;
    public bool ButtonVisibility { get; set; }
    public DateTime BorrowDateTime { get; set; } = default!;
    public DateTime ReturnDateTime { get; set; } = default!;
}