using System.ComponentModel.DataAnnotations;

namespace OpenERP.ViewModels.Auth.Users
{
    public class GetRoleViewModel
    {
        public required int Id { get; set; }
        public string? InactiveDate { get; set; }
        [StringLength(255, MinimumLength = 3)]
        public required string Name { get; set; }
    }
}
