using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UraDataLayer.Database;
using UraDataLayer.Interfaces.Sales;
using UraDataModel.Models.Sales;

namespace UraDataLayer.DataLayers.Sales
{
    public class OrderDetailDL : BaseDL<OrderDetail>, IOrderDetailDL
    {
        public OrderDetailDL(MySqlContext context) :base(context) { }
    }
}
