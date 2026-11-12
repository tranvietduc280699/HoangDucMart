using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UraDataLayer.Database;
using UraDataLayer.DataLayers;
using UraDataLayer.Interfaces.IUsers;
using UraDataModel.Model.Users;

namespace UraDataLayer.DataLayer.Users
{
   public class UserRoleDL : BaseDL<UserRole>, IUserRoleDL
    {
        protected readonly MySqlContext _context;
        public UserRoleDL(MySqlContext context) : base(context)
        {
            _context = context;
        }
        /// <summary>
        ///  lấy vai trò của đối tượng
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetRoleNamesByUserIdAsync(string userId)
        {
            using var connect = _context.CreateConnectTion();
            var sqlQueryRole = @"SELECT r.ROLE_NAME 
                        FROM URA_USER_ROLES ur 
                        JOIN URA_ROLES r ON ur.ROLE_ID = r.ROLE_ID 
                        WHERE ur.USER_ID = @UserId";

            // Gọi thực thi câu lệnh SQL (tùy vào Dapper hoặc ORM bạn đang dùng trong dự án)
            return await connect.QueryAsync<string>(sqlQueryRole, new { UserId = userId });
        }
    }

}
