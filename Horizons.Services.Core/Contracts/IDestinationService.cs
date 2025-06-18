namespace Horizons.Services.Core.Contracts
{
    using Horizons.Web.ViewModels.Destination;

    public interface IDestinationService
    {
        Task<IEnumerable<DestinationIndexViewModel>> GetAllDestinationsAsync(string? userId);

        Task<DestinationDetailsViewModel?> GetDestinationDetailsAsync(int? id, string? userId);

        Task<bool> AddDestinationAsync(string userId, AddDestinationInputModel inputModel);

        //destId is DestinationId
        Task<EditDestinationInputModel?> GetDestinationForEditingAsync(string userId, int? destId); 

        Task<bool> PersistUpdatedDestinationAsync(string userId, EditDestinationInputModel inputModel);

        Task<DeleteDestinationInputModel?> GetDestinationForDeletingAsync(string userId, int? destId);

        Task<bool> SoftDeleteDestinationAsync(string userId, DeleteDestinationInputModel inputModel );
    }
}
