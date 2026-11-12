using UraDataLayer.Enums;

namespace UraDataModel.Response.Sales
{
    public class OrderDetailResponseDto
    {
        public string OrderDetailId { get; set; }

        public string ProductId { get; set; }

        // Thông tin sản phẩm tại thời điểm bán
        public string ProductName { get; set; }

        public string? Sku { get; set; }
        // đơn vị
        
        public UnitTypeEnum? Unit { get; set; }
        // Thông tin giao dịch
        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal VatRate { get; set; }

        public decimal VatAmount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}