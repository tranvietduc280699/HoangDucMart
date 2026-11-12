
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using UraBusinessLogic.Interfaces.Cloudinary;
using UraDataModel.Models.Cloudinary;
using UraDataModel.Response;

namespace UraBusinessLogic.BusinessLogics.Cloudinary
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly ICloudinary _cloudinary;

        public CloudinaryService(ICloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        } 
        /// <summary>
        /// cập nhật avata uesr
        /// </summary>
        /// <param name="file"></param>
        /// <param name="publicId"></param>
        /// <returns></returns>
        public async Task<string?> UploadImageAsync(IFormFile file, string publicId)
        {
            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                AssetFolder = "UserImages/Upload",
                PublicId = publicId,          
                Overwrite = true,
                Invalidate = true
            };
            var result = await _cloudinary.UploadAsync(uploadParams);
            return result.Error == null ? result.SecureUrl.ToString() : null;
        }
    }
}
