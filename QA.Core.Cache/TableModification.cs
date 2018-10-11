using System;

#pragma warning disable 1591


namespace QA.Core.Cache
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
