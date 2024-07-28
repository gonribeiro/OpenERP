using OpenERP.Data;

namespace OpenERP.Services.Global
{
    public class FileStorageService
    {
        private readonly AppDbContext _context;

        public FileStorageService(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> StoreFileAsync(IFormFile file, string uploadDirectory)
        {
            var newFileName = Guid.NewGuid().ToString();
            var fileExtension = Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadDirectory, newFileName + fileExtension);

            using (var stream = new FileStream(filePath, FileMode.Create))
                await file.CopyToAsync(stream);

            return filePath;
        }

        public async Task<(MemoryStream memoryStream, string fileName)> DownloadFileAsync(int id)
        {
            var fileStorage = await _context.FileStorages.FindAsync(id);

            if (fileStorage == null) throw new ArgumentException("File not found");

            var memory = new MemoryStream();

            using (var stream = new FileStream(fileStorage.FilePath, FileMode.Open))
                await stream.CopyToAsync(memory);

            memory.Position = 0;

            return (memory, fileStorage.FilePath);
        }

        public async Task DeleteFileAsync(string filePath)
        {
            await Task.Run(() => File.Delete(filePath));
        }
    }
}
