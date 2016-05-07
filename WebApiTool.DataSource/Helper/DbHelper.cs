using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using System.Text;
using WebApiTool.DataSource.Models;

namespace WebApiTool.DataSource.Helper
{
    public class DbHelper
    {
        public const string SplitCharMultiplesValues = "&|&";
        public const string spNamesPre = "spRapport_";
        
        public static OleDbDataReader ExecuteSpInOlapDB(ref OleDbConnection connection, string spName, IEnumerable<DataSourceFilter> reportFilter)
        {
            OleDbCommand command = new OleDbCommand(spName, connection);
            command.CommandType = CommandType.StoredProcedure;

            #region <Add Parameters>
            if (reportFilter.Count() > 0)
            {
                command.Parameters.AddRange(LoadParameters(reportFilter));
            }
            #endregion

            OleDbDataReader reader = command.ExecuteReader();

            return reader;
        }
        public static DataTable ExecuteSpInOlapDB(string connectionStringValue, string spName, IEnumerable<DataSourceFilter> reportFilter)
        {
            OleDbConnection connection = new OleDbConnection();

            connection = new OleDbConnection(connectionStringValue);
            connection.Open();

            OleDbCommand command = new OleDbCommand(spName, connection);
            command.CommandType = CommandType.StoredProcedure;

            #region <Add Parameters>
            if (reportFilter.Count() > 0)
            {
                foreach (var item in reportFilter)
                {
                    command.Parameters.AddRange(LoadParameters(reportFilter));
                }
            }
            #endregion

            OleDbDataAdapter adapter = new OleDbDataAdapter(command);

            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);

            connection.Close();
            connection.Dispose();

            return dataTable;
        }
        public static bool CreateSpInOlapDb(string connectionStringValue, string TSQL, string spName, ref string exceptionMsg)
        {
            OleDbConnection connection = new OleDbConnection();
            connection = new OleDbConnection(connectionStringValue);
            connection.Open();

            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;

            //Create SP
            command.CommandText = QueryDbHelper.GetQueryCreateSP(spName, TSQL);
            command.ExecuteNonQuery();

            return true;
        }
        public static bool ExistSpInOlapDb(string connectionStringValue, string TSQL, string spName, ref string exceptionMsg)
        {
            OleDbConnection connection = new OleDbConnection();
            connection = new OleDbConnection(connectionStringValue);
            connection.Open();

            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;
            command.CommandText = QueryDbHelper.GetQueryExistsSP(spName);
            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }
        public static bool DropSpInOlapDb(string connectionStringValue, string spName, ref string exceptionMsg)
        {
            OleDbConnection connection = new OleDbConnection();

            connection = new OleDbConnection(connectionStringValue);
            connection.Open();

            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;

            //Verify if exist SP and to Drop is True
            command.CommandText = QueryDbHelper.GetQueryExistsSP(spName);
            if (Convert.ToInt32(command.ExecuteScalar()) > 0)
            {
                //Exist SP and go to proced to drop
                command.CommandText = QueryDbHelper.GetQueryDropSP(spName);
                command.ExecuteNonQuery();
            }

            return true;
        }
        public static string GetSpName(string connectionStringValue, string spName)
        {
            spName = spName.Replace("-", "_");
            OleDbConnection connection = new OleDbConnection();
            string newSpName = spName;

            int count = 1;

            connection = new OleDbConnection(connectionStringValue);
            connection.Open();

            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;
            command.CommandText = QueryDbHelper.GetQueryExistsSP(spName);
            while (Convert.ToInt32(command.ExecuteScalar()) > 0)
            {
                newSpName = spName + "_" + count.ToString();
                command.CommandText = QueryDbHelper.GetQueryExistsSP(newSpName);
                count++;
            }

            return newSpName;

        }
        public static string GenerateQuerySQL(string tableName, IEnumerable<string> fields, IEnumerable<DataSourceFilter> reportFilters, int? top)
        {
            StringBuilder TSQL = new StringBuilder();
            StringBuilder whereClausule = new StringBuilder();
            StringBuilder orderByClausule = new StringBuilder();
            const string operAND = " AND ";
            TSQL.Append("SELECT ");
            if (top != null && top > 0)
            {
                TSQL.Append($" TOP {top} ");
            }

            #region [Select]

            if (fields != null)
            {
                foreach (string field in fields)
                {
                    TSQL.Append(field);
                    if (fields.Last() != field)
                        TSQL.Append(", ");
                }
            }
            else
            {
                TSQL.Append(" * ");
            }
            
            #endregion

            #region [From]
            TSQL.Append(" FROM " + tableName + " ");
            #endregion

            #region [Where]
            if (reportFilters != null && reportFilters.Count() > 0)
            {

                for (int index = 0; index < reportFilters.Count(); index++)
                {
                    var filter = reportFilters.ToList()[index];

                    #region  <<Build Where Clausule>>
                    if (filter.Operator != null)
                    {
                        #region <Build Where>
                        {
                            if (filter.Values != null && filter.Values.Count > 0)
                            {
                                //Multiples Values
                                #region <Multiples Values>

                                whereClausule.Append(filter.Name);
                                whereClausule.Append(filter.Operator == "=" ? " IN " : " NOT IN ");
                                whereClausule.Append(" (");

                                for (int j = 0; j < filter.Values.Count; j++)
                                {
                                    whereClausule.Append(ParseFilterToQuery(filter.DataType, filter.Values[j]));

                                    if (j <= filter.Values.Count - 2)
                                        whereClausule.Append(",");
                                }
                                whereClausule.Append(") ");
                                #endregion

                                whereClausule.Append(operAND);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(filter.Value))
                                {
                                    //Single value
                                    whereClausule.Append(" " + filter.Name + " " + filter.Operator + " " + ParseFilterToQuery(filter.DataType, filter.Value) + " ");
                                    whereClausule.Append(operAND);
                                }
                            }
                        }
                        #endregion
                    }
                    //else
                    //{
                    //    throw new Exception(string.Concat("The filter [", filter.Name, "]", " has not perator operator=>[", filter.Operator, "]"));
                    //}
                    #endregion

                    #region <<Build OrderBy Clausule>>
                    if (!string.IsNullOrEmpty(filter.OrderBy))
                    {
                        orderByClausule.Append(" " + filter.Name + " " + filter.OrderBy);
                        orderByClausule.Append(", ");
                    }
                    #endregion
                }


                //--> Remove AND first and last calusule and Add Where clausule to Query
                if (!string.IsNullOrEmpty(whereClausule.ToString()))
                {
                    #region <Remove first / Last "AND">
                    string sTemp = whereClausule.ToString();

                    //Remove first AND
                    if (sTemp.Substring(0, operAND.Length).Replace(Environment.NewLine, string.Empty) == operAND)
                    {
                        string temp = sTemp.Substring(operAND.Length, sTemp.Length - operAND.Length);
                        whereClausule = new StringBuilder(temp);
                        sTemp = whereClausule.ToString();
                    }

                    //Remove last AND
                    if (sTemp.Substring(sTemp.Length - operAND.Length, operAND.Length).Replace(Environment.NewLine, string.Empty) == operAND)
                    {
                        string temp = sTemp.Substring(0, sTemp.Length - operAND.Length);
                        whereClausule = new StringBuilder(temp);
                    }
                    #endregion

                    TSQL.Append(" WHERE ");
                    TSQL.Append(whereClausule.ToString());
                }

                //--> Add OrderBy Clausule to Query
                if (!string.IsNullOrEmpty(orderByClausule.ToString()))
                {
                    //--> Remove the last "," and concat on query Clasule OrderBy
                    TSQL.Append(" ORDER BY ");
                    TSQL.Append(orderByClausule.ToString().Substring(0, orderByClausule.ToString().Length - 2));
                }
            }
            #endregion

