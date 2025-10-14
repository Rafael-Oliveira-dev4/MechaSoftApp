namespace MechaSoft.Application.Common.Services;

public interface IFileUploadService
{
    /// <summary>
    /// Uploads a profile image and returns the URL
    /// </summary>
    Task<string> UploadProfileImageAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a profile image by URL
    /// </summary>
    Task<bool> DeleteProfileImageAsync(string imageUrl, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates if the file is a valid image
    /// </summary>
    bool IsValidImage(string fileName, long fileSize);
}

