using System.ComponentModel.DataAnnotations;
using OpenERP.ViewModels.Global.Contacts;

namespace OpenERP.ViewModels.Global.Companies
{
    public class CompanyViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 3)]
        public required string LegalName { get; set; }
        public string? TradeName { get; set; }
        [Required]
        public required string Type { get; set; }
        public string? Address { get; set; }
        public string? ZipCode { get; set; }
        public int? CityId { get; set; }
        public string? ProductAndServiceDescription { get; set; }
        public List<ContactViewModel>? Contacts { get; set; }
    }
}
