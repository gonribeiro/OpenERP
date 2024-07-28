using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenERP.Models.Auth
{
    public class RoleUser
    {
        public int Id { get; set; }
        [Required]
        public int RoleId { get; set; }
        [Required]
        public int UserId { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        [ForeignKey("UserId")]
        [JsonIgnore]
        public User User { get; set; }
    }
}
