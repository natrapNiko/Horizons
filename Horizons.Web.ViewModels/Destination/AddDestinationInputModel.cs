
namespace Horizons.Web.ViewModels.Destination
{
    using System.ComponentModel.DataAnnotations;
    using static GCommon.ValidationConstants.Destination;
    public class AddDestinationInputModel
    {
        [Required]
        [MinLength(NameMinLength)]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(DescriptionMinLength)]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        public string? ImageUrl { get; set; }

        [Required]
        [MinLength(PublishedOnLength)]
        [MaxLength(PublishedOnLength)]
        public string PublishedOn { get; set; } = null!;

        public int TerrainId { get; set; }

        public IEnumerable<AddDestinationTerrainDropDownModel> Terrains { get; set; } = null!;

    }
}
