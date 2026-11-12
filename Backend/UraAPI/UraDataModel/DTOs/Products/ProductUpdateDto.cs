using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using UraDataLayer.Enums;

namespace UraDataModel.Models.Products
{
    /// <summary>
    /// Cập nhật sản phẩm
    /// </summary>
    public class ProductUpdateDto
    {
        [Required(ErrorMessage = "Mã sản phẩm không được để trống.")]
        public string ProductId { get; set; } = default!;

        [Required(ErrorMessage = "Tên sản phẩm không được để trống.")]
        [StringLength(255)]
        public string ProductName { get; set; } = default!;

        [Required(ErrorMessage = "Loại sản phẩm không được để trống.")]
        public string CategoryId { get; set; } = default!;

        [Required(ErrorMessage = "Giá bán không được để trống.")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá bán không hợp lệ.")]
        public decimal SellPrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá vốn không hợp lệ.")]
        public decimal? CostPrice { get; set; }

        [EnumDataType(typeof(UnitTypeEnum))]
        public UnitTypeEnum Unit { get; set; }

        [StringLength(50)]
        public string? Sku { get; set; }

        [StringLength(100)]
        public string? QrCode { get; set; }

        [StringLength(2000)]
        public string? Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Trọng lượng không hợp lệ.")]
        public int? WeightGrams { get; set; }

        public bool IsActive { get; set; }

        public bool IsFeatured { get; set; }

        /// <summary>
        /// Chỉ gửi khi muốn đổi ảnh. Không gửi thì giữ ảnh cũ.
        /// </summary>
        public IFormFile? Image { get; set; }
    }
}