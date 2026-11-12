using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UraBusinessLogic.Interfaces.IProducts;
using UraDataModel.Models.Products;

namespace UraAPI.Controllers.Products
{
    [Controller]
    [Route("api/[Controller]")]
    [Authorize]
    public class CategoryController : BaseController<Category>
    {
        private readonly ICategoryBL _categoryBL;
        public CategoryController(ICategoryBL categoryBL) : base(categoryBL)
        {
            _categoryBL = categoryBL;
        }
    }
}
