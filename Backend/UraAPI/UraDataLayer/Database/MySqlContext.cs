using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UraDataLayer.Database
{
    public class MySqlContext
    {
        private readonly string _connectionString;
        public MySqlContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        // public kết nối
        public IDbConnection CreateConnectTion()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
