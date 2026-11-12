using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UraDataModel.DTOs.Sales
{
    public class OrderDetailCreateDto
    {
        [Required(ErrorMessage = "Mã sản phẩm không được để trống")]
        public string ProductId { get; set; }
        // số lượng
        [Range(0, double.MaxValue, ErrorMessage = "Đơn giá không được âm")]
        public decimal? Quantity { get; set; }  // mua theo số lượng
        // giá tiền thay vì mua kg hoặc lạng
        [Range(0, double.MaxValue, ErrorMessage = "Đơn giá không được âm")]
        public decimal? Amount { get; set; }  // mua theo tiền
        public decimal? WeightGrams { get; set; } // mua theo gam

        [Range(0, double.MaxValue, ErrorMessage = "Đơn giá không được âm")]
        public decimal? UnitPrice { get; set; }

        // VAT 
        public decimal? VatRate { get; set; }
    }
}
