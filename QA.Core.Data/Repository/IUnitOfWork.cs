// Owners: Alexey Abretov, Nikolay Karlov
using System;
using System.Data;

namespace QA.Core.Data.Repository
{
    /// <summary>
    /// Контракт UnityOfWork
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Контекст
        /// </summary>
        object Context { get; }

        /// <summary>
        /// Название подключения к БД
        /// </summary>
        string ConnectionName { get; }

        /// <summary>
        /// Применяет изменения
        /// </summary>
        void Commit();

        /// <summary>
        /// Обновляет все элементы, в которых были изменения
        /// </summary>
        void Refresh();

        /// <summary>
        /// Открывает транзакцию
        /// </summary>
        /// <param name="isolationLevel">Уровень изоляции</param>
        /// <returns></returns>
        IDbTransaction BeginTransaction(IsolationLevel isolationLevel);

        /// <summary>
        /// Включить изменения в транзакцию
        /// </summary>
        /// <param name="tran"></param>
        void AttachTransaction(IDbTransaction tran);

        /// <summary>
        /// Откатывает транзакцию
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// Применяет текущую транзакцию.
        /// </summary>
        void CommitTransaction();
    }
}
