using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiTool.DataSource.Helper;
using WebApiTool.DataSource.Models;

namespace WebApiTool.DataSource.Service
{
    public class DataSourceService
    {
        public static List<Dictionary<string, object>> LoadData(DataSourceRequest request)
        {
            if (request.ResourceType.ToLower() == "table")
            {
                return LoadFromTable(request);
            }
            else if (request.ResourceType.ToLower() == "sp")
            {
                return LoadFromSp(request);
            }
            return new List<Dictionary<string, object>>();
        }
        private static List<Dictionary<string, object>> LoadFromSp(DataSourceRequest request)
        {
            List<Dictionary<string, object>> data = null;

            if (!string.IsNullOrEmpty(request.ResourceName))
            {
                OleDbConnection connection = new OleDbConnection();

                connection.ConnectionString = System.Configuration.ConfigurationManager.AppSettings[request.ConnectionStringName];
                connection.Open();

                //Execute SP to get data
                OleDbDataReader dataReader = DbHelper.ExecuteSpInOlapDB(ref connection, request.ResourceName, request.Filters);

                List<string> fields = request.Fields ?? dataReader.ColumnList();
                data = DataHelper.PrepareToSerializeData(ref dataReader, fields);

                //Close current connection
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            return data;
        }

        private static List<Dictionary<string, object>> LoadFromTable(DataSourceRequest request)
        {
            string tableName = request.ResourceName;
            OleDbConnection connection = new OleDbConnection();
            List<Dictionary<string, object>> data = null;

            connection.ConnectionString = System.Configuration.ConfigurationManager.AppSettings[request.ConnectionStringName];

            if (!string.IsNullOrEmpty(tableName))
            {
                //Get SQL queryTemp to execute on DB
                var queryTemp = DbHelper.GenerateQuerySQL(tableName
                    , request.Fields
                    , request.Filters
                    , request.Top
                    );

                #region Return JsonData
                //Preserve connection open to data reader
                connection.Open();

                //Get a prepare data to Json
                OleDbDataReader dataReader = DbHelper.ExecuteQuery(ref connection, queryTemp);
                data = DataHelper.PrepareToSerializeData(ref dataReader, false);


                //Close current connection
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    connection.Dispose();
                    connection = null;
                }
                #endregion
            }
            return data;
        }
    }
}
