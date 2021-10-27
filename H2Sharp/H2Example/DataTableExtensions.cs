using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H2Example
{
    public static class DataTableExtensions
    {
        public static String ToXml(this DataTable table)
        {
            var o = new MemoryStream();
            table.WriteXml(o);
            var t = new StringWriter();
            o.Close();
            o = new MemoryStream(o.GetBuffer());
            var reader = new StreamReader(o);
            return reader.ReadToEnd();
        }
    }
}
