using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UraDataModel.DTOs.Auth
{
    public class LoginResponse
    {
        /// <summary>
        /// JWT dùng để xác thực khi gọi API.
        /// </summary>
        public string? AccessToken { get; set; }

        /// <summary>
        /// Loại token, thông thường là Bearer.
        /// </summary>
        public string TokenType { get; set; } = "Bearer";

        /// <summary>
        /// Thời gian hết hạn của Access Token, tính bằng giây.
        /// Ví dụ: 900 giây = 15 phút.
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Thông tin người dùng sau khi đăng nhập.
        /// </summary>
        public LoginUserResponse User { get; set; }
    }
    public class LoginUserResponse
    {
        public string? UserId { get; set; }

        public string? UserName { get; set; }

        public string? FullName { get; set; }

        public List<string>? Roles { get; set; }
    }
}
