namespace Horizons.Services.Core
{
    using Horizons.Data;
    using Horizons.Services.Core.Contracts;
    using Horizons.Web.ViewModels.Destination;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class DestinationService : IDestinationService
    {
        private readonly HorizonDbContext dbContext;

        public DestinationService(HorizonDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<DestinationIndexViewModel>> GetAllDestinationsAsync(string? userId)
        {
            IEnumerable<DestinationIndexViewModel> allDestinations = await this.dbContext
                .Destinations
                .Include(d => d.Terrain)
                .Include(d => d.UsersDestinations)
                .AsNoTracking()
                .Select(d => new DestinationIndexViewModel()
                {
                    Id = d.Id,
                    Name = d.Name,
                    ImageUrl = d.ImageUrl,
                    TerrainName = d.Terrain.Name,
                    FavoritesCount = d.UsersDestinations.Count,
                    IsUserPublisher = userId != null ?
                        d.PublisherId.ToLower() == userId!.ToLower() : false,
                    IsInUserFavorites = userId != null ?
                        d.UsersDestinations.Any(ud => ud.UserId.ToLower() == userId!.ToLower()) : false,
                })
                .ToArrayAsync();

            return allDestinations;
        }
    }
}
