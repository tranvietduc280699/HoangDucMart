
using Dapper;
using System.Data.Common;
using UraDataLayer.Database;
using UraDataLayer.DataLayers;
using UraDataLayer.Interfaces.IUsers;
using UraDataModel.DTOs.Auth;
using UraDataModel.Model.Users;

namespace UraDataLayer.DataLayer.Users
{
    public class UserDL : BaseDL<User>, IUserDL
    {
        protected readonly MySqlContext _context;

        public UserDL(MySqlContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// đăng nhập tài khoản
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<User> LoginUser(LoginRequest request)
        {
            using var connect = _context.CreateConnectTion();
            string sql = $"SELECT * FROM ura_users WHERE USER_NAME = @userName;";
            var user = await connect.QueryFirstOrDefaultAsync<User>(sql, new { userName = request.UserName });
            if (user == null)
            {
                return null;
            }
            // kiểm tra MK
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            if (!isPasswordValid)
            {
                return null;
            }
            return user;
        }
       

    }
}
