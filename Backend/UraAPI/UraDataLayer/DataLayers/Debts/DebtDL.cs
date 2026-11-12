using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UraDataLayer.Database;
using UraDataLayer.Interfaces.IDebts;
using UraDataModel.Models.Debts;

namespace UraDataLayer.DataLayers.Debts
{
    public class DebtDL : BaseDL<Debt>, IDebtDL
    {
        private readonly MySqlContext _context; 
        public DebtDL(MySqlContext context) : base(context)
        {
            _context = context;
        }
        /// <summary>
        /// thêm công nợ vào 2 bảng debt và detail
        /// </summary>
        /// <param name="debt"></param>
        /// <param name="debtDetail"> dữ liệu thêm vào detail</param>
        /// <returns></returns>
        public async Task<Debt> InsertDebt(Debt debt, List<DebtDetail> debtDetail)
        {
            var detailsJson = JsonSerializer.Serialize(debtDetail); 

            var p = new DynamicParameters();
            p.Add("p_DebtId", debt.DebtId);
            p.Add("p_OrderId", debt.OrderId);
            p.Add("p_UserId", debt.UserId);
            p.Add("p_DebtorName", debt.DebtorName);
            p.Add("p_DebtorPhone", debt.DebtorPhone);
            p.Add("p_DebtorAddress", debt.DebtorAddress);
            p.Add("p_DebtType", debt.DebtType);
            p.Add("p_TotalAmount", debt.TotalAmount);
            p.Add("p_PaidAmount", debt.PaidAmount);
            p.Add("p_RemainingAmount", debt.RemainingAmount);
            p.Add("p_DebtStatus", (int)debt.DebtStatus); 
            p.Add("p_Note", debt.Note);
            p.Add("p_CreatedAt", debt.CreatedAt);
            p.Add("p_CreatedBy", debt.CreatedBy);
            p.Add("p_DetailsJson", detailsJson);
            await _context.CreateConnectTion().ExecuteAsync("Proc_Debt_Insert", p, commandType: CommandType.StoredProcedure);

            return debt;
        }
    }
}
