using LMS.Entity.ViewModels.Category;

namespace LMS.Service.Services.Abstracts;

public interface ICategoryService
{
    Task<bool> CreateNewCategoryAsync(CreateCategoryViewModel viewModel);
    Task<bool> UpdateCategoryAsync(UpdateCategoryViewModel viewModel);
    Task<bool> DeleteCategoryWithIdAsync(string id);
    Task<IEnumerable<IndexCategoryViewModel>> GetAllCategoriesNonDeletedAsync();
    Task<UpdateCategoryViewModel?> GetCategoryByIdWithUpdateViewModelAsync(string id);
    Task<Dictionary<string, string>> GetCategoriesWithKeyAndNameAsync();
    Task<(int NonDeleted, int Deleted)> CountCategoriesAsync();
}