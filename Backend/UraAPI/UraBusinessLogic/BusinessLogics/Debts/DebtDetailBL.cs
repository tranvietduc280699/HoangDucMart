using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UraBusinessLogic.Interfaces.IDebts;
using UraDataLayer.Interfaces;
using UraDataLayer.Interfaces.IDebts;
using UraDataModel.Models.Debts;

namespace UraBusinessLogic.BusinessLogics.Debts
{
    public class DebtDetailBL : BaseBl<DebtDetail>, IDebtDetailBL
    {
        public DebtDetailBL(IDebtDetailDL debtDetailDL) : base(debtDetailDL, "ura_debt_details")
        {
        }
    }
}
