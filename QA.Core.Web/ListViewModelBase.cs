// Owners: Alexey Abretov, Nikolay Karlov

using System.Collections.Generic;

namespace QA.Core.Web
{
    /// <summary>
    /// Базовая модель представления для списков
    /// </summary>
    /// <typeparam name="T">Тип элементов списка</typeparam>
    public class ListViewModelBase<T> : ViewModelBase
    {
        /// <summary>
        /// Список элементов
        /// </summary>
        public virtual List<T> List
        {
            get;
            set;
        }

        /// <summary>
        /// Настройки постраничного вывода
        /// </summary>
        public virtual PagerViewModel Pager
        {
            get;
            set;
        }
    }
}
