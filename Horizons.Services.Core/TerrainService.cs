
namespace Horizons.Services.Core
{
    using Horizons.Data;
    using Horizons.Services.Core.Contracts;
    using Horizons.Web.ViewModels.Destination;

    using Microsoft.EntityFrameworkCore;

    public class TerrainService : ITerrainService
    {
        private readonly HorizonDbContext dbContext;

        public TerrainService(HorizonDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<AddDestinationTerrainDropDownModel>> GetTerrainsDropdownDataAsync()
        {
            IEnumerable<AddDestinationTerrainDropDownModel> terrainsAsDropdown = await this.dbContext
                .Terrains
                .AsNoTracking()
                .Select(t => new AddDestinationTerrainDropDownModel()
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToArrayAsync();

            return terrainsAsDropdown;
        }

    }
}
