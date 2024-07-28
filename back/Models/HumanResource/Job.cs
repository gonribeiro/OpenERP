using OpenERP.Enums.Global;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenERP.Models.HumanResource
{
    public class Job
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120, MinimumLength = 3)]
        public required string Name { get; set; }
        [Required]
        [EnumDataType(typeof(Currency), ErrorMessage = "The value of the field Type is invalid.")]
        public required Currency Currency { get; set; }
        [Column(TypeName = "decimal(13,2)")]
        public decimal? MinSalary { get; set; }
        [Column(TypeName = "decimal(13,2)")]
        public decimal? MaxSalary { get; set; }

        public ICollection<JobHistory> JobHistories { get; set; }
    }
}