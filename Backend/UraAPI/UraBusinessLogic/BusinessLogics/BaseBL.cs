using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UraBusinessLogic.Interfaces;
using UraDataLayer.DataLayers;
using UraDataLayer.Interfaces;
using UraDataModel.Models;

namespace UraBusinessLogic.BusinessLogics
{
    public class BaseBl<T> : IBaseBL<T>
    {
        protected readonly IBaseDL<T> _baseDL;
        protected readonly string _tableName;
        public BaseBl(IBaseDL<T> baseDL, string tableName)
        {
            _baseDL = baseDL;
            _tableName = tableName;
        }
        // virtual cho phép lớp cha ghi đè
        /// <summary>
        /// lấy tất cả
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await _baseDL.GetAllByTable(_tableName);
        }
        /// <summary>
        /// bộ lọc lấy dữ liệu
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> GetByCondition(BaseFilter filter)
        {
            
            return await _baseDL.GetByConditionAsync(_tableName, filter);
        }
        /// <summary>
        /// thêm mưới bản ghi
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<int> Insert(T entity)
        {
            return await _baseDL.InsertAsync(_tableName, entity);
        }
        /// <summary>
        /// cập nhật bản ghi
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<int> Update(T entity)
        {
            return await _baseDL.UpdateAsync(_tableName, entity);
        }
        /// <summary>
        /// xóa theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<int> DeleteById(string id)
        {
            string modelName = typeof(T).Name;

            var columnId = modelName.ToUpper() + "_ID";
            return await _baseDL.DeleteById(_tableName, columnId, id);
        }
        /// <summary>
        /// cập nhật 1 field
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateColumnAsync(string propertyName, T entity)
        {
            return await _baseDL.UpdateColumnAsync(_tableName, propertyName, entity);
        }

    }
}
