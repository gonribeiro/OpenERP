using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OpenERP.Models.HumanResource;

namespace OpenERP.Models.Auth
{
    public class User
    {
        public int Id { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? InactiveDate { get; set; }
        public int? EmployeeId { get; set; }
        [Required]
        [StringLength(120, MinimumLength = 3)]
        public required string Username { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 5)]
        public required string Password { get; set; }
        [Required]
        public required DateTime CreatedAt { get; set; }
        public DateTime? LastPasswordUpdatedAt { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; set; }
        public ICollection<RoleUser> RoleUser { get; set; }
    }
}
