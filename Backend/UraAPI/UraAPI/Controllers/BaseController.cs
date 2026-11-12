using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using UraBusinessLogic.Interfaces;
using UraDataModel.Models;

namespace UraAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class BaseController<T> : Controller
    {
        private readonly IBaseBL<T> _baseBL;
        public BaseController(IBaseBL<T> baseBL)
        {
            _baseBL = baseBL;
        }
        /// <summary>
        /// lấy tất cả bản ghi
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _baseBL.GetAll());
        }
        /// <summary>
        /// lấy bản ghi theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("filter")]
        public virtual async Task<IActionResult> GetByCondition([FromBody] BaseFilter filter)
        {
            var result = await _baseBL.GetByCondition(filter);
            return Ok(result);
        }
        /// <summary>
        /// xóa bản ghi
        /// </summary>
        /// <param name="id"> giá trị truyền vào id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(string id)
        {
            return Ok(await _baseBL.DeleteById(id));
        }
        /// <summary>
        /// thêm mới bản ghi
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost("Insert")]
        public async Task<IActionResult> Insert([FromBody] T obj)
        {
            var result = await _baseBL.Insert(obj);
            if (result == null || result != 0)
            {
                return BadRequest(result);
            }
            else
            {
                return Ok(result);
            }
        }
        /// <summary>
        /// cập nhật bản ghi
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] T obj)
        {
            var result = await _baseBL.Update(obj);
            if (result == null || result != 0)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }

        }
        /// <summary>
        /// cập nhật giá trị / 1 column
        /// </summary>
        /// <param name="propertyName"> tên cột ví dụ: UserName</param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost("UpdateColumn/{propertyName}")]
        public async Task<IActionResult> UpdateColumnAsync([FromRoute] string propertyName, [FromBody] T entity)
        {
            var result = await _baseBL.UpdateColumnAsync(propertyName, entity);
            if (result != null && (int)result > 0)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
