using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UraBusinessLogic.Interfaces.Users;
using UraDataLayer.DataLayer.Users;
using UraDataLayer.Interfaces.IUsers;
using UraDataModel.Model.Users;

namespace UraBusinessLogic.BusinessLogics.Users
{
    public class RoleBL : BaseBl<Role>, IRoleBL
    {
        public RoleBL(IRoleDL IRoleDL) : base(IRoleDL, "ura_roles")
        {
        }
    }
}
