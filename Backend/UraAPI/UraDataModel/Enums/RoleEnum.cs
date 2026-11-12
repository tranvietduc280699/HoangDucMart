using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UraDataModel.Enums
{
    public enum RoleEnum
    {
        /// <summary>
        /// Quản trị viên cấp cao nhất - Toàn quyền hệ thống
        /// </summary>
        Admin = 1,

        /// <summary>
        /// Người quản lý / Quản trị phân hệ - Duyệt dữ liệu, xem báo cáo
        /// </summary>
        Manager = 2,

        /// <summary>
        /// Nhân viên vận hành - Thao tác nghiệp vụ cơ bản
        /// </summary>
        Staff = 3,

        /// <summary>
        /// Khách hàng / Người dùng thông thường
        /// </summary>
        Customer = 4
    }
}
