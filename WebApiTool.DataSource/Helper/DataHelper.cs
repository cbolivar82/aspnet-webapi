using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace WebApiTool.DataSource.Helper
{
    public static class DataHelper
    {
        public static List<Dictionary<string, object>> PrepareToSerializeData(ref OleDbDataReader dataReader, bool singleColumn)
        {
            try
            {
                var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;

                if (dataReader.HasRows)
                {
                    if (singleColumn)
                    {
                        #region <SingleColumn>
                        while (dataReader.Read())
                        {
                            row = new Dictionary<string, object>();
                            row.Add("DataValue", dataReader[0]);
                            rows.Add(row);
                        }
                        #endregion
                    }
                    else
                    {
                        #region <MultiColums>
                        while (dataReader.Read())
                        {
                            row = new Dictionary<string, object>();
                            foreach (string _column in ColumnList(dataReader))
                            {
                                row.Add(_column, dataReader[_column]);
                            }
                            rows.Add(row);
                        }
                        #endregion
                    }

                }
                return rows;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<Dictionary<string, object>> PrepareToSerializeData(ref OleDbDataReader dataReader, List<string> columns)
        {
            try
            {
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        row = new Dictionary<string, object>();
                        foreach (string _column in columns)
                        {
                            row.Add(_column, dataReader[_column]);
                        }
                        rows.Add(row);
                    }
                }
                return rows;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable DeserializeJsonObjectString(string jsonData)
        {
            try
            {
                DataTable dtData = (DataTable) JsonConvert.DeserializeObject(jsonData, (typeof(DataTable)));
                //var dt = JsonConvert.DeserializeObject<DataTable>(modelRequest.Data);
                return dtData;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        public static List<string> ColumnList(this IDataReader dataReader)
        {
            var columns = new List<string>();
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                columns.Add(dataReader.GetName(i));
            }
            return columns;
        }
    }
}
