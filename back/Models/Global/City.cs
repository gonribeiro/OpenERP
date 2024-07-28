using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenERP.Models.Global
{
    public class City
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120, MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        public int StateId { get; set; }

        [ForeignKey("StateId")]
        public State State { get; set; }

        public ICollection<Company>? Companies { get; set; }
    }
}