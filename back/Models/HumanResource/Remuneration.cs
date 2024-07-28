using OpenERP.Enums.Global;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenERP.Models.HumanResource
{
    public class Remuneration
    {
        public int Id { get; set; }
        [Required]
        public required int EmployeeId { get; set; }
        [Required]
        [EnumDataType(typeof(Currency), ErrorMessage = "The value of the field Type is invalid.")]
        public Currency Currency { get; set; }
        [Required]
        [Column(TypeName = "decimal(13, 2)")]
        public decimal BaseSalary { get; set; }
        [Column(TypeName = "decimal(13, 2)")]
        public decimal? Bonus { get; set; }
        [Column(TypeName = "decimal(13, 2)")]
        public decimal? Commission { get; set; }
        [Column(TypeName = "decimal(13, 2)")]
        public decimal? OtherAllowances { get; set; }
        [StringLength(255)]
        public string? OtherAllowancesDescription { get; set; }
        [Required]
        [Column(TypeName = "Date")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? EndDate { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        public decimal TotalCompensation
        {
            get
            {
                return BaseSalary +  (Bonus ?? 0) + (Commission ?? 0) + (OtherAllowances ?? 0);
            }
        }
    }
}
