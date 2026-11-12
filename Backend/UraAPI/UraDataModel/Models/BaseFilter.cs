using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UraDataModel.Models
{
    public class BaseFilter
    {
        //key - object
        public Dictionary<string, string?> Conditions { get; set; } = new();
        // field kiểm tra điều kiện OR trong db
        public string? LogicalOperator { get; set; }
        // thêm điều kiện lặp khác
    }
}
