using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UraBusinessLogic.Interfaces.ISales;
using UraDataLayer.Interfaces.IProducts;
using UraDataLayer.Interfaces.Sales;
using UraDataModel.Models.Sales;

namespace UraBusinessLogic.BusinessLogics.Sales
{
    public class OrderDetailBL : BaseBl<OrderDetail>,IOrderDetailBL
    {
        private readonly IProductDL _productDL;
        public OrderDetailBL(IOrderDetailDL orderDetailDL, IProductDL productDL) : base(orderDetailDL,"ura_order_detail")
        {
            _productDL=productDL;
        } 
      
    }
}
