﻿using CatechistHelper.Application.GoogleServices;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using static CatechistHelper.Infrastructure.Utils.ImageUtil;

namespace CatechistHelper.Infrastructure.GoogleServices
{

    public class FirebaseService : IFirebaseService
    {
        private readonly StorageClient _storageClient;
        private readonly string _bucketName;
        public FirebaseService(StorageClient storageClient, IConfiguration configuration)
        {
            _storageClient = storageClient;
            _bucketName = configuration["Firebase:Bucket"]!;
        }

        public string GetImageUrl(string folderName, string imageName)
        {

            var encodedFolderName = Uri.EscapeDataString(folderName);
            var encodedImageName = Uri.EscapeDataString(imageName);

            var imageUrl = $"https://firebasestorage.googleapis.com/v0/b/{_bucketName}/o/{encodedFolderName}%2F{encodedImageName}?alt=media";
            return imageUrl;
        }

        private string ExtractImageNameFromUrl(string imageUrl)
        {
            var uri = new Uri(imageUrl);
            var segments = uri.Segments;
            var escapedImageName = segments[^1];
            var imageName = Uri.UnescapeDataString(escapedImageName);
            return imageName;
        }

        public async Task DeleteImageAsync(string imageUrl)
        {
            await _storageClient.DeleteObjectAsync(_bucketName, ExtractImageNameFromUrl(imageUrl), cancellationToken: CancellationToken.None);
        }

        public async Task DeleteImagesAsync(List<string> imageUrls)
        {
            var deleteImageTasks = new List<Task>();

            foreach (var imageUrl in imageUrls)
            {
                deleteImageTasks.Add(DeleteImageAsync(imageUrl));
            }

            await Task.WhenAll(deleteImageTasks);
        }

        public async Task<string[]> UploadImagesAsync(List<IFormFile> imageFiles, string folderPath)
        {
            var uploadTasks = new List<Task<string>>();

            foreach (var imageFile in imageFiles)
            {
                var filePath = $"{folderPath}/{Path.GetFileName(imageFile.FileName)}";
                uploadTasks.Add(UploadImageAsyncV2(imageFile, filePath));
            }

            var imageUrls = await Task.WhenAll(uploadTasks);
            return imageUrls;
        }
        public async Task<string> UploadImageAsyncV2(IFormFile imageFile, string imagePath)
        {
            // Đặt giới hạn kích thước file (ví dụ 5MB = 5 * 1024 * 1024 bytes)
            long fileSizeLimit = 5 * 1024 * 1024;

            if (imageFile.Length > fileSizeLimit)
            {
                throw new Exception("Kích thước file vượt quá giới hạn cho phép (5MB).");
            }
            using var stream = new MemoryStream();
            await imageFile.CopyToAsync(stream);
            stream.Position = 0;

            var blob = await _storageClient.UploadObjectAsync(_bucketName, imagePath, imageFile.ContentType, stream, cancellationToken: CancellationToken.None);

            if (blob is null)
            {
                throw new Exception("Upload failed");
            }

            var folderName = Path.GetDirectoryName(imagePath)?.Replace("\\", "/") ?? string.Empty;
            var imageName = Path.GetFileName(imagePath);

            return GetImageUrl(folderName, imageName);
        }
        public async Task<string> UploadImageAsync(IFormFile imageFile, string imagePath)
        {
            // Đặt giới hạn kích thước file (ví dụ 5MB = 5 * 1024 * 1024 bytes)
            long fileSizeLimit = 5 * 1024 * 1024;

            if (imageFile.Length > fileSizeLimit)
            {
                throw new Exception("Kích thước file vượt quá giới hạn cho phép (5MB).");
            }

            using var stream = new MemoryStream();
            await imageFile.CopyToAsync(stream);
            stream.Position = 0;
            var imageName =  Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var blob = await _storageClient.UploadObjectAsync(_bucketName, imagePath + imageName, imageFile.ContentType, stream, cancellationToken: CancellationToken.None);

            if (blob is null)
            {
                throw new Exception("Upload failed");
            }

            var folderName = Path.GetDirectoryName(imagePath)?.Replace("\\", "/") ?? string.Empty;

            return GetImageUrl(folderName, imageName);
        }

        public async Task<IFormFile> DownloadImageFromUrl(string imageUrl, string fileName, string contentType)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(imageUrl);
            if (!response.IsSuccessStatusCode) throw new Exception($"Failed to download image from URL: {imageUrl}");
            var fileBytes = await response.Content.ReadAsByteArrayAsync();
            return FormFileHelper.ToFormFile(fileBytes, fileName, contentType);
        }
    }
}
