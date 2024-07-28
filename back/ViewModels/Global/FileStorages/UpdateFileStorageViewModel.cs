using System.ComponentModel.DataAnnotations;

namespace OpenERP.ViewModels.Global.FileStorages
{
    public class UpdateFileStorageViewModel
    {
        [Required]
        public IFormFile File { get; set; }
        public string? Description { get; set; }
    }
}
