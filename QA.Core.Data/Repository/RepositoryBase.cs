// Owners: Alexey Abretov, Nikolay Karlov
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
#pragma warning disable 1591


namespace QA.Core.Data.Repository
{
    /// <summary>
    /// Базовый репозиторий
    /// </summary>
    /// <typeparam name="T">Тип сущности</typeparam>
    /// <typeparam name="TId">Тип ключа</typeparam>
    public class RepositoryBase<T, TId> : IRepository<T, TId>
        where T : class
        where TId : struct, IComparable, IConvertible
    {
        /// <summary>
        /// Контекст данных
        /// </summary>
        protected IUnitOfWork UnitOfWork { get; set; }

        /// <summary>
        /// Конструирует объект
        /// </summary>
        /// <param name="unitOfWork">Контекст данных</param>
        public RepositoryBase(IUnitOfWork unitOfWork)
        {
            Throws.IfArgumentNull(unitOfWork, "unitOfWork");

            this.UnitOfWork = unitOfWork;
        }

        /// <summary>
        /// Возвращает все элементы сущности
        /// </summary>
        /// <returns></returns>
        public virtual IList<T> GetAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Возвращает сушность по ключу
        /// </summary>
        /// <param name="id">Ключ</param>
        /// <returns></returns>
        public virtual T GetById(TId id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Возвращает список элементов сущности по ключам
        /// </summary>
        /// <param name="ids">Спсико ключей</param>
        /// <returns></returns>
        public virtual IList<T> GetById(params TId[] ids)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Создает сущность
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <returns></returns>
        public virtual T Create(T entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Удаляет сущность по ключу
        /// </summary>
        /// <param name="id">Ключ</param>
        public virtual void Delete(TId id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Добавляет в запрос постраничную выборку
        /// </summary>
        /// <typeparam name="TQ">Тип запроса</typeparam>
        /// <param name="query">Запрос</param>
        /// <param name="beginIndex">Начальная позиция</param>
        /// <param name="pageSize">Размер страницы</param>
        protected virtual void ApplyPaging<TQ>(ref IQueryable<TQ> query, int? beginIndex, int? pageSize)
        {
            if (beginIndex != null & pageSize != null)
            {
                query = query.Skip((beginIndex.Value == 0 ? beginIndex.Value : beginIndex.Value - 1) * pageSize.Value);
                query = query.Take(pageSize.Value);
            }
        }

        protected virtual TT ReadWithNoLock<TT>(Func<TT> func)
        {
            using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {
                return func();
            }
        }
    }
}
