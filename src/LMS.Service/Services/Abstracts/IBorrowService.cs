using LMS.Entity.ViewModels.Borrow;

namespace LMS.Service.Services.Abstracts;

public interface IBorrowService
{
    Task<IEnumerable<IndexBorrowViewModel>> GetAllBorrowsAsync();
    Task BorrowAsync(string bookId);
    Task ReturnAsync(string id);
    Task ApproveAsync(string id);
}