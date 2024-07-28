using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OpenERP.Models.HumanResource;

namespace OpenERP.Models.Global
{
    public class FileStorage
    {
        public int Id { get; set; }
        public string Description { get; set; }
        [Required]
        public string ModelType { get; set; }
        [Required]
        public int ModelId { get; set; }
        [Required]
        public string OriginalFileName { get; set; }
        [Required]
        public string FilePath { get; set; }
        [Required]
        public string FileExtension { get; set; }
        [Required]
        public long FileSize { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("ModelId")]
        public Employee Employee { get; set; }
    }
}
