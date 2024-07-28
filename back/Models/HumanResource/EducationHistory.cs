using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using OpenERP.Models.Global;

namespace OpenERP.Models.HumanResource
{
    public class EducationHistory
    {
        public int Id { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 3)]
        public int InstitutionId { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string Course { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string EducationLevel { get; set; }
        [Required]
        [Column(TypeName = "Date")]
        public DateTime StartDate { get; set; }
        [Required]
        [Column(TypeName = "Date")]
        public DateTime EndDate { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
        [ForeignKey("InstitutionId")]
        public Company Institution { get; set; }
    }
}
