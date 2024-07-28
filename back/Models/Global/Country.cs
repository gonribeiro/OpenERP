using System.ComponentModel.DataAnnotations;

namespace OpenERP.Models.Global
{
    public class Country
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120, MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        [StringLength(120, MinimumLength = 3)]
        public string Nationality { get; set; }

        public ICollection<State>? States { get; set; }
    }
}