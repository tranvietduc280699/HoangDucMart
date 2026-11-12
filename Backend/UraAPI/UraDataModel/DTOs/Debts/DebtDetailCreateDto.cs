using System.ComponentModel.DataAnnotations;

namespace UraDataModel.DTOs.Debts
{
    /// <summary>
    /// Một dòng sản phẩm khi tạo công nợ nhập trực tiếp
    /// </summary>
    public class DebtDetailCreateDto
    {
        [Required(ErrorMessage = "Mã sản phẩm không được để trống")]
        public string ProductId { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Số lượng không được âm")]
        public decimal? Quantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Trọng lượng không được âm")]
        public decimal? WeightGrams { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Số tiền không được âm")]
        public decimal? Amount { get; set; }

        public decimal? VatRate { get; set; }

        // Ghi chú từng dòng
        public string? Note { get; set; }
    }
}