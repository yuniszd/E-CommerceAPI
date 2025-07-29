using E_CommerceAPI.Application.Abstracts.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;


namespace E_CommerceAPI.Persistence.Services;

public class FileService : IFileService
{
    public async Task<string> SaveFileAsync(IFormFile file, string folderPath)
    {
        if (file == null || file.Length == 0)
            throw new Exception("Fayl tapılmadı");

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(folderPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return fileName;
    }
}
