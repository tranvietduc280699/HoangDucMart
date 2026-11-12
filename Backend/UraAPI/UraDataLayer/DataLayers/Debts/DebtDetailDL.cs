using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UraDataLayer.Database;
using UraDataLayer.Interfaces.IDebts;
using UraDataModel.Models.Debts;

namespace UraDataLayer.DataLayers.Debts
{
    public class DebtDetailDL : BaseDL<DebtDetail>, IDebtDetailDL
    {
        public DebtDetailDL(MySqlContext context) : base(context)
        {
        }
    }
}
