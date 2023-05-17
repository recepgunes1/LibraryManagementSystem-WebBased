using LMS.Entity.ViewModels.Category;

namespace LMS.Service.Services.Abstracts;

public interface ICategoryService
{
    Task<bool> CreateNewCategoryAsync(CreateCategoryViewModel viewModel);
    Task<bool> UpdateCategoryAsync(UpdateCategoryViewModel viewModel);
    Task<bool> DeleteCategoryWithIdAsync(string id);
    Task<IEnumerable<IndexCategoryViewModel>> GetAllCategoriesNonDeletedAsync();
    Task<UpdateCategoryViewModel?> GetCategoryByIdWithUpdateViewModelAsync(string id);
    Task<Dictionary<string, string>> GetParentCategoriesAsync();
    Task<Dictionary<string, string>> GetParentCategoriesAsync(string name);
    Task<Dictionary<string, string>> GetCategoriesWithKeyAndNameAsync();
}
