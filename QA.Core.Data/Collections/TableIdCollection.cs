using System.Collections.Generic;
using System.Data;
using Microsoft.SqlServer.Server;

namespace QA.Core.Data.Collections
{
    /// <summary>
    /// Коллекция идентификаторов
    /// </summary>
    public class TableIdCollection : List<int>, ITableParameter
    {
        /// <summary>
        /// Конвертация
        /// </summary>
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            SqlMetaData[] metaData = new SqlMetaData[1];
            metaData[0] = new SqlMetaData("Id", SqlDbType.Int);
            SqlDataRecord record = new SqlDataRecord(metaData);

            foreach (var id in this)
            {
                record.SetInt32(0, id);
                yield return record;
            }
        }
    }
}
