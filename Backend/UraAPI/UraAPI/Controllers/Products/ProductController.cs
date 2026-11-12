using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using System.Security.Claims;
using UraBusinessLogic.Interfaces.IProducts;
using UraDataModel.Models.Products;

namespace UraAPI.Controllers.Products
{
    [Controller]
    [Route("api/[Controller]")]
    [Authorize]

    public class ProductController : BaseController<Product>
    {
        private readonly IProductBL _productBL;
        public ProductController(IProductBL productBL) : base(productBL)
        {
            _productBL = productBL;
        }
        /// <summary>
        /// thêm mới sản phẩm
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost("InsertProduct")]
        public async Task<IActionResult> InsertProductAsync([FromForm] ProductCreateDto product)
        {
            // lấy id người dùng được xác thực
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Không xác định được UserId.");
            var result = await _productBL.InsertProductAsync(product, userId);
            if (result == null)
            {
                return BadRequest(result);
            }
            else
            {
                return Ok(result);
            }
        }
        /// <summary>
        /// cập nhật sản phẩm
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProductAsync([FromForm] ProductUpdateDto product)
        {
            // lấy id người dùng được xác thực
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Không xác định được UserId.");

            var result = await _productBL.UpdateProductAsync(product, userId);
            if (result == null)
            {
                return BadRequest(result);
            }
            else
            {
                return Ok(result);
            }
        }

    }
}
