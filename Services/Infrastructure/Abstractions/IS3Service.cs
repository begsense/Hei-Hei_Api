namespace Hei_Hei_Api.Services.Infrastructure.Abstractions;

public interface IS3Service
{
    Task<string> UploadPublicFileAsync(IFormFile file, string folder);

    Task UploadPrivateFileAsync(Stream stream, string key, string contentType);
}
