using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UraDataModel.DTOs.Debts;
using UraDataModel.Models.Debts;
using UraDataModel.Response;
using UraDataModel.Response.Debts;

namespace UraBusinessLogic.Interfaces.IDebts
{
    public interface IDebtBL : IBaseBL<Debt>
    {
        /// <summary>
        /// thêm mới công nợ
        /// </summary>
        /// <param name="debt"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<ResultModel<DebtResponseDto>> InsertDebt(DebtCreateDto dto, string userId);
        /// <summary>
        /// cập nhật trạng thái Debt
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<ResultModel<string>> UpdateStatus(Debt dto, string userId);

    }
}
