using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UraBusinessLogic.Interfaces.ISales;
using UraDataModel.DTOs.Sales;
using UraDataModel.Models.Products;
using UraDataModel.Models.Sales;

namespace UraAPI.Controllers.Sales
{
    [Controller]
    [Route("api/[Controller]")]
    [Authorize]
    public class OrderController : BaseController<Order>
    {
        private readonly IOrderBL _orderBL;
        public OrderController(IOrderBL orderBL) :base(orderBL)
        {
            _orderBL= orderBL;
        }
        /// <summary>
        /// thêm mới đơn bán hàng
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost("InsertOrder")]
        public async Task<IActionResult> InsertOrder([FromBody] OrderCreateDto order)
        {
            // lấy id người dùng được xác thực
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Không xác định được UserId.");
            var result = await _orderBL.InsertOrder(order, userId);
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
        /// cập nhật đơn hàng
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderUpdateDto order)
        {
            // lấy id người dùng được xác thực
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Không xác định được UserId.");
            var result = await _orderBL.UpdateOrder(order, userId);
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
