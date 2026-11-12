using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UraDataLayer.Enums;

namespace UraDataModel.Models.Sales
{
    /// <summary>
    /// chi tiết đơn hàng
    /// </summary>
    public class OrderDetail : BaseEntity
    {
        public string OrderDetailId { get; set; }
        public string OrderId { get; set; }
        public string ProductId { get; set; }

        public string ProductName { get; set; }
        // mã sp
        public string Sku { get; set; }
        public UnitTypeEnum Unit { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        // thuế
        public Decimal? VatRate {  get; set; }
        public Decimal? VatAmount { get; set; }

    }
}
