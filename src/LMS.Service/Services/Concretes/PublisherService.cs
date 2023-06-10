using AutoMapper;
using LMS.Data.UnitOfWorks;
using LMS.Entity.Entities;
using LMS.Entity.ViewModels.Publisher;
using LMS.Service.Services.Abstracts;

namespace LMS.Service.Services.Concretes;

public class PublisherService : IPublisherService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;

    public PublisherService(IMapper mapper, IUnitOfWork unitOfWork, IUserService userService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userService = userService;
    }

    public async Task<bool> CreateNewPublisherAsync(CreatePublisherViewModel viewModel)
    {
        if (await _unitOfWork.GetRepository<Publisher>().AnyAsync(p => p.Name == viewModel.Name)) return false;
        var mapped = _mapper.Map<Publisher>(viewModel);
        mapped.CreatedId = await _userService.GetCurrentUserId();
        await _unitOfWork.GetRepository<Publisher>().AddAsync(mapped);
        await _unitOfWork.SaveAsync();
        return true;
    }

    public async Task<bool> UpdatePublisherAsync(UpdatePublisherViewModel viewModel)
    {
        if (await _unitOfWork.GetRepository<Publisher>()
                .AnyAsync(p => p.Name == viewModel.Name && p.Id != viewModel.Id))
            return false;

        var flag = await _unitOfWork.GetRepository<Publisher>().AnyAsync(p => p.Id == viewModel.Id);

        if (!flag) return false;

        var publisher = await _unitOfWork.GetRepository<Publisher>().GetAsync(p => p.Id == viewModel.Id);
        _mapper.Map(viewModel, publisher);
        publisher.UpdatedId = await _userService.GetCurrentUserId();
        publisher.UpdateDateTime = DateTime.Now;
        await _unitOfWork.SaveAsync();
        return true;
    }

    public async Task<bool> DeletePublisherWithIdAsync(string id)
    {
        var flag = await _unitOfWork.GetRepository<Publisher>().AnyAsync(p => p.Id == id);

        if (!flag) return false;

        var publisher = await _unitOfWork.GetRepository<Publisher>().GetAsync(p => p.Id == id);
        publisher.DeletedId = await _userService.GetCurrentUserId();
        publisher.DeleteDateTime = DateTime.Now;
        publisher.IsDeleted = true;

        var books = await _unitOfWork.GetRepository<Book>().GetAllAsync(p => p.PublisherId == publisher.Id);
        foreach (var book in books)
        {
            book.DeletedId = await _userService.GetCurrentUserId();
            book.DeleteDateTime = DateTime.Now;
            book.IsDeleted = true;
        }

        await _unitOfWork.SaveAsync();
        return true;
    }

    public async Task<IEnumerable<IndexPublisherViewModel>> GetAllPublishersNonDeletedAsync()
    {
        var publishers = await _unitOfWork.GetRepository<Publisher>().GetAllAsync(p => !p.IsDeleted, i => i.Books);
        var mapped = _mapper.Map<List<IndexPublisherViewModel>>(publishers);
        return mapped;
    }

    public async Task<UpdatePublisherViewModel?> GetPublisherByIdWithUpdateViewModelAsync(string id)
    {
        var publisher = await _unitOfWork.GetRepository<Publisher>().GetAsync(p => p.Id == id);
        var mapped = _mapper.Map<UpdatePublisherViewModel>(publisher);
        return mapped;
    }

    public async Task<Dictionary<string, string>> GetPublishersWithKeyAndNameAsync()
    {
        var publishers = await _unitOfWork.GetRepository<Publisher>().GetAllAsync(p => !p.IsDeleted);
        return publishers.ToDictionary(k => k.Id, v => v.Name);
    }
}