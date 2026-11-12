using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UraDataModel.Model.Users;
using UraDataModel.Models.Cloudinary;
using UraDataModel.Response;

namespace UraBusinessLogic.Interfaces.Cloudinary
{
    public interface ICloudinaryService
    {
        Task<string?> UploadImageAsync(IFormFile file, string publicId);
    }
}
