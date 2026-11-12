using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UraDataLayer.Enums;

namespace UraDataModel.Response.Products
{
    public class ProductResponseDto : BaseEntity
    {
        public string ProductId { get; set; } = default!;
        public string? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string ProductName { get; set; } = default!;
        public string? Sku { get; set; }
        public string? QrCode { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public UnitTypeEnum Unit { get; set; }
        public string UnitName { get; set; } = default!;
        public decimal SellPrice { get; set; }
        public decimal? CostPrice { get; set; }
        public int? WeightGrams { get; set; }
        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }
    }
}
