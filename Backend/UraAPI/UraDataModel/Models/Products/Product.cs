using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UraDataLayer.Enums;

namespace UraDataModel.Models.Products
{
    /// <summary>
    /// sản phẩm
    /// </summary>
    public class Product :BaseEntity 
    {
        public string? ProductId { get; set; } 
        public string? CategoryId { get; set; }
        public string? QrCode { get; set; }
        public string? ImageUrl { get; set; }
        // mã hàng
        public string? Sku { get; set; } 
        public string? ProductName { get; set; } 
        public string? Description { get; set; }
        public UnitTypeEnum Unit { get; set; }
        public decimal SellPrice { get; set; }
        // Giá vốn nhập hàng
        public decimal? CostPrice { get; set; }
        //Trọng lượng của sản phẩm (gam)
        public int? WeightGrams { get; set; }

        public bool IsActive { get; set; }
        // Cờ đánh dấu sản phẩm nổi bật
        public bool IsFeatured { get; set; }

        // file ảnh : để lấy ảnh rồi lưu vào ImageUrl
        [NotMapped]
        public IFormFile? Image { get; set; } 
    }
}
