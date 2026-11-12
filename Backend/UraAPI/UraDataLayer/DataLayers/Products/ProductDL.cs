using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UraDataLayer.Database;
using UraDataLayer.Interfaces.IProducts;
using UraDataModel.Models.Products;

namespace UraDataLayer.DataLayers.Products
{
    public class ProductDL : BaseDL<Product> ,IProductDL
    {
        public ProductDL(MySqlContext context) : base(context) { }
    }
}
