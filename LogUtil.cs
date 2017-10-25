using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperDemo
{
    public static class LogUtil
    {
        private static Logger log = LogManager.GetLogger("TransactionReport");

        public static Logger Log
        {
            get { return log; }
        }
    }
}
