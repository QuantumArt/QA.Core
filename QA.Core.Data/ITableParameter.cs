using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Server;

namespace QA.Core.Data
{
    public interface ITableParameter : IEnumerable<SqlDataRecord>
    {
    }
}
