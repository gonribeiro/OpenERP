using System.ComponentModel.DataAnnotations;

namespace OpenERP.ViewModels.Global.Countries
{
    public class CountryViewModel
    {
        [Required]
        [StringLength(40, MinimumLength = 3)]
        public required string Name { get; set; }
        [Required]
        [StringLength(40, MinimumLength = 3)]
        public required string Nationality { get; set; }
    }
}
