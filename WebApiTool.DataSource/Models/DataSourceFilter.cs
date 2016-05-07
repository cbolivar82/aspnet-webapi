using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiTool.DataSource.Models
{
    public class DataSourceFilter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Caption { get; set; }
        public string Value { get; set; }
        public List<string> Values { get; set; }
        public int DataType { get; set; } //Reference Oldb.DbType
        public bool IsNullable { get; set; }
        public bool Visible { get; set; }
        public string Operator { get; set; }
        public string OrderBy { get; set; }
    }
}
