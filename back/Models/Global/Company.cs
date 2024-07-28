using OpenERP.Enums.Global;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenERP.Models.Global
{
    public class Company
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 3)]
        public required string LegalName { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 3)]
        public required string TradeName { get; set; }
        [Required]
        [EnumDataType(typeof(CompanyType), ErrorMessage = "The value of the field Type is invalid.")]
        public required CompanyType Type { get; set; }
        public string? Address { get; set; }
        public string? ZipCode { get; set; }
        public int? CityId { get; set; }
        public string? ProductAndServiceDescription { get; set; }

        [ForeignKey("CityId")]
        public City? City { get; set; }
        public ICollection<Contact>? Contacts { get; set; } = new List<Contact>();

        public string? FullName => $"{TradeName} - {LegalName}";
    }
}