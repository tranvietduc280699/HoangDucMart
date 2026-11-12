using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UraDataModel.DTOs.Debts
{
    public class DebtCreateDto
    {
        // Có OrderId: nợ từ đơn hàng. Không có: nợ trực tiếp
        public string? OrderId { get; set; }

        // Khách có tài khoản (phải tồn tại trong URA_USERS), có thể null
        public string? UserId { get; set; }

        // Thông tin người nợ
        [MaxLength(150)]
        public string? DebtorName { get; set; }

        [MaxLength(20)]
        public string? DebtorPhone { get; set; }

        public string? DebtorAddress { get; set; }
        public decimal? DiscountAmount { get; set; } = 0;
        public decimal? VatRate { get; set; } = 0;

        // Số tiền khách trả ngay lúc ghi nợ (nếu có), mặc định 0
        [Range(0, double.MaxValue, ErrorMessage = "Số tiền đã trả không được âm")]
        public decimal? PaidAmount { get; set; } = 0;

        public string? Note { get; set; }

        // Chỉ dùng cho nợ trực tiếp (không có OrderId)
        public List<DebtDetailCreateDto> Items { get; set; } = new();
    }
}
