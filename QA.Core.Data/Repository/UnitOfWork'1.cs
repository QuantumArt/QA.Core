#pragma warning disable 1591

using System.Data;
namespace QA.Core.Data.Repository
{
    /// <summary>
    /// Обобщенная единица работы
    /// </summary>
    /// <typeparam name="TContext">Тип контекста</typeparam>
    public abstract class UnitOfWork<TContext> : IUnitOfWork
    {
        private TContext _context;
        /// <summary>
        /// Контекст
        /// </summary>
        public TContext Context
        {
            get
            {
                if (_context == null)
                {
                    _context = CreateContext();
                }

                return _context;
            }
        }

        /// <summary>
        /// Сохраняет изменения
        /// </summary>
        public abstract void Commit();

        protected virtual bool IsContextCreated
        {
            get
            {
                return _context != null;
            }
        }

        /// <summary>
        /// Создает контекст
        /// </summary>
        /// <returns></returns>
        protected abstract TContext CreateContext();

        /// <summary>
        /// Контекст
        /// </summary>
        object IUnitOfWork.Context
        {
            get { return Context; }
        }

        public abstract string ConnectionName { get; }

        /// <summary>
        /// Обновляет все элементы, в которых были изменения
        /// </summary>
        public abstract void Refresh();

        /// <summary>
        /// Открывает транзакцию
        /// </summary>
        /// <param name="isolationLevel">Уровень изоляции</param>
        /// <returns></returns>
        public abstract IDbTransaction BeginTransaction(IsolationLevel isolationLevel);

        /// <summary>
        /// Откатывает транзакцию
        /// </summary>
        public abstract void RollbackTransaction();

        /// <summary>
        /// Применяет текущую транзакцию.
        /// </summary>
        public abstract void CommitTransaction();

        /// <summary>
        /// Включить изменения в транзакцию
        /// </summary>
        /// <param name="tran"></param>
        public abstract void AttachTransaction(IDbTransaction tran);
    }
}
