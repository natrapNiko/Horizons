namespace Horizons.Services.Core.Contracts
{
    using Horizons.Web.ViewModels.Destination;

    public interface ITerrainService
    {
        Task<IEnumerable<AddDestinationTerrainDropDownModel>> GetTerrainsDropdownDataAsync();
    }
}
