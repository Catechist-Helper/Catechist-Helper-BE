using Microsoft.AspNetCore.Http;

namespace CatechistHelper.Application.GoogleServices
{
    public interface IFirebaseService
    {
        Task<string> UploadImageAsync(IFormFile imageFile, string imagePath);

        string GetImageUrl(string folderName, string imageName);

        Task DeleteImageAsync(string imageName);

        Task DeleteImagesAsync(List<string> imageUrls);

        Task<string[]> UploadImagesAsync(List<IFormFile> imageFiles, string folderPath);

        Task<IFormFile> DownloadImageFromUrl(string imageUrl, string fileName, string contentType);
    }
}
