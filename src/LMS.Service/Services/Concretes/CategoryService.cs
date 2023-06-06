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
        category.UpdateDateTime = DateTime.UtcNow;
        await _unitOfWork.SaveAsync();
        return true;
    }

    public async Task<bool> DeleteCategoryWithIdAsync(string id)
    {
        var flag = await _unitOfWork.GetRepository<Category>().AnyAsync(p => p.Id == id);

        if (!flag) return false;

        var category = await _unitOfWork.GetRepository<Category>().GetAsync(p => p.Id == id);
        category.DeletedId = await _userService.GetCurrentUserId();
        category.DeleteDateTime = DateTime.UtcNow;
        category.IsDeleted = true;

        var children = await _unitOfWork.GetRepository<Category>().GetAllAsync(p => p.ParentCategoryId == category.Id);
        foreach (var child in children)
        {
            child.DeletedId = await _userService.GetCurrentUserId();
            child.DeleteDateTime = DateTime.UtcNow;
            child.IsDeleted = true;
        }

        var books = await _unitOfWork.GetRepository<Book>()
            .GetAllAsync(p => p.CategoryId == category.Id || children.Select(s => s.Id).Contains(p.Id));
        foreach (var book in books)
        {
            book.DeletedId = await _userService.GetCurrentUserId();
            book.DeleteDateTime = DateTime.UtcNow;
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

    public async Task<Dictionary<string, string>> GetParentCategoriesAsync()
    {
        var categories = await _unitOfWork.GetRepository<Category>()
            .GetAllAsync(p => !p.IsDeleted && p.ParentCategoryId == null);
        return categories.ToDictionary(p => p.Id, p => p.Name);
    }

    public async Task<Dictionary<string, string>> GetParentCategoriesAsync(string name)
    {
        var categories = await _unitOfWork.GetRepository<Category>()
            .GetAllAsync(p => !p.IsDeleted && p.ParentCategoryId == null && p.Name != name);
        return categories.ToDictionary(p => p.Id, p => p.Name);
    }

    public async Task<Dictionary<string, string>> GetCategoriesWithKeyAndNameAsync()
    {
        var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync();
        return categories.ToDictionary(k => k.Id, v => v.Name);
        // var finalNodes = context.Categories
        //     .Where(c => !context.Categories.Any(p => p.ParentCategoryId == c.Id))
        //     .ToList();
    }
}