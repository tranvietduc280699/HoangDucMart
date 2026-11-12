using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UraDataModel.Models;

namespace UraDataLayer.Interfaces
{
    public interface IBaseDL<T>
    {
         Task<IEnumerable<T>> GetAllByTable(string tableName);
         Task<IEnumerable<T>> GetByConditionAsync(string tableName, BaseFilter filter);

         Task<int> DeleteById(string tableName, string columnId, string id);

         Task<int> InsertAsync<T>(string tableName, T entity);
         Task<int> UpdateAsync(string tableName, T entity);
        Task<int> UpdateColumnAsync(string tableName, string propertyName, T entity);

    }
}
