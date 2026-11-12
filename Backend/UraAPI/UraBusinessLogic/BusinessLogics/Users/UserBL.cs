using Microsoft.AspNetCore.Http;
using MySqlX.XDevAPI.Relational;
using UraBusinessLogic.Interfaces.Cloudinary;
using UraBusinessLogic.Interfaces.Users;
using UraDataLayer.DataLayer.Users;
using UraDataLayer.Interfaces.IUsers;
using UraDataModel.DTOs.Auth;
using UraDataModel.DTOs.Users;
using UraDataModel.Enums;
using UraDataModel.Model.Users;
using UraDataModel.Models;
using UraDataModel.Models.Users;
using UraDataModel.Response;
using UraDataModel.Response.Users;

namespace UraBusinessLogic.BusinessLogics.Users
{
    public class UserBL : BaseBl<User>, IUserBL
    {
        private readonly IUserDL _userDL;
        private readonly IUserRoleDL _userRoleDL;
        //avata
        private readonly ICloudinaryService _cloudinaryService;
        
        public UserBL(IUserDL userDL, IUserRoleDL userRoleDL, ICloudinaryService cloudinaryService) : base(userDL, "ura_users")
        {
            _userDL = userDL;
            _userRoleDL = userRoleDL;
            _cloudinaryService = cloudinaryService;
        }

        /// <summary>
        /// đăng ký tài khoản
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns> 
        public async Task<ResultModel<UserResponseDto>> RegisterAsync(UserCreateDto user, string password)
        {
            // khởi tạo object tìm kiếm
            BaseFilter obj = new BaseFilter
            {
                Conditions = new Dictionary<string, string?>
                {
                    { "UserName", user.UserName },{"Email",user.Email},{"Phone",user.Phone}
                },
                LogicalOperator = "OR",
            };
            // Kiểm tra người dùng đã tồn tại chưa
            var exitUser = await _userDL.GetByConditionAsync(_tableName, obj);

            if(exitUser != null && exitUser.Any())
            {
                return ResultHelper.Fail<UserResponseDto>(ResponseCode.AlreadyExists);
            }
            // máp DTO vs model
            var userId = Guid.NewGuid().ToString();
            User userObj = new User
            {
                UserId = userId,
                UserName = user.UserName,
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password, 12),
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                Address = user.Address,
                AvatarUrl = "https://res.cloudinary.com/mkbccsy5/image/upload/default-avatar.jpg",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId

            };
          
            var result = await _userDL.InsertAsync(_tableName, userObj);
            if (result <= 0)
            {
                return ResultHelper.Fail<UserResponseDto>(ResponseCode.BadRequest);
            }
            // khởi tạo object
            UserRole userRole = new UserRole
            {
                UserId = userId,
                RoleId = (int)RoleEnum.Customer
            };
            // thêm mới vào bảng UserRole
            var roleResult = await _userRoleDL.InsertAsync("ura_user_roles", userRole);

            if (roleResult <= 0)
            {
                return ResultHelper.Fail<UserResponseDto>(ResponseCode.BadRequest);
            }
            // map entity -> response
            var response = new UserResponseDto
            {
                UserId = userObj.UserId,
                UserName = userObj.UserName,
                FullName = userObj.FullName,
                Email = userObj.Email,
                Phone = userObj.Phone,
                DateOfBirth = userObj.DateOfBirth,
                Gender = userObj.Gender,
                Address = userObj.Address,
                AvatarUrl = userObj.AvatarUrl,
                IsActive = userObj.IsActive,
                LastLoginAt = userObj.LastLoginAt
            };

            return ResultHelper.Success(response);
        }

