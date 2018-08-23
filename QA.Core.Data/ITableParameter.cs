using System.Collections.Generic;
using Microsoft.SqlServer.Server;
#pragma warning disable 1591

namespace QA.Core.Data
{
    public interface ITableParameter : IEnumerable<SqlDataRecord>
    {
    }
}
