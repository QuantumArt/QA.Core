using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA.Core
{
    /// <summary>
    /// Тип инвалидации кэша
    /// </summary>
    public enum InvalidationMode
    {
        /// <summary>
        /// Везде
        /// </summary>
        All,
        /// <summary>
        /// только для live 
        /// </summary>
        Live,
        /// <summary>
        /// только для stage
        /// </summary>
        Stage

    }
}
