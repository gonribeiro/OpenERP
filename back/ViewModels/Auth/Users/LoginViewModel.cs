using System.ComponentModel.DataAnnotations;

namespace OpenERP.ViewModels.Auth.Users
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string Username { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 5)]
        public string Password { get; set; }
    }
}
