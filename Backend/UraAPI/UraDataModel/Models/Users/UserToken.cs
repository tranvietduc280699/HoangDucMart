using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UraDataModel.Models.Users
{
    public class UserToken
    {
        public string TokenId { get ; set; }
        public string UserId { get; set; }
        public string TokenHash {  get; set; }
        // thời gian hết hạn
        public DateTime ExpiresAt { get; set; }
        // thu hồi / hủy bỏ
        public DateTime RevokedAt {  get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
