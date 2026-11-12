using Dapper;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UraDataLayer.Database;
using UraDataLayer.Interfaces;
using UraDataModel.Models;

namespace UraDataLayer.DataLayers
{
    public class BaseDL<T> : IBaseDL<T>
    {
        // cung cấp cục kết nối
        protected readonly MySqlContext _context;
        public BaseDL(MySqlContext context)
        {
            _context = context;
        }
        /// <summary>
        /// lấy tất cả dữ liệu
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"> tên bảng db</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAllByTable(string tableName)
        {
            using var connect = _context.CreateConnectTion();

            string sql = $"SELECT * FROM {tableName}";
            return await connect.QueryAsync<T>(sql);

        }
        /// <summary>
        /// lấy tất cả bản ghi theo điều kiện truyền vào
        /// </summary>
        /// <param name="tableName"> tên bảng</param>
        /// <param name="conditions"> truyền object : column & param</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetByConditionAsync(string tableName, BaseFilter filter)
        {
            using var connect = _context.CreateConnectTion();
            var parameters = new DynamicParameters();
            var sqlWhere = filter.Conditions.Select((c, i) =>
            {
                var columnName = ToColumn(c.Key);
                parameters.Add($"p{i}", c.Value);
                return $"{columnName} = @p{i}";
            }).ToList();
            var LogicalOperator = filter.LogicalOperator.Equals("OR")? " OR " : " AND ";
            string where = sqlWhere.Any() ? "WHERE " + string.Join(LogicalOperator, sqlWhere) : " ";
            string sql = $"SELECT * FROM {tableName} {where}";

            return await connect.QueryAsync<T>(sql, parameters);

        }
        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="tableName"> tên bảng</param>
        /// <param name="columnId"> tên cột</param>
        /// <param name="id"> id bản ghi</param>
        /// <returns></returns>
        public async Task<int> DeleteById(string tableName, string columnId, string id)
        {
            using var connect = _context.CreateConnectTion();

            string sql = $"DELETE FROM {tableName} WHERE {columnId} = @Id";
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            return await connect.ExecuteAsync(sql, parameters);

        }
        /// <summary>
        /// thêm mới bản ghi
        /// </summary>
        /// <param name="tableName"> tên bảng</param>
        /// <param name="entity"> đối tượng </param>
        /// <returns></returns>
        public async Task<int> InsertAsync<T>(string tableName, T entity)
        {
            using var connect = _context.CreateConnectTion();
            // lấy field và loại bỏ image
            var properties = typeof(T).GetProperties()
                .Where(p => p.Name != "Image")
                .ToList();
            // field và convert field
            var fieldNames = properties.Select(p => ToColumn(p.Name));
           
            // @Field
            var paramNames = properties.Select(p => $"@{p.Name}");
            string columns = string.Join(" , ", fieldNames);
            string values = string.Join(" , ", paramNames);
            string sql = $"INSERT INTO {tableName}({columns}) VALUES ({values})";

            return await connect.ExecuteAsync(sql, entity);

        }
        /// <summary>
        /// cập nhật bản ghi
        /// </summary>
        /// <param name="tableName"> tên bảng</param>
        /// <param name="primaryKeyName"> id </param>
        /// <param name="entity"> đối tượng</param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(string tableName, T entity)
        {
            using var connect = _context.CreateConnectTion();
            // tên class
            string modelName = typeof(T).Name;
            var propName = modelName + "Id";   // UserId

            var properties = typeof(T).GetProperties().Where(p => p.Name != "Image").ToList();

            // field và convert field (USER_NAME = @UserName)
            var setField = properties.Select(p => $"{ToColumn(p.Name)} = @{p.Name}");
            // @Field
            var paramNames = properties.Select(p => $"@{p.Name}");
            var sqlColumn = ToColumn(modelName + "Id");       // USER_ID

            string setString = string.Join(" , ", setField);
            string sql = $"UPDATE {tableName} SET {setString} WHERE {sqlColumn} = @{propName}";
            return await connect.ExecuteAsync(sql, entity);
        }
       /// <summary>
       /// cập nhật giá trị trong cột
       /// </summary>
       /// <param name="tableName"></param>
       /// <param name="propertyName"></param>
       /// <param name="entity"></param>
       /// <returns></returns>
        public async Task<int> UpdateColumnAsync(string tableName, string propertyName, T entity)
        {
            using var connect = _context.CreateConnectTion();
            string idProp = typeof(T).Name + "Id"; //UserId
            string columnID = ToColumn(idProp); // USER_ID
            // convert
            string column = ToColumn(propertyName);
            string sqlColumn = Regex.Replace(propertyName, "(?<!^)([A-Z])","_$1").ToUpper();

            string sql = $"UPDATE {tableName} SET {column} = @{propertyName} WHERE {columnID} = @{idProp}";

            return await connect.ExecuteAsync(sql, entity);
        }
        /// <summary>
        /// hàm máp key vs db
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected static string ToColumn(string name)
        {
            return Regex.Replace(name, "(?<!^)([A-Z])", "_$1").ToUpperInvariant();
        }
    }
}
