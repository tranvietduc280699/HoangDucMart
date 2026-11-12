using UraBusinessLogic.Interfaces.IUsers;
using UraDataLayer.Interfaces.IUsers;
using UraDataModel.Models;
using UraDataModel.Models.Users;

namespace UraBusinessLogic.BusinessLogics.Users
{
    public class UserTokenBL :BaseBl<UserToken>, IUserTokenBL
    {
        private readonly IUserTokenDL _userTokenDL;
        public UserTokenBL(IUserTokenDL userTokenDL) :base(userTokenDL, "ura_tokens")
        {
            _userTokenDL = userTokenDL;
        }
        /// <summary>
        /// lấy bản ghi
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<UserToken> GetByToken(string token)
        {
            //var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
            // tìm kiếm trong db
            var condition = new BaseFilter
            {
                Conditions = new Dictionary<string, string?> { { "TokenHash", token } },
                LogicalOperator = ""
            };
            var tokens = await _userTokenDL.GetByConditionAsync(_tableName, condition);
            var userToken = tokens.FirstOrDefault();
            // kiểm tra thời hạn
            if (userToken == null || userToken.ExpiresAt < DateTime.UtcNow)
                return null;
            return userToken;
        }
        
    }
    
}
