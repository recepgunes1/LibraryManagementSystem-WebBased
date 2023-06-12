using LMS.Entity.ViewModels.Author;

namespace LMS.Service.Services.Abstracts;

public interface IAuthorService
{
    Task<bool> CreateNewAuthorAsync(CreateAuthorViewModel viewModel);
    Task<bool> UpdateAuthorAsync(UpdateAuthorViewModel viewModel);
    Task<bool> DeleteAuthorWithIdAsync(string id);
    Task<IEnumerable<IndexAuthorViewModel>> GetAllAuthorsNonDeletedAsync();
    Task<UpdateAuthorViewModel?> GetAuthorByIdWithUpdateViewModelAsync(string id);
    Task<Dictionary<string, string>> GetAuthorsWithKeyAndNameAsync();
    Task<(int NonDeleted, int Deleted)> CountAuthorsAsync();
}