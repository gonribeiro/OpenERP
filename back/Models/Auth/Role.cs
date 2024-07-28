using System.ComponentModel.DataAnnotations;

namespace OpenERP.Models.Auth
{
    public class Role
    {
        public int Id { get; set; }
        public DateTime? InactiveDate { get; set; }
        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string Name { get; set; }

        public ICollection<RoleUser> RoleUser { get; set; }
    }
}
