using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.Common.Services;

public class FileUploadService : IFileUploadService
{
    private readonly ILogger<FileUploadService> _logger;
    private readonly string _uploadPath;
    private readonly string _baseUrl;
    
    // Allowed image extensions
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
    
    // Max file size: 5MB
    private const long MaxFileSize = 5 * 1024 * 1024;

    public FileUploadService(ILogger<FileUploadService> logger, string uploadPath, string baseUrl)
    {
        _logger = logger;
        _uploadPath = uploadPath;
        _baseUrl = baseUrl;

        // Create upload directory if it doesn't exist
        if (!Directory.Exists(_uploadPath))
        {
            Directory.CreateDirectory(_uploadPath);
            _logger.LogInformation("Created upload directory: {Path}", _uploadPath);
        }
    }

    public async Task<string> UploadProfileImageAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate file
            if (!IsValidImage(fileName, fileStream.Length))
            {
                throw new InvalidOperationException("Invalid image file");
            }

            // Generate unique file name
            var extension = Path.GetExtension(fileName).ToLower();
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(_uploadPath, uniqueFileName);

            // Save file
            using (var fileStreamOutput = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.CopyToAsync(fileStreamOutput, cancellationToken);
            }

            _logger.LogInformation("Profile image uploaded successfully: {FileName}", uniqueFileName);

            // Return public URL
            return $"{_baseUrl}/uploads/profiles/{uniqueFileName}";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading profile image: {FileName}", fileName);
            throw;
        }
    }

    public Task<bool> DeleteProfileImageAsync(string imageUrl, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrEmpty(imageUrl))
                return Task.FromResult(false);

            // Extract file name from URL
            var fileName = Path.GetFileName(imageUrl);
            var filePath = Path.Combine(_uploadPath, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                _logger.LogInformation("Profile image deleted: {FileName}", fileName);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting profile image: {ImageUrl}", imageUrl);
            return Task.FromResult(false);
        }
    }

    public bool IsValidImage(string fileName, long fileSize)
    {
        // Check file size
        if (fileSize > MaxFileSize)
        {
            _logger.LogWarning("File size exceeds maximum: {FileSize} bytes", fileSize);
            return false;
        }

        // Check extension
        var extension = Path.GetExtension(fileName).ToLower();
        if (!AllowedExtensions.Contains(extension))
        {
            _logger.LogWarning("Invalid file extension: {Extension}", extension);
            return false;
        }

        return true;
    }
}

