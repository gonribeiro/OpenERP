using System.ComponentModel.DataAnnotations;

namespace OpenERP.ViewModels.Global.States
{
    public class StateViewModel
    {
        [Required]
        [StringLength(40, MinimumLength = 3)]
        public required string Name { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "The CountryId field is required.")]
        public required int CountryId { get; set; }
    }
}
