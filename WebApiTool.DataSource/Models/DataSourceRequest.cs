using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiTool.DataSource.Models
{
    public class DataSourceRequest
    {
        public string ResourceType { get; set; }
        public string ResourceName { get; set; }
        public string ConnectionStringName { get; set; }
        public List<string> Fields { get; set; }
        public IEnumerable<DataSourceFilter> Filters { get; set; }
        public int Top { get; set; }
    }
}