using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UraDataModel.Models;
using static Mysqlx.Expect.Open.Types;

namespace UraBusinessLogic.Interfaces
{
    public interface IBaseBL<T>
    {
        public Task<IEnumerable<T>> GetAll();
        public Task<IEnumerable<T>> GetByCondition(BaseFilter filter);
        public Task<int> Insert(T entity);
        public Task<int> Update(T entity);
        Task<int> DeleteById(string id);
        Task<int> UpdateColumnAsync(string propertyName,T entity);
    }
}
