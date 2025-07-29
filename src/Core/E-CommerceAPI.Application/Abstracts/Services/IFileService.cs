using Microsoft.AspNetCore.Http;

namespace E_CommerceAPI.Application.Abstracts.Services;

public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile file, string folderName);
}
