using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UraBusinessLogic.Interfaces.IDebts;
using UraDataModel.DTOs.Debts;
using UraDataModel.DTOs.Sales;
using UraDataModel.Models.Debts;

namespace UraAPI.Controllers.Debts
{
    [Controller]
    [Route("api/[Controller]")]
    [Authorize]
    public class DebtController : Controller
    {
        private readonly IDebtBL _debtBL;
        public DebtController(IDebtBL debtBL)
        {
            _debtBL = debtBL;
        }
        /// <summary>
        /// thêm mới công nợ
        /// </summary>
        /// <param name="debt"></param>
        /// <returns></returns>
        [HttpPost("InsertDebt")]
        public async Task<IActionResult> InsertDebt([FromBody] DebtCreateDto debt)
        {
            // lấy id người dùng được xác thực
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Không xác định được UserId.");
            var result = await _debtBL.InsertDebt(debt, userId);
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
        /// cập nhật trạng thái công nợ
        /// </summary>
        /// <param name="debt"></param>
        /// <returns></returns>
        [HttpPost("UpdateDebt")]
        public async Task<IActionResult> UpdateDebt([FromBody] Debt debt)
        {
            // lấy id người dùng được xác thực
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Không xác định được UserId.");
            var result = await _debtBL.UpdateStatus(debt, userId);
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
