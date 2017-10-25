using NLog;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperDemo.DbUtil
{
    internal class LogDbConnection : DbConnection, IDisposable
    {
        private readonly List<DbCommand> cmds;
        private readonly DbConnection conn;
        private Logger log;

        public LogDbConnection(DbConnection conn)
        {
            this.conn = conn;
            this.cmds = new List<DbCommand>();
        }

        public override string ConnectionString
        {
            get { return this.conn.ConnectionString; }
            set { this.conn.ConnectionString = value; }
        }

        public override string Database
        {
            get { return this.conn.Database; }
        }

        public override string DataSource
        {
            get { return this.conn.DataSource; }
        }

        public override string ServerVersion
        {
            get { return this.conn.ServerVersion; }
        }

        public override System.Data.ConnectionState State
        {
            get { return this.conn.State; }
        }

        private Logger Log
        {
            get
            {
                if (this.log == null)
                {
                    this.log = LogManager.GetLogger("Database");
                }

                return this.log;
            }
        }

        public override void ChangeDatabase(string databaseName)
        {
            this.conn.ChangeDatabase(databaseName);
        }

        public override void Close()
        {
            this.conn.Close();
            foreach (var cmd in this.cmds)
            {
                this.Log.Debug("Database: <{0}> SQL Command: < {1}{2} >", this.Database, cmd.CommandText, FormatParameters(cmd.Parameters));
            }
        }

        public override void Open()
        {
            this.conn.Open();
        }

        void IDisposable.Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.Close();
        }

        protected override DbTransaction BeginDbTransaction(System.Data.IsolationLevel isolationLevel)
        {
            return this.conn.BeginTransaction(isolationLevel);
        }

        protected override DbCommand CreateDbCommand()
        {
            var cmd = this.conn.CreateCommand();
            this.cmds.Add(cmd);
            return cmd;
        }

        private static string FormatParameters(DbParameterCollection parameters)
        {
            var sb = new StringBuilder();
            foreach (var param in parameters.Cast<DbParameter>())
            {
                if (sb.Length != 0)
                {
                    sb.Append(", ");
                }

                sb.AppendFormat("@{0} [{1}] = \"{2}\"", param.ParameterName, param.DbType, param.Value);
            }

            if (sb.Length != 0)
            {
                sb.Insert(0, "    -> ");
            }

            return sb.ToString();
        }
    }
}
