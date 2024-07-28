using System.ComponentModel.DataAnnotations;

namespace OpenERP.ViewModels.Auth.Users
{
    public class CreateUserViewModel
    {
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        [StringLength(120, MinimumLength = 3)]
        public string Username { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 6)]
        public string Password { get; set; }

        public List<int>? RoleIds { get; set; }
    }
}
