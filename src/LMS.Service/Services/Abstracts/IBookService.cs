using LMS.Entity.ViewModels.Book;
using LMS.Entity.ViewModels.Borrow;

namespace LMS.Service.Services.Abstracts;

public interface IBookService
{
    Task<bool> CreateNewBookAsync(CreateBookViewModel viewModel);
    Task<bool> UpdateBookAsync(UpdateBookViewModel viewModel);
    Task<bool> DeleteBookWithIdAsync(string id);
    Task<IEnumerable<IndexBookViewModel>> GetAllBooksNonDeletedAsync();
    Task<IEnumerable<IndexBookViewModel>> GetBorrowableBooksAsync();
    Task<IEnumerable<UserBorrowViewModel>> GetBorrowedBookHistoryAsync();
    Task<UpdateBookViewModel?> GetBookByIdWithUpdateViewModelAsync(string id);
    Task<DetailBookViewModel?> GetBookByIdWithDetailBookViewModelAsync(string id);
}