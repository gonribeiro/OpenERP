using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenERP.Data;
using OpenERP.Models.Global;
using OpenERP.Services.Global;
using OpenERP.ViewModels.Global.FileStorages;

namespace OpenERP.Controllers.v1
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class FileStorageController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly FileStorageService _fileStorageService;
        private readonly string _uploadDirectory;

        public FileStorageController(
            [FromServices] AppDbContext context,
            FileStorageService fileStorageService,
            IConfiguration configuration)
        {
            _context = context;
            _fileStorageService = fileStorageService;
            _uploadDirectory = configuration.GetValue<string>("FileStorage:UploadDirectory");
        }

        [HttpPost("v1/fileStorages")]
        public async Task<IActionResult>PostAsync([FromForm] CreateFileStorageViewModel model)
        {
            try
            {
                var filePath = await _fileStorageService.StoreFileAsync(model.File, _uploadDirectory);

                var fileStorage = new FileStorage
                {
                    ModelId = model.ModelId,
                    Description = model.Description,
                    ModelType = model.ModelType,
                    OriginalFileName = Path.GetFileNameWithoutExtension(model.File.FileName),
                    FilePath = filePath,
                    FileExtension = Path.GetExtension(model.File.FileName),
                    FileSize = model.File.Length,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };
                await _context.FileStorages.AddAsync(fileStorage);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    fileStorage,
                    message = "Created successfully",
                    filePath = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + "/v1/fileStorages/" + fileStorage.Id + "/download",
                });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, ex);
            }
            catch
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("v1/fileStorages/{id:int}/download")]
        public async Task<IActionResult> DownloadFile(int id)
        {
            try
            {
                var (memoryStream, fileName) = await _fileStorageService.DownloadFileAsync(id);
                return File(memoryStream, "application/octet-stream", fileName);
            }
            catch (FileNotFoundException)
            {
                return NotFound("File not found");
            }
        }

        [HttpPut("v1/fileStorages/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int id,
            [FromForm] UpdateFileStorageViewModel model)
        {
            try
            {
                var fileStorage = await _context.FileStorages.FirstOrDefaultAsync(x => x.Id == id);

                if (fileStorage == null) return NotFound("File not found");

                await _fileStorageService.DeleteFileAsync(fileStorage.FilePath);

                var filePath = await _fileStorageService.StoreFileAsync(model.File, _uploadDirectory);

                fileStorage.OriginalFileName = Path.GetFileNameWithoutExtension(model.File.FileName);
                fileStorage.FilePath = filePath;
                fileStorage.FileExtension = Path.GetExtension(model.File.FileName);
                fileStorage.FileSize = model.File.Length;
                fileStorage.Description = !string.IsNullOrWhiteSpace(model.Description) ? model.Description : fileStorage.Description;
                fileStorage.UpdatedAt = DateTime.UtcNow;

                _context.FileStorages.Update(fileStorage);
                await _context.SaveChangesAsync();

                return Ok(new { fileStorage, message = "Saved successfully" });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, ex);
            }
            catch
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("v1/fileStorages/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            try
            {
                var fileStorage = await _context.FileStorages.FirstOrDefaultAsync(x => x.Id == id);

                if (fileStorage == null) return NotFound("File not found");

                await _fileStorageService.DeleteFileAsync(fileStorage.FilePath);

                _context.FileStorages.Remove(fileStorage);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    fileStorage,
                    message = "Deleted successfully",
                });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, ex);
            }
            catch
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
