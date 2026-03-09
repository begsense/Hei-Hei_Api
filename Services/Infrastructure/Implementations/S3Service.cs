using Amazon.S3;
using Amazon.S3.Model;
using Hei_Hei_Api.Services.Infrastructure.Abstractions;

namespace Hei_Hei_Api.Services.Infrastructure.Implementations;

public class S3Service : IS3Service
{
    private readonly IAmazonS3 _s3;
    private readonly string _region;
    private readonly string _bucketNamePics;
    private readonly string _bucketNameLogs;

    public S3Service(IConfiguration configuration)
    {
        _region = configuration["AWS:Region"] ?? throw new InvalidOperationException("AWS Region is not configured.");
        _bucketNamePics = configuration["AWS:BucketName-Pics"] ?? throw new InvalidOperationException("AWS BucketName-Pics is not configured.");
        _bucketNameLogs = configuration["AWS:BucketName-Logs"] ?? throw new InvalidOperationException("AWS BucketName-Logs is not configured.");
        var accessKey = configuration["AWS:AccessKey"] ?? throw new InvalidOperationException("AWS AccessKey is not configured.");
        var secretKey = configuration["AWS:SecretKey"] ?? throw new InvalidOperationException("AWS SecretKey is not configured.");
        _s3 = new AmazonS3Client(accessKey, secretKey, Amazon.RegionEndpoint.GetBySystemName(_region));
    }

    public async Task<string> UploadPublicFileAsync(IFormFile file, string folder)
    {
        var key = $"{folder}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

        using var stream = file.OpenReadStream();

        var request = new PutObjectRequest
        {
            BucketName = _bucketNamePics,
            Key = key,
            InputStream = stream,
            ContentType = file.ContentType,
            CannedACL = S3CannedACL.PublicRead
        };

        await _s3.PutObjectAsync(request);

        return $"https://{_bucketNamePics}.s3.amazonaws.com/{key}";
    }

    public async Task UploadPrivateFileAsync(Stream stream, string key, string contentType)
    {
        var request = new PutObjectRequest
        {
            BucketName = _bucketNameLogs,
            Key = key,
            InputStream = stream,
            ContentType = contentType
        };

        await _s3.PutObjectAsync(request);
    }
}
