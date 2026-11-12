using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UraDataModel
{
    public class BaseEntity
    {
        /// <summary>
        ///  ngày tạo
        /// </summary>
        public DateTime? CreatedAt { get; set; } 
        /// <summary>
        /// id người tạo
        /// </summary>

        public string? CreatedBy { get; set; }
        /// <summary>
        /// thời gian cập nhật
        /// </summary>

        public DateTime? UpdatedAt { get; set; } 
        /// <summary>
        /// id người cập nhật
        /// </summary>

        public string? UpdatedBy { get; set; }
    }
}
