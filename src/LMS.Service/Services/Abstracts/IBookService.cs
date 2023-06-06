using LMS.Entity.ViewModels.Book;

namespace LMS.Service.Services.Abstracts;

public interface IBookService
{
    Task<bool> CreateNewBookAsync(CreateBookViewModel viewModel);
    Task<bool> UpdateBookAsync(UpdateBookViewModel viewModel);
    Task<bool> DeleteBookWithIdAsync(string id);
    Task<IEnumerable<IndexBookViewModel>> GetAllBooksNonDeletedAsync();
    Task<UpdateBookViewModel?> GetBookByIdWithUpdateViewModelAsync(string id);
}