using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace MyGoldenFood.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<string?> UploadImageAsync(IFormFile imageFile, string folder)
        {
            try
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(imageFile.FileName, imageFile.OpenReadStream()),
                    Folder = folder,
                    Transformation = new Transformation().Width(500).Height(500).Crop("fit").Quality("auto:good")
                };
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                return uploadResult.Error == null ? uploadResult.SecureUrl.ToString() : null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> DeleteImageAsync(string imagePath)
        {
            try
            {
                var publicId = imagePath.Replace("https://res.cloudinary.com/dbhogeepn/image/upload/", "").Split('.')[0];
                var deletionParams = new DeletionParams(publicId);
                var deletionResult = await _cloudinary.DestroyAsync(deletionParams);

                return deletionResult.Result == "ok";
            }
            catch
            {
                return false;
            }
        }
    }
}
