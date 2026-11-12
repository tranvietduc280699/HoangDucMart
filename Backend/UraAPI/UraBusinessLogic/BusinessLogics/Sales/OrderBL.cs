using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UraBusinessLogic.Interfaces.ISales;
using UraDataLayer.Database;
using UraDataLayer.DataLayer.Users;
using UraDataLayer.Interfaces.IProducts;
using UraDataLayer.Interfaces.IUsers;
using UraDataLayer.Interfaces.Sales;
using UraDataModel.DTOs.Sales;
using UraDataModel.Enums;
using UraDataModel.Models;
using UraDataModel.Models.Sales;
using UraDataModel.Response;
using UraDataModel.Response.Sales;

namespace UraBusinessLogic.BusinessLogics.Sales
{
    public class OrderBL : BaseBl<Order>, IOrderBL
    {
        private readonly IOrderDL _orderDL;
        private readonly IProductDL _productDL;
        private readonly IUserDL _userDL;
        public OrderBL(IOrderDL orderDL, IProductDL productDL, IUserDL userDL) :base(orderDL, "ura_orders")
        {
            _orderDL = orderDL;
            _productDL = productDL;
            _userDL = userDL;
        }
        /// <summary>
        /// thêm mưới đơn hàng
        /// </summary>
        /// <param name="order"></param>
        /// <param name="userId"></param> 
        /// <returns></returns>
        public async Task<ResultModel<OrderResponseDto>> InsertOrder(OrderCreateDto order, string userId)
        {
            // Kiểm tra dữ liệu đầu vào
            if (order.Items == null || !order.Items.Any())
            {
                return ResultHelper.Fail<OrderResponseDto>(ResponseCode.BadRequest);
            }
            // phải có 1 trong 3 điều kiện: tiền, số lượng, trọng lượng
            if (order.Items.Any(x => (x.Quantity ?? 0) <= 0 && (x.Amount ?? 0) <= 0 && (x.WeightGrams ?? 0) <= 0))
                return ResultHelper.Fail<OrderResponseDto>(ResponseCode.BadRequest);

            // Xác định loại đơn hàng
            bool isPosOrder = true; // luồng admin bán tại quầy
            string orderId = Guid.NewGuid().ToString();
            
            // nếu sản phẩm trùng nhau thì gộp lại
            var items = order.Items.GroupBy(x => x.ProductId).Select(y => new {
                ProductId = y.Key,
                Quantity = y.Sum(x => x.Quantity ?? 0),
                WeightGrams = y.Sum(x => x.WeightGrams ?? 0),
                Amount = y.Sum(x => x.Amount ?? 0),
                y.First().VatRate

            }).ToList();

            //Khởi tạo danh sách chi tiết đơn hàng
            var orderDetails = new List<OrderDetail>();
            decimal subTotal = 0;
            decimal vatAmount = 0;
            foreach(var item in items)
            {
                var filter = new BaseFilter
                {
                    Conditions = new Dictionary<string, string?> { { "ProductId", item.ProductId } },
                    LogicalOperator = ""
                };
                var products = await _productDL.GetByConditionAsync("ura_products",filter);
                var product = products?.FirstOrDefault();

                if (product == null)  return ResultHelper.Fail<OrderResponseDto>(ResponseCode.NotFound);

                decimal unitPrice = product.SellPrice;
                if (unitPrice <= 0)
                    return ResultHelper.Fail<OrderResponseDto>(ResponseCode.BadRequest);
                decimal quantity = item.Quantity;
                decimal detailTotal;
                decimal detailVatRate = item.VatRate ?? order.VatRate ?? 0;
                // nếu nhập tiền thay vì kg hoặc lạng
                if (item.Amount > 0)
                {
                    detailTotal = item.Amount;
                    quantity = Math.Round(item.Amount / unitPrice, 3);
                }
                else
                {
                    if (item.WeightGrams > 0)
                    {
                        // mua theo gam
                        if (product.WeightGrams <= 0)
                            return ResultHelper.Fail<OrderResponseDto>(ResponseCode.BadRequest);
                        decimal productWeight = product.WeightGrams ?? 0;
                        quantity = Math.Round(item.WeightGrams / productWeight, 3);
                    }
                    detailTotal = Math.Round(quantity * unitPrice, 0);
                }
                // vat
                decimal detailVatAmount = Math.Round(detailTotal * detailVatRate / 100m, 0); // làm tròn

                subTotal += detailTotal;
                // vat
                vatAmount += detailVatAmount;

                // thêm sản phẩm vào items
                orderDetails.Add(new OrderDetail
                {
                    OrderDetailId = Guid.NewGuid().ToString(),
                    OrderId = orderId,
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Quantity = quantity,
                    UnitPrice = unitPrice,
                    VatRate = detailVatRate,
                    TotalPrice = detailTotal,
                    Sku = product.Sku,
                    Unit = product.Unit,
                    VatAmount = detailVatAmount,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId
                });
            }
            // kiểm tra tiền giảm giá xem có ko
            decimal discountAmount = order.DiscountAmount ?? 0;
            if (discountAmount < 0 || discountAmount > subTotal)
                return ResultHelper.Fail<OrderResponseDto>(ResponseCode.BadRequest);

            // tổng order
            decimal finalAmount = subTotal - discountAmount + vatAmount;

            // dữ liệu request order
            var orderObj = new Order
            {
                OrderId = orderId,
                UserId = string.IsNullOrEmpty(order.UserId) ? null : order.UserId,
                OrderCode = GenerateOrderCode(isPosOrder),
                OrderStatus = 1,
                PaymentStatus =(PaymentStatusEnum)2,
                PaymentMethod = string.IsNullOrEmpty(order.PaymentMethod) ? "Cash" : order.PaymentMethod,

                TotalAmount = subTotal,
                DiscountAmount = discountAmount,
                VatRate = order.VatRate,
                VatAmount = vatAmount,
                FinalAmount = finalAmount,
                // thông tin khách hàng
                ReceiverName = string.IsNullOrWhiteSpace(order.ReceiverName) ? null : order.ReceiverName,
                ReceiverPhone = string.IsNullOrWhiteSpace(order.ReceiverPhone) ? null : order.ReceiverPhone,
                ShippingAddress = string.IsNullOrWhiteSpace(order.ShippingAddress) ? null : order.ShippingAddress,
                Note = order.Note,

                CreatedAt = DateTime.Now,
                CreatedBy = userId,

            };
            // nếu có user id (người đăng nhập hoặc có công nợ)
            if(!string.IsNullOrEmpty(order.UserId))
            {
                var filter = new BaseFilter
                {
                    Conditions = new Dictionary<string, string?> { { "UserId", order.UserId } },
                    LogicalOperator = ""
                };
                var users = await _userDL.GetByConditionAsync("ura_users", filter);
                var user = users.FirstOrDefault();
                // máp thông tin
                orderObj.ReceiverName = user.UserName;
                orderObj.ReceiverPhone = user.Phone;
                orderObj.ShippingAddress = user.Address;

            }
            await _orderDL.InsertOrder(orderObj, orderDetails);

            // dữ liệu trả về
            OrderResponseDto orderRespon = new OrderResponseDto
            {
                OrderId = orderObj.OrderId,
                OrderCode = orderObj.OrderCode,
                UserId = orderObj.UserId,
                OrderStatus = orderObj.OrderStatus.ToString(),
                PaymentStatus = orderObj.PaymentStatus,
                PaymentMethod = orderObj.PaymentMethod,

                TotalAmount = orderObj.TotalAmount,
                DiscountAmount = orderObj.DiscountAmount ?? 0,
                VatAmount = orderObj.VatAmount ?? 0,
                // tổng hóa đơn
                FinalAmount = orderObj.FinalAmount,

                ReceiverName = orderObj.ReceiverName,
                ReceiverPhone = orderObj.ReceiverPhone,
                ShippingAddress = orderObj.ShippingAddress,
                Note = orderObj.Note,
                CreatedAt = orderObj.CreatedAt,

                Items = orderDetails.Select(d => new OrderDetailResponseDto
                {
                    OrderDetailId = d.OrderDetailId,
                    ProductId = d.ProductId,
                    ProductName = d.ProductName,
                    Sku = d.Sku,
                    Unit = d.Unit,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice,
                    VatRate = d.VatRate ?? 0,
                    VatAmount = d.VatAmount ?? 0,
                    TotalPrice = d.TotalPrice
                }).ToList()
            };

            return ResultHelper.Success<OrderResponseDto>(orderRespon);
        }
        /// <summary>
        ///  cập nhật đơn hàng
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResultModel<OrderResponseDto>> UpdateOrder(OrderUpdateDto dto, string userId)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.OrderId))
                return ResultHelper.Fail<OrderResponseDto>(ResponseCode.BadRequest);

            if (dto.Items == null || !dto.Items.Any())
                return ResultHelper.Fail<OrderResponseDto>(ResponseCode.BadRequest);

            if (dto.Items.Any(x => (x.Quantity ?? 0) <= 0 && (x.Amount ?? 0) <= 0 && (x.WeightGrams ?? 0) <= 0))
                return ResultHelper.Fail<OrderResponseDto>(ResponseCode.BadRequest);

            // Lấy đơn hiện tại
            var orders = await _orderDL.GetByConditionAsync("ura_orders", new BaseFilter
            {
                Conditions = new Dictionary<string, string?> { { "OrderId", dto.OrderId } },
                LogicalOperator = ""
            });
            var orderObj = orders?.FirstOrDefault();
            if (orderObj == null)
                return ResultHelper.Fail<OrderResponseDto>(ResponseCode.NotFound);

            // Chỉ sửa khi đang chờ
            if (orderObj.PaymentStatus != PaymentStatusEnum.Pending)
                return ResultHelper.Fail<OrderResponseDto>(ResponseCode.BadRequest);

            // Gộp sản phẩm trùng
            var items = dto.Items.GroupBy(x => x.ProductId).Select(y => new
            {
                ProductId = y.Key,
                Quantity = y.Sum(x => x.Quantity ?? 0),
                WeightGrams = y.Sum(x => x.WeightGrams ?? 0),
                Amount = y.Sum(x => x.Amount ?? 0),
                y.First().VatRate
            }).ToList();

            // Tính lại 
            var orderDetails = new List<OrderDetail>();
            decimal subTotal = 0;
            decimal vatAmount = 0;

            foreach (var item in items)
            {
                var products = await _productDL.GetByConditionAsync("ura_products", new BaseFilter
                {
                    Conditions = new Dictionary<string, string?> { { "ProductId", item.ProductId } },
                    LogicalOperator = ""
                });
                var product = products?.FirstOrDefault();
                if (product == null)
                    return ResultHelper.Fail<OrderResponseDto>(ResponseCode.NotFound);

                decimal unitPrice = product.SellPrice;
                if (unitPrice <= 0)
                    return ResultHelper.Fail<OrderResponseDto>(ResponseCode.BadRequest);

                decimal quantity = item.Quantity;
                decimal detailTotal;
                decimal detailVatRate = item.VatRate ?? dto.VatRate ?? 0;

                if (item.Amount > 0)
                {
                    detailTotal = item.Amount;
                    quantity = Math.Round(item.Amount / unitPrice, 3);
                }
                else
                {
                    if (item.WeightGrams > 0)
                    {
                        decimal productWeight = product.WeightGrams ?? 0;
                        if (productWeight <= 0)
                            return ResultHelper.Fail<OrderResponseDto>(ResponseCode.BadRequest);
                        quantity = Math.Round(item.WeightGrams / productWeight, 3);
                    }
                    detailTotal = Math.Round(quantity * unitPrice, 0);
                }

                decimal detailVatAmount = Math.Round(detailTotal * detailVatRate / 100m, 0);
                subTotal += detailTotal;
                vatAmount += detailVatAmount;

                orderDetails.Add(new OrderDetail
                {
                    OrderDetailId = Guid.NewGuid().ToString(),
                    OrderId = orderObj.OrderId,
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Sku = product.Sku,
                    Unit = product.Unit,
                    Quantity = quantity,
                    UnitPrice = unitPrice,
                    VatRate = detailVatRate,
                    VatAmount = detailVatAmount,
                    TotalPrice = detailTotal,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId
                });
            }

            decimal discountAmount = dto.DiscountAmount ?? 0;
            if (discountAmount < 0 || discountAmount > subTotal)
                return ResultHelper.Fail<OrderResponseDto>(ResponseCode.BadRequest);

            // Cập nhật các field của đơn (OrderId, OrderCode, CreatedAt/By, trạng thái giữ nguyên)
            orderObj.UserId = string.IsNullOrWhiteSpace(dto.UserId) ? orderObj.UserId : dto.UserId;
            orderObj.PaymentMethod = string.IsNullOrEmpty(dto.PaymentMethod) ? orderObj.PaymentMethod : dto.PaymentMethod;

            orderObj.TotalAmount = subTotal;
            orderObj.DiscountAmount = discountAmount;
            orderObj.VatRate = dto.VatRate;
            orderObj.VatAmount = vatAmount;
            orderObj.FinalAmount = subTotal - discountAmount + vatAmount;

            orderObj.ReceiverName = string.IsNullOrWhiteSpace(dto.ReceiverName) ? null : dto.ReceiverName;
            orderObj.ReceiverPhone = string.IsNullOrWhiteSpace(dto.ReceiverPhone) ? null : dto.ReceiverPhone;
            orderObj.ShippingAddress = string.IsNullOrWhiteSpace(dto.ShippingAddress) ? null : dto.ShippingAddress;
            orderObj.Note = dto.Note;

            // Có tài khoản khách: chỉ điền chỗ còn trống
            if (!string.IsNullOrWhiteSpace(orderObj.UserId))
            {
                var users = await _userDL.GetByConditionAsync("ura_users", new BaseFilter
                {
                    Conditions = new Dictionary<string, string?> { { "UserId", orderObj.UserId } },
                    LogicalOperator = ""
                });
                var user = users?.FirstOrDefault();
                if (user == null)
                    return ResultHelper.Fail<OrderResponseDto>(ResponseCode.NotFound);

                if (string.IsNullOrWhiteSpace(orderObj.ReceiverName)) orderObj.ReceiverName = user.UserName;
                if (string.IsNullOrWhiteSpace(orderObj.ReceiverPhone)) orderObj.ReceiverPhone = user.Phone;
                if (string.IsNullOrWhiteSpace(orderObj.ShippingAddress)) orderObj.ShippingAddress = user.Address;
            }

            orderObj.UpdatedAt = DateTime.UtcNow;
            orderObj.UpdatedBy = userId;

            //  Lưu 
            await _orderDL.UpdateOrder(orderObj, orderDetails);

            //Trả dữ liệu trả về
            var response = new OrderResponseDto
            {
                OrderId = orderObj.OrderId,
                OrderCode = orderObj.OrderCode,
                UserId = orderObj.UserId,
                OrderStatus = orderObj.OrderStatus.ToString(),
                PaymentStatus = orderObj.PaymentStatus,
                PaymentMethod = orderObj.PaymentMethod,
                TotalAmount = orderObj.TotalAmount,
                DiscountAmount = orderObj.DiscountAmount ?? 0,
                VatAmount = orderObj.VatAmount ?? 0,
                FinalAmount = orderObj.FinalAmount,
                ReceiverName = orderObj.ReceiverName,
                ReceiverPhone = orderObj.ReceiverPhone,
                ShippingAddress = orderObj.ShippingAddress,
                Note = orderObj.Note,
                CreatedAt = orderObj.CreatedAt,
                Items = orderDetails.Select(d => new OrderDetailResponseDto
                {
                    OrderDetailId = d.OrderDetailId,
                    ProductId = d.ProductId,
                    ProductName = d.ProductName,
                    Sku = d.Sku,
                    Unit = d.Unit,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice,
                    VatRate = d.VatRate ?? 0,
                    VatAmount = d.VatAmount ?? 0,
                    TotalPrice = d.TotalPrice
                }).ToList()
            };

            return ResultHelper.Success(response);
        }
        /// <summary>
        /// Sinh mã đơn hàng
        /// </summary>
        private string GenerateOrderCode(bool isPosOrder)
        {
            string prefix = isPosOrder ? "POS" : "WEB";

            string timePart = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            return $"{prefix}_{timePart}";
        }
    }
}
