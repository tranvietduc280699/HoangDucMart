using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UraDataModel.DTOs.Auth;
using UraDataModel.DTOs.Users;
using UraDataModel.Model.Users;
using UraDataModel.Response;
using UraDataModel.Response.Users;

namespace UraBusinessLogic.Interfaces.Users
{
    public interface IUserBL : IBaseBL<User>
    {
        /// <summary>
        /// đăng ký tài khoản
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<ResultModel<UserResponseDto>> RegisterAsync(UserCreateDto user, string password);
        /// <summary>
        /// đăng nhập
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ResultModel<LoginResponse>> LoginUser(LoginRequest request);
        /// <summary>
        /// cập nhật avt
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        Task<ResultModel<string>> UpdateAvatarAsync(string userId, IFormFile file);
        /// <summary>
        /// cập nhật
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userId"></param>
        /// <returns></returns>

        Task<ResultModel<UserResponseDto>> UpdateAsync(UserUpdateDto user, string userId);

    };
}