        /// <summary>
        /// đăng nhập
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResultModel<LoginResponse>> LoginUser(LoginRequest request)
        {
            var user = await _userDL.LoginUser(request);
            if (user == null)
            {
                return ResultHelper.Fail<LoginResponse>(ResponseCode.NotFound);
            }
            // tạo token
            var token = Convert.ToBase64String(System.Security.Cryptography.RandomNumberGenerator.GetBytes(32));

            // gọi hàm insert URA_TOKENS
            var userToken = new UserToken
            {
                TokenId = Guid.NewGuid().ToString(),
                UserId = user.UserId,
                TokenHash = token,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };
            var isInsert = await _userDL.InsertAsync("URA_TOKENS", userToken);
            if (isInsert <= 0)
            {
                return ResultHelper.Fail<LoginResponse>(ResponseCode.BadRequest);
            }
            // cập nhật trạng thái đăng nhập cuối cùng bảng user
            user.LastLoginAt = DateTime.UtcNow;
            var updateUser = await _userDL.UpdateColumnAsync(_tableName,"LastLoginAt", user);


            // lấy vài trò của đối tượng
            var role = await _userRoleDL.GetRoleNamesByUserIdAsync(user.UserId);

            // dữ liệu trả về
            var loginResponse = new LoginResponse
            {
                User = new LoginUserResponse()
                {
                    UserId = user.UserId,
                    Roles = role.ToList(),
                    UserName = user.UserName,
                    FullName = user.FullName
                },
                AccessToken = token,
                ExpiresIn = 9000
            };

            return ResultHelper.Success(loginResponse);
        }

        /// <summary>
        /// cập nhật avata user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<ResultModel<string>> UpdateAvatarAsync(string userId, IFormFile file)
        {

            // Validate file
            if (file == null || file.Length == 0)
                return ResultHelper.Fail<string>(ResponseCode.BadRequest);

            var allowed = new[] { "image/jpeg", "image/png", "image/webp" };
            if (!allowed.Contains(file.ContentType))
                return ResultHelper.Fail<string>(ResponseCode.BadRequest);

            if (file.Length > 5 * 1024 * 1024)
                return ResultHelper.Fail<string>(ResponseCode.BadRequest);

            // Lấy user trước khi upload
            BaseFilter condition = new BaseFilter
            {
                Conditions = new Dictionary<string, string?>
                {
                    { "UserId", userId }
                },
                LogicalOperator =""
            };
            var users = await _userDL.GetByConditionAsync(_tableName, condition);

            var user = users?.FirstOrDefault();
            if (user == null)
                return ResultHelper.Fail<string>(ResponseCode.NotFound);

            // Upload lên Cloudinary
            var url = await _cloudinaryService.UploadImageAsync(file,userId);
            if (url == null)
                return ResultHelper.Fail<string>(ResponseCode.BadGateway);

            // Lưu URL vào DB
            user.AvatarUrl = url;
            var affected = await _userDL.UpdateColumnAsync(_tableName, nameof(User.AvatarUrl), user);
            if (affected == 0)
                return ResultHelper.Fail<string>(ResponseCode.InternalServerError);

            return ResultHelper.Success(url);
        }
        /// <summary>
        /// cập nhật user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ResultModel<UserResponseDto>> UpdateAsync(UserUpdateDto user, string userId)
        {
            if (user.UserId != userId)
                return ResultHelper.Fail<UserResponseDto>(ResponseCode.Forbidden);
            // Lấy user hiện có trong DB
            var condition = new BaseFilter
            {
                Conditions = new Dictionary<string, string?>
                {
                    { "UserId", user.UserId }
                },
                LogicalOperator = ""
            };

            var result = await _userDL.GetByConditionAsync(_tableName, condition);
            var existing = result?.FirstOrDefault();   
            if (existing == null)
            {
                return ResultHelper.Fail<UserResponseDto>(ResponseCode.NotFound);
            }
            // map
            existing.UserName = user.UserName;
            existing.FullName = user.FullName;
            existing.Email = user.Email;
            existing.Phone = user.Phone!;
            existing.DateOfBirth = user.DateOfBirth;
            existing.Gender = user.Gender!;
            existing.Address = user.Address;
            existing.UpdatedAt = DateTime.UtcNow;
            existing.UpdatedBy = userId;

            var affected = await base.Update(existing);
            if (affected <= 0)
            {
                return ResultHelper.Fail<UserResponseDto>(ResponseCode.BadRequest);
            }
            return ResultHelper.Success(new UserResponseDto
            {
                UserId = existing.UserId,
                UserName = existing.UserName,
                FullName = existing.FullName,
                Email = existing.Email,
                Phone = existing.Phone,
                DateOfBirth = existing.DateOfBirth,
                Gender = existing.Gender,
                Address = existing.Address,
                AvatarUrl = existing.AvatarUrl,
                IsActive = existing.IsActive,
                LastLoginAt = existing.LastLoginAt
            });
        }
    }
}
