using System.ComponentModel.DataAnnotations;

namespace Horizons.Web.ViewModels.Destination
{
    public class EditDestinationInputModel : AddDestinationInputModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string PublisherId { get; set; } = null!;
    }
}
