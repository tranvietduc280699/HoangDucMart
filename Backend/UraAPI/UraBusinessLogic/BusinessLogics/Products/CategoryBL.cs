using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UraBusinessLogic.Interfaces.IProducts;
using UraDataLayer.Interfaces;
using UraDataLayer.Interfaces.IProducts;
using UraDataModel.Models.Products;

namespace UraBusinessLogic.BusinessLogics.Products
{
    public class CategoryBL : BaseBl<Category>, ICategoryBL
    {
        public CategoryBL(ICategoryDL categoryDL) : base(categoryDL, "ura_categories")
        {
        }
    }
}
