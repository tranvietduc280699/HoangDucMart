using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UraDataModel.Enums;

namespace UraDataModel.DTOs.Sales
{
    public class OrderCreateDto
    {
        public string? UserId { get; set; }

        public PaymentStatusEnum? PaymentStatus { get; set; }

        public string? PaymentMethod { get; set; } 
        // số lượng
        public decimal? DiscountAmount { get; set; } = 0;

        public decimal? VatRate { get; set; } = 0;

        public string? ReceiverName { get; set; }

        public string? ReceiverPhone { get; set; }

        public string? ShippingAddress { get; set; }

        public string? Note { get; set; }

        [Required(ErrorMessage = "Đơn hàng phải có ít nhất một sản phẩm")]
        public List<OrderDetailCreateDto> Items { get; set; } = new();
    }
}
