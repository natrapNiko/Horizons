namespace Horizons.Services.Core.Contracts
{
    using Horizons.Web.ViewModels.Destination;

    public interface IDestinationService
    {
        Task<IEnumerable<DestinationIndexViewModel>> GetAllDestinationsAsync(string? userId);

        Task<DestinationDetailsViewModel?> GetDestinationDetailsAsync(int? id, string? userId);

        Task<bool> AddDestinationAsync(string userId, AddDestinationInputModel inputModel);
    }
}