            return TSQL.ToString();
        }
        public static OleDbDataReader ExecuteQuery(ref OleDbConnection connection, string query)
        {
            OleDbCommand command = new OleDbCommand(query, connection);
            command.CommandType = CommandType.Text;

            OleDbDataReader reader = command.ExecuteReader();
            return reader;

        }
        private static OleDbParameter[] LoadParameters(IEnumerable<DataSourceFilter> reportFilter)
        {
            List<OleDbParameter> param = new List<OleDbParameter>();
            int currentParameterId = 0;
            string currentParameterName = "";
            string currentParameterValue = "";
            DataSourceParameter.Type currentParameterReportParameterType = DataSourceParameter.Type.None;

            try
            {
                #region <Add Parameters>
                if (reportFilter.Count() > 0)
                {
                    foreach (var item in reportFilter)
                    {
                        OleDbParameter oledbP = new OleDbParameter();
                        oledbP.ParameterName = item.Name;
                        currentParameterId = item.Id;
                        currentParameterName = item.Name;
                        currentParameterValue = item.Value;
                        currentParameterReportParameterType = ((DataSourceParameter.Type) item.DataType);

                        #region Convert Value by DbType
                        if (!string.IsNullOrEmpty(item.Value) || !string.IsNullOrWhiteSpace(item.Value))
                        {
                            switch (item.DataType)
                            {
                                case (int) DataSourceParameter.Type.Date:
                                    {
                                        DateTime value = DateTime.ParseExact(item.Value, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                                        oledbP.Value = value.ToString("yyyy/MM/dd");
                                        oledbP.DbType = DbType.Date;
                                        break;
                                    }
                                default:
                                    {
                                        oledbP.Value = item.Value;
                                        break;
                                    }
                            }
                        }
                        else
                        {
                            item.Value = null;
                        }

                        #endregion

                        oledbP.IsNullable = item.IsNullable;
                        param.Add(oledbP);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                #region
                Exception _ex = ex.ToString().Contains("Error converting data type")
                                ? new Exception("Error converting data type [" + currentParameterName + "] with value [" + currentParameterValue + "]")
                                : ex;
                _ex.Data.Add("currentParameterId", currentParameterId);
                _ex.Data.Add("currentParameterName", currentParameterName);
                _ex.Data.Add("currentParameterValue", currentParameterValue);
                _ex.Data.Add("currentParameterType", currentParameterReportParameterType.ToString());

                #endregion
                throw _ex;
            }
            return param.ToArray();

        }
        private static string ParseFilterToQuery(int type, string value)
        {
            #region Parse Value by DbType
            switch (type)
            {
                case (int) DataSourceParameter.Type.Numeric:
                    {
                        return value;
                    }
                case (int) DataSourceParameter.Type.Date:
                    {
                        return string.Concat(" CONVERT(DATETIME, '", value, "', 101) ");
                    }
                case (int) DataSourceParameter.Type.Text:
                    {
                        return string.Concat(" '%", value, "%'");
                    }
                default:
                    {
                        return string.Concat(" '", value, "' ");
                    }
            }

            #endregion
        }
    }
}
