using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiTool.DataSource.Models
{
    public class DataSourceParameter
    {
        public enum Type
        {
            None = 0,
            Numeric = 1,
            Text = 2,
            Date = 3,
            Multivalue = 4
        }
    }
}
