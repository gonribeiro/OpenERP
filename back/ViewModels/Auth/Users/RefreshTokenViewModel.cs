using System.ComponentModel.DataAnnotations;

namespace OpenERP.ViewModels.Auth.Users
{
    public class RefreshTokenViewModel
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
