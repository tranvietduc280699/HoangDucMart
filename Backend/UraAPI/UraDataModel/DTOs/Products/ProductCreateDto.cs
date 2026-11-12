
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using UraDataLayer.Enums;

namespace UraDataModel.Models.Products
{
    /// <summary>
    ///  thêm mới sản phẩm
    /// </summary>
    public class ProductCreateDto
    {
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

        [Required(ErrorMessage = "Đơn vị tính không được để trống.")]
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
        /// File ảnh, server upload lên Cloudinary rồi lưu URL vào ImageUrl
        /// </summary>
        public IFormFile? Image { get; set; }
    }
}
