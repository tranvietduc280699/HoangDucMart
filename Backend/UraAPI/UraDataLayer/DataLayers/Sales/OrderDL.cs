using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UraDataLayer.Database;
using UraDataLayer.Interfaces.Sales;
using UraDataModel.Models.Sales;

namespace UraDataLayer.DataLayers.Sales
{
    public class OrderDL : BaseDL<Order>, IOrderDL
    {
        private readonly MySqlContext _context;
        public OrderDL(MySqlContext context): base(context)
        {
            _context = context;
        }
        /// <summary>
        /// thêm mới vào order và orderDetail,, 1 đơn hàng và nhiều sản phẩm
        /// </summary>
        /// <param name="order"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        public async Task<Order> InsertOrder(Order order, List<OrderDetail> details)
        {
            var detailsJson = JsonSerializer.Serialize(details);

            var p = new DynamicParameters();
            p.Add("p_OrderId", order.OrderId);
            p.Add("p_UserId", order.UserId);
            p.Add("p_OrderCode", order.OrderCode);
            p.Add("p_OrderStatus", order.OrderStatus);
            p.Add("p_PaymentStatus", order.PaymentStatus);
            p.Add("p_PaymentMethod", order.PaymentMethod);
            p.Add("p_TotalAmount", order.TotalAmount);
            p.Add("p_DiscountAmount", order.DiscountAmount);
            p.Add("p_FinalAmount", order.FinalAmount);
            p.Add("p_ReceiverName", order.ReceiverName);
            p.Add("p_ReceiverPhone", order.ReceiverPhone);
            p.Add("p_ShippingAddress", order.ShippingAddress);
            p.Add("p_Note", order.Note);
            p.Add("p_VatRate", order.VatRate);
            p.Add("p_VatAmount", order.VatAmount);
            p.Add("p_CreatedAt", order.CreatedAt);
            p.Add("p_CreatedBy", order.CreatedBy);
            p.Add("p_DetailsJson", detailsJson);

            await _context.CreateConnectTion().ExecuteAsync("Proc_Order_Insert", p, commandType: CommandType.StoredProcedure);

            return order;
        }
        /// <summary>
        /// cập nhật đơn hàng
        /// </summary>
        /// <param name="order"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        public async Task<Order> UpdateOrder(Order order, List<OrderDetail> details)
        {
            var detailsJson = JsonSerializer.Serialize(details);

            var p = new DynamicParameters();
            p.Add("p_OrderId", order.OrderId);
            p.Add("p_UserId", order.UserId);
            p.Add("p_PaymentMethod", order.PaymentMethod);
            p.Add("p_TotalAmount", order.TotalAmount);
            p.Add("p_DiscountAmount", order.DiscountAmount);
            p.Add("p_FinalAmount", order.FinalAmount);
            p.Add("p_ReceiverName", order.ReceiverName);
            p.Add("p_ReceiverPhone", order.ReceiverPhone);
            p.Add("p_ShippingAddress", order.ShippingAddress);
            p.Add("p_Note", order.Note);
            p.Add("p_VatRate", order.VatRate);
            p.Add("p_VatAmount", order.VatAmount);
            p.Add("p_UpdatedAt", order.UpdatedAt);
            p.Add("p_UpdatedBy", order.UpdatedBy);
            p.Add("p_DetailsJson", detailsJson);

            await _context.CreateConnectTion().ExecuteAsync("Proc_Order_Update", p, commandType: CommandType.StoredProcedure);
            return order;
        }
    }
}
