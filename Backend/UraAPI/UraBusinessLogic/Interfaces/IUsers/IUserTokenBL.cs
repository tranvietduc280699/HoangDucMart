using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UraDataModel.Models.Users;

namespace UraBusinessLogic.Interfaces.IUsers
{
    public interface IUserTokenBL :IBaseBL<UserToken>
    {
        /// <summary>
        /// lấy bản ghi 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<UserToken> GetByToken(string token);
    }
}
