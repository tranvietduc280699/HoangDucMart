using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UraDataModel.Models.Debts;

namespace UraDataLayer.Interfaces.IDebts
{
    public interface IDebtDL : IBaseDL<Debt>
    {
        public Task<Debt> InsertDebt(Debt debt, List<DebtDetail> debtDetail);
    }
}
