using System.ComponentModel.DataAnnotations;

namespace OpenERP.ViewModels.Auth.Users
{
    public class RoleViewModel
    {
        public int? Id { get; set; }
        public DateTime? InactiveDate { get; set; }
        [StringLength(255, MinimumLength = 3)]
        public required string Name { get; set; }
    }
}
