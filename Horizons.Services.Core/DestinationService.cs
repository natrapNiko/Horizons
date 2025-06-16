namespace Horizons.Services.Core
{
    using Horizons.Data;
    using Horizons.Data.Models;
    using Horizons.Services.Core.Contracts;
    using Horizons.Web.ViewModels.Destination;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using static GCommon.ValidationConstants.Destination;

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

        public async Task<DestinationDetailsViewModel?> GetDestinationDetailsAsync(int? id, string? userId)
        {
            DestinationDetailsViewModel? detailsVm = null;
            if (id.HasValue)
            {
                Destination? destinationModel = await this.dbContext
                    .Destinations
                    .Include(d => d.Terrain)
                    .Include(d => d.Publisher)
                    .Include(d => d.UsersDestinations)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(d => d.Id == id.Value);
                if (destinationModel != null)
                {
                    detailsVm = new DestinationDetailsViewModel()
                    {
                        Id = destinationModel.Id,
                        Name = destinationModel.Name,
                        ImageUrl = destinationModel.ImageUrl,
                        Description = destinationModel.Description,
                        PublishedOn = destinationModel.PublishedOn.ToString(PublishedOnFormat),
                        TerrainName = destinationModel.Terrain.Name,
                        PublisherName = destinationModel.Publisher.UserName,
                        IsUserPublisher = userId != null ?
                            destinationModel.PublisherId.ToLower() == userId!.ToLower() : false,
                        IsInUserFavorites = userId != null ?
                            destinationModel.UsersDestinations
                                .Any(ud => ud.UserId.ToLower() == userId!.ToLower()) : false,
                    };
                }
            }

            return detailsVm;
        }
    }
}
