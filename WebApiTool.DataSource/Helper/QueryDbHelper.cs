using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiTool.DataSource.Helper
{
    public static class QueryDbHelper
    {
        public const string SchemaName = "dbo";
        public static string GetQueryExistsSP(string spName)
        {
            return "SELECT COUNT(object_id) FROM sys.objects WHERE type = 'P' AND name = '" + spName + "'";
        }
        public static string GetQueryDropSP(string spName)
        {
            return string.Concat("DROP PROCEDURE ", " [", SchemaName, "].", "[", spName, "]");
        }
        public static string GetQueryCreateSP(string spName, string TSQL)
        {
            return string.Concat("CREATE PROC ", " [", SchemaName, "].", "[", spName, "] ", TSQL);
        }
    }
}
