using UraDataModel.Enums;
using UraDataModel.Response.Sales;

namespace UraDataModel.DTOs.Sales
{
    public class OrderResponseDto :BaseEntity
    {
        public string OrderId { get; set; }

        public string OrderCode { get; set; }

        public string? UserId { get; set; }

        public PaymentStatusEnum PaymentStatus { get; set; }

        public string PaymentMethod { get; set; }

        // Tổng tiền hàng trước giảm giá và VAT
        public decimal SubTotal { get; set; }

        // Tiền giảm giá
        public decimal DiscountAmount { get; set; }

        // Thuế VAT
        public decimal VatAmount { get; set; }

        // Tổng tiền thanh toán
        public decimal TotalAmount { get; set; }
        // tổng hóa đơn
        public decimal FinalAmount { get; set; }

        public string? ReceiverName { get; set; }

        public string? ReceiverPhone { get; set; }

        public string? ShippingAddress { get; set; }

        public string? Note { get; set; }

        // Trạng thái đơn hàng
        public string OrderStatus { get; set; }

        public List<OrderDetailResponseDto> Items { get; set; } = new();
    }
}