using UraDataModel.Model.Users;

namespace UraDataLayer.Interfaces.IUsers
{
    public interface IUserRoleDL : IBaseDL<UserRole>
    {
        /// <summary>
        /// lấy vai trò của đối tượng
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> GetRoleNamesByUserIdAsync(string userId);
    }

}
