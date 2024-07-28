using System.ComponentModel.DataAnnotations;

namespace OpenERP.ViewModels.Global.Cities
{
    public class CityViewModel
    {
        [Required]
        [StringLength(120, MinimumLength = 3)]
        public required string Name { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "The StateId field is required.")]
        public required int StateId { get; set; }
    }
}
