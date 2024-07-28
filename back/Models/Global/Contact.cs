using OpenERP.Enums.Global;
using OpenERP.Enums.HumanResource;
using OpenERP.Models.HumanResource;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenERP.Models.Global
{
    public class Contact
    {
        public int Id { get; set; }
        [Required]
        public required string ModelType { get; set; }
        [Required]
        public int ModelId { get; set; }
        [Required]
        [EnumDataType(typeof(ContactType), ErrorMessage = "The value of the field Type is invalid.")]
        public required ContactType Type { get; set; }
        [Required]
        [StringLength(120, MinimumLength = 3)]
        public required string Information { get; set; }
        [StringLength(120, MinimumLength = 3)]
        public string? ContactName { get; set; }
        [EnumDataType(typeof(ContactRelationType), ErrorMessage = "The value of the field Type is invalid.")]
        public ContactRelationType? ContactRelationType { get; set; }

        [ForeignKey("ModelId")]
        public Employee Employee { get; set; }
        [ForeignKey("ModelId")]
        public Company? Company { get; set; }
    }
}