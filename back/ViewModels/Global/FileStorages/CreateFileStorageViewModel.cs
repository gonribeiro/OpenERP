using System.ComponentModel.DataAnnotations;

namespace OpenERP.ViewModels.Global.FileStorages
{
    public class CreateFileStorageViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int ModelId { get; set; }
        public string ModelType { get; set; }
        [Required]
        public IFormFile File { get; set; }
    }
}
