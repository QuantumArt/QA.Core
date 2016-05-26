// Owners: Alexey Abretov, Nikolay Karlov

using System;
using System.Runtime.Serialization;

namespace QA.Core.Web
{
    /// <summary>
    /// Базовый класс модели представления
    /// </summary>
    [Serializable]
    public class ViewModelBase
    {
        /// <summary>
        /// Признак наличия/отсутствия ошибок в 
        /// </summary>
        public bool IsSucceeded { get; set; }

        /// <summary>
        /// Ошибка
        /// </summary>
        public JsonError Error { get; set; }

        /// <summary>
        /// Признак отображения элементов модели
        /// </summary>
        public virtual bool IsHideControls
        {
            get;
            set;
        }

        /// <summary>
        /// Пользователь аутентифицировался
        /// </summary>
        public virtual bool IsAuthenticated
        {
            get;
            set;
        }
    }
}
