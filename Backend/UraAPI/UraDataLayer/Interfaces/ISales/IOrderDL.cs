using UraDataModel.Models.Sales;

namespace UraDataLayer.Interfaces.Sales
{
    public interface IOrderDL :IBaseDL<Order>
    {
        /// <summary>
        /// thêm mới đơn hàng
        /// </summary>
        /// <param name="order"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        Task<Order> InsertOrder(Order order, List<OrderDetail> details);
        /// <summary>
        /// cập nhật đơn hàng
        /// </summary>
        /// <param name="order"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        Task<Order> UpdateOrder(Order order, List<OrderDetail> details);
    }
}
