using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UraDataModel.Enums
{
    public enum PaymentStatusEnum
    {
        [Display (Name = "Chưa thanh toán")]
        Unpaid = 1,

        [Display(Name = "Đang chờ")]
        Pending = 2,

        [Display(Name = "Đã thanh toán")]
        Paid = 3,

        [Display(Name =  "Công nợ")]
        Debt = 4
    }
}
