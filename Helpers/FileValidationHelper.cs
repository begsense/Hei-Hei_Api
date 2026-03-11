namespace Hei_Hei_Api.Helpers;

public static class FileValidationHelper
{
    private static readonly string[] AllowedImageTypes = { "image/jpeg", "image/png", "image/webp" };
    private const long MaxImageSize = 5 * 1024 * 1024;

    public static void ValidateImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("Image file is required.");
        }

        if (!AllowedImageTypes.Contains(file.ContentType.ToLower()))
        {
            throw new ArgumentException("Invalid image format. Allowed formats: JPEG, PNG, WebP.");
        }

        if (file.Length > MaxImageSize)
        {
            throw new ArgumentException("Image size exceeds 5MB.");
        }
    }
}