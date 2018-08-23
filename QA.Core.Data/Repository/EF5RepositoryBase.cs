// Owners: Alexey Abretov, Nikolay Karlov
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace QA.Core.Data.Repository
{
    /// <summary>
    /// Базовый репозиторий EF
    /// </summary>
    /// <typeparam name="T">Тип сущности</typeparam>
    /// <typeparam name="TId">Тип идентификатора</typeparam>
    public class EF5RepositoryBase<T, TId> : RepositoryBase<T, TId>
        where T : class
        where TId : struct, IComparable, IConvertible
    {
        /// <summary>
        /// Конструирует объект
        /// </summary>
        public EF5RepositoryBase(IUnitOfWork unityOfWork)
            : base(unityOfWork)
        {
        }

        /// <summary>
        /// Возвращает набор сущности
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        /// <returns></returns>
        protected virtual IDbSet<TEntity> GetDbSet<TEntity>() where TEntity : class
        {
            return (UnitOfWork.Context as DbContext)?.Set<TEntity>();
        }

        /// <summary>
        /// Возвращает набор сущности
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        /// <returns></returns>
        protected virtual DbSet<TEntity> GetDbSet2<TEntity>() where TEntity : class
        {
            return (UnitOfWork.Context as DbContext)?.Set<TEntity>();
        }

        /// <summary>
        /// Возвращает запрос к сущности
        /// </summary>
        /// <typeparam name="TEntity">Тип сушности</typeparam>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> GetAllQuery<TEntity>() where TEntity : class
        {
            return (UnitOfWork.Context as DbContext)?.Set<TEntity>().AsQueryable<TEntity>();
        }

        /// <summary>
        /// Возвращает все элементы сущности
        /// </summary>
        /// <returns></returns>
        public override IList<T> GetAll()
        {
            return GetDbSet<T>().ToList();
        }

        /// <summary>
        /// Удаляет сущность по ключу
        /// </summary>
        /// <param name="id">Ключ</param>
        public override void Delete(TId id)
        {
            T entity = GetById(id);

            Throws.IfArgumentNull(entity, "entity");

            GetDbSet<T>().Remove(entity);
        }

        /// <summary>
        /// Создает сущность
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <returns></returns>
        public override T Create(T entity)
        {
            Throws.IfArgumentNull(entity, "entity");

            GetDbSet<T>().Add(entity);

            return entity;
        }
    }
}
