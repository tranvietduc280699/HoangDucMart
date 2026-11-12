using UraBusinessLogic.Interfaces.IDebts;
using UraDataLayer.Interfaces.IDebts;
using UraDataLayer.Interfaces.IProducts;
using UraDataLayer.Interfaces.IUsers;
using UraDataLayer.Interfaces.Sales;
using UraDataModel.DTOs.Debts;
using UraDataModel.Enums;
using UraDataModel.Models;
using UraDataModel.Models.Debts;
using UraDataModel.Models.Sales;
using UraDataModel.Response;
using UraDataModel.Response.Debts;

namespace UraBusinessLogic.BusinessLogics.Debts
{
    public class DebtBL : BaseBl<Debt>, IDebtBL
    {
        private readonly IDebtDL _debtDL;
        private readonly IOrderDL _orderDL;
        private readonly IUserDL _userDL;
        private readonly IProductDL _productDL;
        private readonly IOrderDetailDL _orderDetailDL;
        public DebtBL(IDebtDL debtDL, IOrderDL orderDL, IUserDL userDL, IOrderDetailDL orderDetailDL, IProductDL productDL) : base(debtDL, "ura_debts")
        {
            _debtDL = debtDL;
            _orderDL = orderDL;
            _userDL = userDL;
            _orderDetailDL = orderDetailDL;
            _productDL = productDL;
        }
        /// <summary>
        /// thêm mới công nợ
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResultModel<DebtResponseDto>> InsertDebt(DebtCreateDto dto, string userId)
        {
            if (dto == null)
                return ResultHelper.Fail<DebtResponseDto>(ResponseCode.BadRequest);

            string debtId = Guid.NewGuid().ToString();
            // đơn hàng có ko ?
            bool isFromOrder = !string.IsNullOrWhiteSpace(dto.OrderId);

            // thông tin người nợ
            string? debtorName = dto.DebtorName;
            string? debtorPhone = dto.DebtorPhone;
            string? debtorAddress = dto.DebtorAddress;
            string? debtUserId = string.IsNullOrWhiteSpace(dto.UserId) ? null : dto.UserId;


            var debtDetails = new List<DebtDetail>();
            decimal total = 0;
            string? orderId = null;
            string? orderCode = null;

            // nếu có orderId sẽ lấy thông tin trong db
            if (isFromOrder)
            {
                // đơn hàng
                var orders = await _orderDL.GetByConditionAsync("ura_orders", new BaseFilter
                {
                    Conditions = new Dictionary<string, string?> { { "OrderId", dto.OrderId } },
                    LogicalOperator = ""
                });
                var order = orders?.FirstOrDefault();
                if (order == null)
                    return ResultHelper.Fail<DebtResponseDto>(ResponseCode.NotFound);

                // Đã thanh toán hoặc đã có trong công nợ
                if (order.PaymentStatus == PaymentStatusEnum.Paid || order.PaymentStatus == PaymentStatusEnum.Debt)
                    return ResultHelper.Fail<DebtResponseDto>(ResponseCode.BadRequest);

                // chi tiết đơn hàng
                var orderDetails = (await _orderDetailDL.GetByConditionAsync("ura_order_details", new BaseFilter
                {
                    Conditions = new Dictionary<string, string?> { { "OrderId", dto.OrderId } },
                    LogicalOperator = ""
                }))?.ToList();
                if (orderDetails == null || !orderDetails.Any())
                    return ResultHelper.Fail<DebtResponseDto>(ResponseCode.BadRequest);

                orderId = order.OrderId;
                orderCode = order.OrderCode;
                total = order.FinalAmount;             
                debtUserId ??= order.UserId;

                // Thông tin người nợ
                debtorName = string.IsNullOrWhiteSpace(debtorName) ? null : debtorName;
                debtorPhone = string.IsNullOrWhiteSpace(debtorPhone) ? null : debtorPhone;
                debtorAddress = string.IsNullOrWhiteSpace(debtorAddress) ? null : debtorAddress;

                // copy order detail từ đơn sang detail nợ
                debtDetails = orderDetails.Select(d => new DebtDetail
                {
                    DebtDetailId = Guid.NewGuid().ToString(),
                    DebtId = debtId,
                    ProductId = d.ProductId,
                    ProductName = d.ProductName,
                    Sku = d.Sku,
                    Unit = d.Unit,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice,
                    TotalPrice = d.TotalPrice,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId
                }).ToList();

                // Lấy thông tin từ tài khoản (nếu có) trước, rồi mới đến thông tin trên đơn
                if (debtUserId != null)
                {
                    var users = await _userDL.GetByConditionAsync("ura_users", new BaseFilter
                    {
                        Conditions = new Dictionary<string, string?> { { "UserId", debtUserId } },
                        LogicalOperator = ""
                    });
                    var user = users?.FirstOrDefault();
                    if (user == null)
                        return ResultHelper.Fail<DebtResponseDto>(ResponseCode.NotFound);

                    debtorName ??= user.UserName;
                    debtorPhone ??= user.Phone;
                    debtorAddress ??= user.Address;
                }

                debtorName ??= order.ReceiverName;
                debtorPhone ??= order.ReceiverPhone;
                debtorAddress ??= order.ShippingAddress;
            }
            // nhập trực tiếp 
            else
            {
                if (dto.Items == null || !dto.Items.Any())
                    return ResultHelper.Fail<DebtResponseDto>(ResponseCode.BadRequest);

                if (dto.Items.Any(x => (x.Quantity ?? 0) <= 0 && (x.Amount ?? 0) <= 0 && (x.WeightGrams ?? 0) <= 0))
                    return ResultHelper.Fail<DebtResponseDto>(ResponseCode.BadRequest);

                // Có userId: lấy thông tin còn thiếu từ tài khoản
                if (debtUserId != null)
                {
                    var users = await _userDL.GetByConditionAsync("ura_users", new BaseFilter
                    {
                        Conditions = new Dictionary<string, string?> { { "UserId", debtUserId } },
                        LogicalOperator = ""
                    });
                    var user = users?.FirstOrDefault();
                    if (user == null)
                        return ResultHelper.Fail<DebtResponseDto>(ResponseCode.NotFound);

                    if (string.IsNullOrWhiteSpace(debtorName)) debtorName = user.UserName;
                    if (string.IsNullOrWhiteSpace(debtorPhone)) debtorPhone = user.Phone;
                    if (string.IsNullOrWhiteSpace(debtorAddress)) debtorAddress = user.Address;
                }
                // gộp trùng nhau
                var items = dto.Items.GroupBy(x => x.ProductId).Select(y => new
                {
                    ProductId = y.Key,
                    Quantity = y.Sum(x => x.Quantity ?? 0),
                    WeightGrams = y.Sum(x => x.WeightGrams ?? 0),
                    Amount = y.Sum(x => x.Amount ?? 0),
                    y.First().VatRate
                }).ToList();

                decimal subTotal = 0;
                decimal vatAmount = 0;

                foreach (var item in items)
                {
                    // lấy bản ghi sản phẩm
                    var products = await _productDL.GetByConditionAsync("ura_products", new BaseFilter
                    {
                        Conditions = new Dictionary<string, string?> { { "ProductId", item.ProductId } },
                        LogicalOperator = ""
                    });
                    var product = products?.FirstOrDefault();
                    if (product == null)
                        return ResultHelper.Fail<DebtResponseDto>(ResponseCode.NotFound);

                    decimal unitPrice = product.SellPrice;
                    if (unitPrice <= 0)
                        return ResultHelper.Fail<DebtResponseDto>(ResponseCode.BadRequest);

                    decimal quantity = item.Quantity;
                    decimal detailTotal;
                    decimal detailVatRate = item.VatRate ?? dto.VatRate ?? 0;

                    // nếu nhập vào số tiền
                    if (item.Amount > 0)
                    {
                        detailTotal = item.Amount;
                        quantity = Math.Round(item.Amount / unitPrice, 3);
                    }
                    else
                    {
                        // nếu nhập vào trọng lượng
                        if (item.WeightGrams > 0)
                        {
                            decimal productWeight = product.WeightGrams ?? 0;
                            if (productWeight <= 0)
                                return ResultHelper.Fail<DebtResponseDto>(ResponseCode.BadRequest);
                            quantity = Math.Round(item.WeightGrams / productWeight, 3);
                        }
                        detailTotal = Math.Round(quantity * unitPrice, 0);
                    }

                    subTotal += detailTotal;
                    vatAmount += Math.Round(detailTotal * detailVatRate / 100m, 0);

                    debtDetails.Add(new DebtDetail
                    {
                        DebtDetailId = Guid.NewGuid().ToString(),
                        DebtId = debtId,
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        Sku = product.Sku,
                        Unit = product.Unit,
                        Quantity = quantity,
                        UnitPrice = unitPrice,
                        TotalPrice = detailTotal,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = userId
                    });
                }

                decimal discount = dto.DiscountAmount ?? 0;
                if (discount < 0 || discount > subTotal)
                    return ResultHelper.Fail<DebtResponseDto>(ResponseCode.BadRequest);

                total = subTotal - discount + vatAmount;
            }

            // tên người nợ ko đc để trống
            if (string.IsNullOrWhiteSpace(debtorName))
                return ResultHelper.Fail<DebtResponseDto>(ResponseCode.BadRequest);

            decimal paid = dto.PaidAmount ?? 0;
            if (paid < 0 || paid > total)
                return ResultHelper.Fail<DebtResponseDto>(ResponseCode.BadRequest);

            var debt = new Debt
            {
                DebtId = debtId,
                OrderId = orderId,
                UserId = debtUserId,
                DebtorName = debtorName,
                DebtorPhone = debtorPhone,
                DebtorAddress = debtorAddress,
                DebtType = isFromOrder ? 1 : 2,
                TotalAmount = total,
                PaidAmount = paid,
                RemainingAmount = total - paid,
                DebtStatus = paid <= 0 ? DebtStatusEnum.Unpaid
                           : paid < total ? DebtStatusEnum.Partial
                           : DebtStatusEnum.Paid,
                Note = dto.Note,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId
            };

            // thêm mới
            await _debtDL.InsertDebt(debt, debtDetails);

            var response = new DebtResponseDto
            {
                DebtId = debt.DebtId,
                OrderId = debt.OrderId,
                OrderCode = orderCode,
                UserId = debt.UserId,
                DebtorName = debt.DebtorName,
                DebtorPhone = debt.DebtorPhone,
                DebtorAddress = debt.DebtorAddress,
                DebtType = debt.DebtType,
                DebtTypeText = isFromOrder ? "Nợ từ đơn hàng" : "Nợ trực tiếp",
                TotalAmount = debt.TotalAmount,
                PaidAmount = debt.PaidAmount,
                RemainingAmount = debt.RemainingAmount,
                DebtStatus = (int)debt.DebtStatus,
                Note = debt.Note,
                CreatedAt = (DateTime)debt.CreatedAt,
                CreatedBy = debt.CreatedBy,
                Items = debtDetails.Select(d => new DebtDetailResponseDto
                {
                    DebtDetailId = d.DebtDetailId,
                    ProductId = d.ProductId,
                    ProductName = d.ProductName,
                    Sku = d.Sku,
                    Unit = d.Unit,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice,
                    TotalPrice = d.TotalPrice,
                    Note = d.Note
                }).ToList()
            };

            return ResultHelper.Success(response);
        }
        /// <summary>
        /// cập nhật công nợ và đơn hàng
        /// </summary>
        /// <param name="debt"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResultModel<string>> UpdateStatus(Debt debt, string userId)
        {
            if (debt == null || string.IsNullOrWhiteSpace(debt.DebtId))
                return ResultHelper.Fail<string>(ResponseCode.BadRequest);

            // Kiểm tra trạng thái
            if (!Enum.IsDefined(typeof(DebtStatusEnum), debt.DebtStatus))
                return ResultHelper.Fail<string>(ResponseCode.BadRequest);

            try
            {
                var now = DateTime.UtcNow;

                // Cập nhật bảng công nợ
                Debt obj = new Debt
                {
                    DebtId = debt.DebtId,
                    DebtStatus = debt.DebtStatus,
                    UpdatedAt = now,
                    UpdatedBy = userId
                };

                var resultDebt = await base.UpdateColumnAsync("debtStatus", obj);
                if (resultDebt == null)
                    return ResultHelper.Fail<string>(ResponseCode.NotFound);

                // Có đơn hàng thì cập nhật trạng thái thanh toán của đơn
                if (!string.IsNullOrWhiteSpace(debt.OrderId))
                {
                    // Trả hết nợ (Paid) thì đơn thành Đã thanh toán, ngược lại giữ là Công nợ
                    var paymentStatus = debt.DebtStatus == DebtStatusEnum.Paid
                        ? PaymentStatusEnum.Paid
                        : PaymentStatusEnum.Debt;

                    Order orderObj = new Order
                    {
                        OrderId = debt.OrderId,
                        PaymentStatus = paymentStatus,
                        UpdatedAt = now,
                        UpdatedBy = userId
                    };

                    var resultOrder = await _orderDL.UpdateAsync("ura_orders", orderObj);
                    if (resultOrder == null)
                        return ResultHelper.Fail<string>(ResponseCode.BadRequest);
                }

                return ResultHelper.Success<string>(debt.DebtId);
            }
            catch (Exception)
            {
                return ResultHelper.Fail<string>(ResponseCode.BadRequest);
            }
        }
    }
}
