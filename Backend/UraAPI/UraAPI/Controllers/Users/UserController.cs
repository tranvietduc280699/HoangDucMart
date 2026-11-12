using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Security.Claims;
using UraBusinessLogic.Interfaces.Users;
using UraDataModel.DTOs.Auth;
using UraDataModel.DTOs.Users;
using UraDataModel.Model.Users;

namespace UraAPI.Controllers.Users
{
    [Controller]
    [Route("api/[Controller]")]
    [AllowAnonymous]
    public class UserController : BaseController<User>
    {
        private readonly IUserBL _userBL;

        public UserController(IUserBL baseBL) : base(baseBL)
        {
            _userBL = baseBL;
        }
       
        /// <summary>
        /// đăng ký người dùng
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost("InsertUser")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserCreateDto obj)
        {
            var result = await _userBL.RegisterAsync(obj, obj.Password);
            if (result == null)
            {
                return BadRequest(result);
            }
            else
            {
                return Ok(result);
            }
        }
        /// <summary>
        /// đăng ký tài khoản
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAssync([FromBody] LoginRequest request)
        {
            var result = await _userBL.LoginUser(request);
            if (result == null)
            {
                return BadRequest(result);
            }
            else
            {
                return Ok(result);
            }
        }
        /// <summary>
        /// cập nhật avata
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("{userId}/UpdateAvatarAsync")]
        public async Task<IActionResult> UpdateAvatarAsync([FromRoute] string userId, IFormFile file)
        {
            var result = await _userBL.UpdateAvatarAsync(userId, file);
            if (result == null)
            {
                return BadRequest(result);
            }
            else
            {
                return Ok(result);
            }
        }
        [HttpPost("UpdateAsync")]
        public async Task<IActionResult> UpdateAsync([FromBody] UserUpdateDto obj)
        {
            // lấy ID người request
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Không xác định được UserId.");

            var result = await _userBL.UpdateAsync(obj, userId);

            if (result == null)
                return BadRequest(result);

            return Ok(result);
        }


    }
}
