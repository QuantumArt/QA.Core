using System.Linq;
using System.Collections.Generic;
using System.Data;
using Microsoft.SqlServer.Server;

namespace QA.Core.Data.Collections
{
    /// <summary>
    /// 
    /// </summary>
    public static class TableVariableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SqlDataRecord> CreateSqlDataRecords(this IEnumerable<int> ids)
        {
            SqlMetaData[] metaData = new SqlMetaData[1];
            metaData[0] = new SqlMetaData("Id", SqlDbType.Int);
            SqlDataRecord record = new SqlDataRecord(metaData);
            foreach (var id in ids)
            {
                record.SetInt32(0, id);
                yield return record;
            }
        }
		public static IEnumerable<SqlDataRecord> Ensure(this IEnumerable<SqlDataRecord> ids)
		{
			if (!ids.Any())
			{
				return null;
			}

			return ids;
		}

    }
}
