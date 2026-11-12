using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UraDataModel.Response.Debts
{
    public class DebtResponseDto
    {
        public string DebtId { get; set; } = string.Empty;

        // Có giá trị nếu nợ từ đơn hàng
        public string? OrderId { get; set; }
        public string? OrderCode { get; set; }

        public string? UserId { get; set; }

        // Thông tin người nợ
        public string DebtorName { get; set; } = string.Empty;
        public string DebtorPhone { get; set; } = string.Empty;
        public string? DebtorAddress { get; set; }

        // 1: nợ từ đơn hàng, 2: nợ trực tiếp
        public int DebtType { get; set; }
        public string DebtTypeText { get; set; } = string.Empty;

        // Tiền
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }

        // Trạng thái
        public int DebtStatus { get; set; }
        public string DebtStatusText { get; set; } = string.Empty;

        public string? Note { get; set; }

        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }

        public List<DebtDetailResponseDto> Items { get; set; } = new();
    }
}
