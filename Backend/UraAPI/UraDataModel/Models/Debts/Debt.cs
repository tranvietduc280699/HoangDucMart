using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UraDataModel.Enums;

namespace UraDataModel.Models.Debts
{
    public class Debt : BaseEntity
    {
        public string DebtId { get; set; }

        // Đơn hàng phát sinh nợ (null nếu nợ trực tiếp)
        public string? OrderId { get; set; }

        public string? UserId { get; set; }

        // Thông tin người nợ tại thời điểm tạo công nợ
        public string DebtorName { get; set; }

        public string? DebtorPhone { get; set; }

        public string? DebtorAddress { get; set; }

        // 1: nợ từ đơn hàng, 2: nợ trực tiếp
        public int DebtType { get; set; }

        // Tổng tiền nợ
        public decimal TotalAmount { get; set; }

        // Đã trả
        public decimal PaidAmount { get; set; }

        // Còn lại
        public decimal RemainingAmount { get; set; }

        // 1: Chưa thanh toán, 2: Thanh toán một phần, 3: Đã thanh toán, 4: Đã hủy
        public DebtStatusEnum DebtStatus { get; set; }

        public string? Note { get; set; }
    }
}
