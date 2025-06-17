namespace Horizons.Web.Controllers
{
    using Horizons.Services.Core.Contracts;

    using Horizons.Web.ViewModels.Destination;
    using static GCommon.ValidationConstants.Destination;

    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;

    public class DestinationController : BaseController
    {
        private readonly IDestinationService destinationService;
        private readonly ITerrainService terrainService;

        public DestinationController(IDestinationService destinationService, ITerrainService terrainService)
        {
            this.destinationService = destinationService;
            this.terrainService = terrainService;
        }

        [HttpGet]
        [AllowAnonymous] //available to unregistered users
        public async Task<IActionResult> Index()
        {
            try
            {
                string? userId = this.GetUserId();
                IEnumerable<DestinationIndexViewModel> allDestinations = await
                    this.destinationService.GetAllDestinationsAsync(userId);

                return View(allDestinations);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index), "Home");
            }
        }

        [HttpGet]
        [AllowAnonymous] // available to unregistered users
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                string? userId = this.GetUserId();
                DestinationDetailsViewModel? destinationDetails = await this.destinationService
                    .GetDestinationDetailsAsync(id, userId);
                if (destinationDetails == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(destinationDetails);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index), "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        { 
            AddDestinationInputModel inputModel = new AddDestinationInputModel()
            {
                PublishedOn = DateTime.UtcNow.ToString(PublishedOnFormat),
                Terrains = await this.terrainService.GetTerrainsDropdownDataAsync()
            };

            return this.View(inputModel);
        }
    }
}
