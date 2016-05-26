// Owners: Alexey Abretov, Nikolay Karlov

namespace QA.Core.Web
{
    /// <summary>
    /// Модель представления для постраничный списков
    /// </summary>
    public class PagerViewModel : ViewModelBase
    {
        /// <summary>
        /// Общее количество страниц
        /// </summary>
        public virtual int TotalPageCount
        {
            get
            {
                return TotalCount / PageSize;
            }
        }

        /// <summary>
        /// Общее количество элементов
        /// </summary>
        public virtual int TotalCount
        {
            get;
            set;
        }

        /// <summary>
        /// Размер старницы
        /// </summary>
        public virtual int PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// Текущая страница
        /// </summary>
        public virtual int CurrentPage
        {
            get;
            set;
        }

        /// <summary>
        /// Действие
        /// </summary>
        public virtual string Action
        {
            get;
            set;
        }

        /// <summary>
        /// Контроллер
        /// </summary>
        public virtual string Controller
        {
            get;
            set;
        }

        /// <summary>
        /// Дополнительные значения для route
        /// </summary>
        public virtual object RouteValues
        {
            get;
            set;
        }

        /// <summary>
        /// Стиль
        /// </summary>
        public virtual string ClassName
        {
            get;
            set;
        }

        /// <summary>
        /// Показывать или не показывать ссылки на последнюю и первую страницы.
        /// True - показывать, False - не показывать
        /// </summary>
        public virtual bool ShowFirstLast
        {
            get;
            set;
        }

        /// <summary>
        /// Показывать или не показывать ссылки на предыдущую и следюущую страницы.
        /// True - показывать, False - не показывать
        /// </summary>
        public virtual bool ShowPrevNext
        {
            get;
            set;
        }
    }
}
