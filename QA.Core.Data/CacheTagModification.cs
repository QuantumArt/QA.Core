using System;

namespace QA.Core.Data
{

    /// <summary>
    /// Класс, описывающий последнее изменение тега
    /// </summary>
    public class TableModification
    {
        public string TableName { get; set; }
        public DateTime LiveModified { get; set; }
        public DateTime StageModified { get; set; }
    }
}
