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
    public class RoleDL : BaseDL<Role>, IRoleDL
    {
        public RoleDL(MySqlContext context) : base(context) { }
    }
}
