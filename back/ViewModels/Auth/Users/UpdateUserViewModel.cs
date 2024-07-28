using System.ComponentModel.DataAnnotations;

namespace OpenERP.ViewModels.Auth.Users
{
    public class UpdateUserViewModel
    {
        public DateTime? InactiveDate { get; set; }
        [StringLength(120, MinimumLength = 3)]
        public required string Username { get; set; }
        [StringLength(255, MinimumLength = 6)]
        public string? Password { get; set; }

        public List<int>? RoleIds { get; set; }
    }
}
