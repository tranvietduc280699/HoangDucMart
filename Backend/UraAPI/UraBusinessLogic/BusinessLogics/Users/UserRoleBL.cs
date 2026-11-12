using UraBusinessLogic.Interfaces.Users;
using UraDataLayer.DataLayer.Users;
using UraDataLayer.Interfaces.IUsers;
using UraDataModel.Model.Users;
using UraDataModel.Response;

namespace UraBusinessLogic.BusinessLogics.Users
{
    public class UserRoleBL : BaseBl<UserRole>, IUserRoleBL
    {
        public UserRoleBL(IUserRoleDL userRoleDL): base(userRoleDL, "ura_user_roles")
        {
        }
    }
}