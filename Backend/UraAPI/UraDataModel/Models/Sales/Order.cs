using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UraDataModel.Enums;

namespace UraDataModel.Models.Sales
{
    /// <summary>
    /// đơn đặt hàng
    /// </summary>
    public class Order : BaseEntity
    {
        public string OrderId { get; set; }
        
        public string? UserId { get; set; }
        // mã đơn hàng
        public string OrderCode { get; set; }
        // trạng thái đơn hàng
        public int OrderStatus { get; set; }
        // trạng thái thanh thanh toán
        public PaymentStatusEnum PaymentStatus { get; set; }
        // phương thức thanh toán
        public string PaymentMethod { get; set; }
        // tổng tiền

        public decimal TotalAmount { get; set; }
        // mã giảm giá
        public decimal? DiscountAmount { get; set; }
        // tổng thực tế
        public decimal FinalAmount { get; set; }
        // thông tin người nhận
        public string? ReceiverName { get; set; }
        public string? ReceiverPhone { get; set; }
        public string? ShippingAddress { get; set; }
        public string? Note { get; set; }

        // thuế
        public Decimal? VatRate { get; set; }
        public Decimal? VatAmount { get; set; }

    }
}
