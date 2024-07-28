using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using OpenERP.Enums.HumanResource;

namespace OpenERP.Models.HumanResource
{
    public class Vacation
    {
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        [Required]
        [EnumDataType(typeof(VacationType), ErrorMessage = "The value of the field Type is invalid.")]
        public required VacationType Type { get; set; }
        [StringLength(255, MinimumLength = 3)]
        [Required]
        [Column(TypeName = "Date")]
        public DateTime StartDate { get; set; }
        [Required]
        [Column(TypeName = "Date")]
        public DateTime EndDate { get; set; }
        public string? Reason { get; set; }
        public int? ApprovedById { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
        [ForeignKey("ApprovedById")]
        public Employee ApprovedBy { get; set; }
    }
}
