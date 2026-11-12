using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UraDataModel.DTOs.Auth;
using UraDataModel.Model.Users;

namespace UraDataLayer.Interfaces.IUsers
{
    public interface IUserDL : IBaseDL<User>
    {
        Task<User> LoginUser(LoginRequest request);
    }
}
