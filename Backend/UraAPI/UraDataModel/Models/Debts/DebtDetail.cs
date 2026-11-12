using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UraDataLayer.Enums;

namespace UraDataModel.Models.Debts
{
    /// <summary>
    /// chi tiết sản phẩm, phát sinh nợ
    /// </summary>
    public class DebtDetail : BaseEntity
    {
        public string DebtDetailId { get; set; }

        public string DebtId { get; set; }

        public string? ProductId { get; set; }

        public string ProductName { get; set; }

        public string? Sku { get; set; }

        public UnitTypeEnum Unit { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public string? Note { get; set; }
    }
}
