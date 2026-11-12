using UraDataModel.DTOs.Sales;
using UraDataModel.Models.Sales;
using UraDataModel.Response;


namespace UraBusinessLogic.Interfaces.ISales
{
    public interface IOrderBL : IBaseBL<Order>
    {
        /// <summary>
        /// thêm mới đơn hàng
        /// </summary>
        /// <param name="order"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ResultModel<OrderResponseDto>> InsertOrder(OrderCreateDto order, string userId);
       /// <summary>
       /// cập nhật đơn hàng
       /// </summary>
       /// <param name="dto"></param>
       /// <param name="userId"></param>
       /// <returns></returns>
        Task<ResultModel<OrderResponseDto>> UpdateOrder(OrderUpdateDto dto, string userId);
    }
}
