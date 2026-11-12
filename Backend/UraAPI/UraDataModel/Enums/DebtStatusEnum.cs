using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UraDataModel.Enums
{
    public enum DebtStatusEnum
    {
        [Display(Name = "Chưa thanh toán")]
        Unpaid = 1,

        [Display(Name = "Thanh toán 1 phần")]
        Partial = 2,

        [Display(Name = "Đã thanh toán")]
        Paid = 3,

        [Display(Name = "Đã hủy")]
        Cancelled = 4
    }
}
