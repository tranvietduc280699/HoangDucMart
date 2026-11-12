using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UraDataLayer.Enums;

namespace UraDataModel.Response.Debts
{
    public class DebtDetailResponseDto
    {
        public string DebtDetailId { get; set; } = string.Empty;
        public string? ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? Sku { get; set; }
        public UnitTypeEnum Unit { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Note { get; set; }
    }
}
