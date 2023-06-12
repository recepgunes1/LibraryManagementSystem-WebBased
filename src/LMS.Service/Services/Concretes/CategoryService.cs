using AutoMapper;
using LMS.Data.UnitOfWorks;
using LMS.Entity.Entities;
using LMS.Entity.ViewModels.Category;
using LMS.Service.Services.Abstracts;

namespace LMS.Service.Services.Concretes;

public class CategoryService : ICategoryService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;

    public CategoryService(IMapper mapper, IUnitOfWork unitOfWork, IUserService userService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userService = userService;
    }

    public async Task<bool> CreateNewCategoryAsync(CreateCategoryViewModel viewModel)
    {
        if (await _unitOfWork.GetRepository<Category>().AnyAsync(p => p.Name == viewModel.Name)) return false;
        var mapped = _mapper.Map<Category>(viewModel);
        mapped.CreatedId = await _userService.GetCurrentUserId();
        await _unitOfWork.GetRepository<Category>().AddAsync(mapped);
        await _unitOfWork.SaveAsync();
        return true;
    }

    public async Task<bool> UpdateCategoryAsync(UpdateCategoryViewModel viewModel)
    {
        if (await _unitOfWork.GetRepository<Category>()
                .AnyAsync(p => p.Name == viewModel.Name && p.Id != viewModel.Id))
            return false;

        var flag = await _unitOfWork.GetRepository<Category>().AnyAsync(p => p.Id == viewModel.Id);

        if (!flag) return false;

        var category = await _unitOfWork.GetRepository<Category>().GetAsync(p => p.Id == viewModel.Id);
        _mapper.Map(viewModel, category);
        category.UpdatedId = await _userService.GetCurrentUserId();
        category.UpdateDateTime = DateTime.Now;
        await _unitOfWork.SaveAsync();
        return true;
    }

    public async Task<bool> DeleteCategoryWithIdAsync(string id)
    {
        var flag = await _unitOfWork.GetRepository<Category>().AnyAsync(p => p.Id == id);

        if (!flag) return false;

        var category = await _unitOfWork.GetRepository<Category>().GetAsync(p => p.Id == id);
        category.DeletedId = await _userService.GetCurrentUserId();
        category.DeleteDateTime = DateTime.Now;
        category.IsDeleted = true;

        var books = await _unitOfWork.GetRepository<Book>().GetAllAsync(p => p.CategoryId == category.Id);

        foreach (var book in books)
        {
            book.DeletedId = await _userService.GetCurrentUserId();
            book.DeleteDateTime = DateTime.Now;
            book.IsDeleted = true;
        }

        await _unitOfWork.SaveAsync();
        return true;
    }

    public async Task<IEnumerable<IndexCategoryViewModel>> GetAllCategoriesNonDeletedAsync()
    {
        var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync(p => !p.IsDeleted, i => i.Books);
        var mapped = _mapper.Map<List<IndexCategoryViewModel>>(categories);
        return mapped;
    }

    public async Task<UpdateCategoryViewModel?> GetCategoryByIdWithUpdateViewModelAsync(string id)
    {
        var category = await _unitOfWork.GetRepository<Category>().GetAsync(p => p.Id == id);
        var mapped = _mapper.Map<UpdateCategoryViewModel>(category);
        return mapped;
    }

    public async Task<Dictionary<string, string>> GetCategoriesWithKeyAndNameAsync()
    {
        var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync(p => !p.IsDeleted);
        return categories.ToDictionary(k => k.Id, v => v.Name);
    }

    public async Task<(int NonDeleted, int Deleted)> CountCategoriesAsync()
    {
        var nonDeleted = await _unitOfWork.GetRepository<Category>().CountAsync(p => !p.IsDeleted);
        var deleted = await _unitOfWork.GetRepository<Category>().CountAsync(p => p.IsDeleted);
        return (nonDeleted, deleted);
    }
}