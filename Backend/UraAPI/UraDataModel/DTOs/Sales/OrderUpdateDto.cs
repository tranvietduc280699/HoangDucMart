using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UraDataModel.DTOs.Sales
{
    public class OrderUpdateDto : OrderCreateDto
    {
        [Required(ErrorMessage = "Mã đơn hàng không được để trống")]
        public string OrderId { get; set; } = string.Empty;
    }
}
