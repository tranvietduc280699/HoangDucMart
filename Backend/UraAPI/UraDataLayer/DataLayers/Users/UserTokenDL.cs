using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UraDataLayer.Database;
using UraDataLayer.Interfaces.IUsers;
using UraDataModel.Models.Users;

namespace UraDataLayer.DataLayers.Users
{
    public class UserTokenDL : BaseDL<UserToken>, IUserTokenDL
    {
        public UserTokenDL(MySqlContext context) : base(context) { }
    }
}
