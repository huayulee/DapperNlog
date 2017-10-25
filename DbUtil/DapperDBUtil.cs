using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperDemo.DbUtil
{
    public class DapperDBUtil
    {
        private readonly string connectionString;

        public DapperDBUtil(short brandID)
        {
            this.connectionString = ConfigurationManager.ConnectionStrings["conn"].ToString();
        }

        public IEnumerable<dynamic> Get(string sql, object param = null)
        {
            using (var conn = this.GetOpenConnection())
            {
                return conn.Query(sql, param);
            }
        }

        public IEnumerable<TModel> Get<TModel>(string sql, object param = null)
        {
            using (var conn = this.GetOpenConnection())
            {
                return conn.Query<TModel>(sql, param);
            }
        }

        public T QueryForObject<T>(string sql, object param = null)
        {
            using (var conn = this.GetOpenConnection())
            {
                return conn.QuerySingleOrDefault<T>(sql, param);
            }
        }

        public int QueryForInt(string sql, object param = null)
        {
            using (var conn = this.GetOpenConnection())
            {
                return conn.QuerySingle<int>(sql, param);
            }
        }

        public string QueryForString(string sql, object param = null)
        {
            using (var conn = this.GetOpenConnection())
            {
                return conn.QuerySingle<string>(sql, param);
            }
        }

        public int Execute(string sql, object param = null)
        {
            int result = 0;
            using (var conn = this.GetOpenConnection())
            {
                result = conn.Execute(sql, param);
            }

            return result;
        }

        public DbConnection GetOpenConnection()
        {
            var factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
            var cnn = factory.CreateConnection();
            if (cnn == null)
            {
                throw new ApplicationException("Can't Create Connection!");
            }

            cnn.ConnectionString = this.connectionString;
            cnn.Open();
            return new LogDbConnection(cnn);
        }
    }
}
