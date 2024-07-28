using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenERP.Models.HumanResource
{
    public class Department
    {
        public int Id { get; set; }
        [Required]
        [StringLength(40, MinimumLength = 3)]
        public required string Name { get; set; }
        public int? ManagerId { get; set; }

        [ForeignKey("ManagerId")]
        public Employee Manager { get; set; }
        public ICollection<JobHistory> JobHistories { get; set; }
    }
}
