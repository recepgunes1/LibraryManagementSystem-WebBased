using LMS.Entity.ViewModels.Author;

namespace LMS.Service.Services.Abstracts;

public interface IAuthorService
{
    Task<bool> CreateNewAuthorAsync(CreateAuthorViewModel viewModel);
    Task<bool> UpdateAuthorAsync(UpdateAuthorViewModel viewModel);
    Task<bool> DeleteAuthorWithIdAsync(string id);
    Task<IEnumerable<IndexAuthorViewModel>> GetAllAuthorsAsync();
    Task<UpdateAuthorViewModel?> GetAuthorByIdWithUpdateViewModelAsync(string id);
}