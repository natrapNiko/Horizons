namespace Horizons.Services.Core
{
    using Horizons.Data;
    using Horizons.Data.Models;
    using Horizons.Services.Core.Contracts;
    using Horizons.Web.ViewModels.Destination;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using static GCommon.ValidationConstants.Destination;

    public class DestinationService : IDestinationService
    {
        private readonly HorizonDbContext dbContext;
        private readonly UserManager<IdentityUser> userManager;

        public DestinationService(HorizonDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
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

        public async Task<bool> AddDestinationAsync(string userId, AddDestinationInputModel inputModel)
        {
            bool opResult = false; //operation Result

            IdentityUser? user = await this.userManager
                .FindByIdAsync(userId);

            Terrain? terrainRef = await this.dbContext // get the terrain reference
                .Terrains
                .FindAsync(inputModel.TerrainId);

            bool isPublishedOnDateValid = DateTime.TryParseExact(inputModel.PublishedOn, PublishedOnFormat, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out DateTime publishedOnDate);

            if ((user != null) && (terrainRef != null) && (isPublishedOnDateValid))
            {
                Destination newDestination = new Destination()
                {
                    Name = inputModel.Name,
                    Description = inputModel.Description,
                    ImageUrl = inputModel.ImageUrl,
                    PublishedOn = publishedOnDate,
                    PublisherId = userId,
                    TerrainId = inputModel.TerrainId,
                };

                await this.dbContext.Destinations.AddAsync(newDestination);
                await this.dbContext.SaveChangesAsync();

                opResult = true; //operation was successful
            }

            return opResult;
        }

        public async Task<EditDestinationInputModel?> GetDestinationForEditingAsync(string userId, int? destId)
        {
            EditDestinationInputModel? editModel = null;

            if (destId != null)
            {
                Destination? editDestinationModel = await this.dbContext
                    .Destinations
                    .AsNoTracking()
                    .SingleOrDefaultAsync(d => d.Id == destId);

                // check if the user is the publisher of the destination
                if ((editDestinationModel != null) &&
                    (editDestinationModel.PublisherId.ToLower() == userId.ToLower()))
                {
                    editModel = new EditDestinationInputModel()
                    {
                        Id = editDestinationModel.Id,
                        Name = editDestinationModel.Name,
                        Description = editDestinationModel.Description,
                        ImageUrl = editDestinationModel.ImageUrl,
                        TerrainId = editDestinationModel.TerrainId,
                        PublishedOn = editDestinationModel.PublishedOn.ToString(PublishedOnFormat),
                        PublisherId = editDestinationModel.PublisherId,
                    };
                }
            }

            return editModel;
        }

        public async Task<bool> PersistUpdatedDestinationAsync(string userId, EditDestinationInputModel inputModel)
        {
            bool opResult = false; //operation Result

            IdentityUser? user = await this.userManager
                .FindByIdAsync(userId);

            Destination? updatedDest = await this.dbContext // get the destination to be updated
                .Destinations
                .FindAsync(inputModel.Id);

            Terrain? terrainRef = await this.dbContext // get the terrain reference
                .Terrains
                .FindAsync(inputModel.TerrainId);

            bool isPublishedOnDateValid = DateTime.TryParseExact(inputModel.PublishedOn, PublishedOnFormat, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out DateTime publishedOnDate);
            if ((user != null) && (terrainRef != null) &&
                (updatedDest != null) && (isPublishedOnDateValid) &&
                updatedDest.PublisherId.ToLower() == userId.ToLower())
            {
                updatedDest.Name = inputModel.Name;
                updatedDest.Description = inputModel.Description;
                updatedDest.ImageUrl = inputModel.ImageUrl;
                updatedDest.PublishedOn = publishedOnDate;
                updatedDest.TerrainId = inputModel.TerrainId;

                await this.dbContext.SaveChangesAsync();

                opResult = true; //operation was successful
            }

            return opResult;
        }

        public async Task<DeleteDestinationInputModel?> GetDestinationForDeletingAsync(string userId, int? destId)
        {
            DeleteDestinationInputModel? deleteModel = null;

            if (destId != null)
            {
                Destination? deleteDestinationModel = await this.dbContext
                    .Destinations
                    .Include(d => d.Publisher)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(d => d.Id == destId);

                if ((deleteDestinationModel != null) &&
                    (deleteDestinationModel.PublisherId.ToLower() == userId.ToLower()))
                {
                    deleteModel = new DeleteDestinationInputModel()
                    {
                        Id = deleteDestinationModel.Id,
                        Name = deleteDestinationModel.Name,
                        Publisher = deleteDestinationModel.Publisher.NormalizedUserName,
                        PublisherId = deleteDestinationModel.PublisherId,
                    };
                }
            }

            return deleteModel;
        }

        public async Task<bool> SoftDeleteDestinationAsync(string userId, DeleteDestinationInputModel inputModel)
        {
            bool opResult = false; //operation Result

            IdentityUser? user = await this.userManager
                .FindByIdAsync(userId);

            Destination? deletedDest = await this.dbContext // get the destination to be deleted
                .Destinations
                .FindAsync(inputModel.Id);
            if ((user != null) && (deletedDest != null) &&
                (deletedDest.PublisherId.ToLower() == userId.ToLower()))
            {
                deletedDest.IsDeleted = true; //soft delete the destination

                await this.dbContext.SaveChangesAsync();

                opResult = true; //operation was successful
            }

            return opResult;
        }

        public async Task<IEnumerable<FavoriteDestinationViewModel>?> GetUserFavoriteDestinationsAsync(string userId)
        {
            IEnumerable<FavoriteDestinationViewModel>? favDestinations = null;
            IdentityUser? user = await this.userManager
                .FindByIdAsync(userId);
            if (user != null)
            {
                favDestinations = await this.dbContext
                    .UsersDestinations
                    .Include(ud => ud.Destination)
                    .ThenInclude(d => d.Terrain)
                    .Where(ud => ud.UserId.ToLower() == userId.ToLower())
                    .Select(ud => new FavoriteDestinationViewModel()
                    {
                        Id = ud.Destination.Id,
                        Name = ud.Destination.Name,
                        ImageUrl = ud.Destination.ImageUrl,
                        Terrain = ud.Destination.Terrain.Name,
                    })
                    .ToArrayAsync();
            }

            return favDestinations;
        }

        public async Task<bool> AddDestinationToFavoritesAsync(string userId, int destinationId)
        {
            bool opResult = false; //operation Result
            IdentityUser? user = await this.userManager
                .FindByIdAsync(userId);
            Destination? favDestination = await this.dbContext
                .Destinations
                .FindAsync(destinationId);
            if ((user != null) && (favDestination != null) &&
                (favDestination.PublisherId.ToLower() != userId.ToLower()))
            {
                UserDestination? userFavDestination = await this.dbContext
                    .UsersDestinations
                    .SingleOrDefaultAsync(ud =>
                        ud.UserId.ToLower() == userId.ToLower() &&
                        ud.DestinationId == destinationId);
                if (userFavDestination == null)
                {
                    userFavDestination = new UserDestination()
                    {
                        UserId = userId,
                        DestinationId = destinationId,
                    };

                    await this.dbContext.UsersDestinations.AddAsync(userFavDestination);
                    await this.dbContext.SaveChangesAsync();

                    opResult = true; //operation was successful
                }
            }

            return opResult;
        }

        public async Task<bool> RemoveDestinationFromFavoritesAsync(string userId, int destinationId)
        {
            bool opResult = false; //operation Result
            IdentityUser? user = await this.userManager
                .FindByIdAsync(userId);
            Destination? favDestination = await this.dbContext
                .Destinations
                .FindAsync(destinationId);
            if ((user != null) && (favDestination != null) &&
                (favDestination.PublisherId.ToLower() != userId.ToLower()))
            {
                UserDestination? userFavDestination = await this.dbContext
                    .UsersDestinations
                    .SingleOrDefaultAsync(ud =>
                        ud.UserId.ToLower() == userId.ToLower() &&
                        ud.DestinationId == destinationId);
                if (userFavDestination != null)
                {
                    this.dbContext.UsersDestinations.Remove(userFavDestination);
                    await this.dbContext.SaveChangesAsync();

                    opResult = true; //operation was successful
                }
            }
            return opResult;
        }
    }
}
