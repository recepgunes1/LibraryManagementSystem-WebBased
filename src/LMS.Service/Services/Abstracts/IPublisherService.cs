using LMS.Entity.ViewModels.Publisher;

namespace LMS.Service.Services.Abstracts;

public interface IPublisherService
{
    Task<bool> CreateNewPublisherAsync(CreatePublisherViewModel viewModel);
    Task<bool> UpdatePublisherAsync(UpdatePublisherViewModel viewModel);
    Task<bool> DeletePublisherWithIdAsync(string id);
    Task<IEnumerable<IndexPublisherViewModel>> GetAllPublishersNonDeletedAsync();
    Task<UpdatePublisherViewModel?> GetPublisherByIdWithUpdateViewModelAsync(string id);
    Task<Dictionary<string, string>> GetPublishersWithKeyAndNameAsync();
}