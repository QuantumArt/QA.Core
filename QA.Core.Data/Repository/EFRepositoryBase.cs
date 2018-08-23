// Owners: Alexey Abretov, Nikolay Karlov
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;

namespace QA.Core.Data.Repository
{
    /// <summary>
    /// Базовый репозиторий EF
    /// </summary>
    /// <typeparam name="T">Тип сущности</typeparam>
    /// <typeparam name="TId">Тип идентификатора</typeparam>
    public class EFRepositoryBase<T, TId> : RepositoryBase<T, TId>
        where T : class
        where TId : struct, IComparable, IConvertible
    {
        /// <summary>
        /// Конструирует объект
        /// </summary>
        /// <param name="unityOfWork"></param>
        public EFRepositoryBase(IUnitOfWork unityOfWork) : base(unityOfWork)
        {
        }

        /// <summary>
        /// Возвращает набор сущности
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        /// <returns></returns>
        protected virtual IObjectSet<TEntity> GetDbSet<TEntity>() where TEntity : class
        {
            return (this.UnitOfWork.Context as ObjectContext).CreateObjectSet<TEntity>();
        }

        /// <summary>
        /// Возвращает набор сущности
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        /// <returns></returns>
        protected virtual ObjectSet<TEntity> GetDbSet2<TEntity>() where TEntity : class
        {
            return (this.UnitOfWork.Context as ObjectContext).CreateObjectSet<TEntity>();
        }

        /// <summary>
        /// Возвращает запрос к сущности
        /// </summary>
        /// <typeparam name="TEntity">Тип сушности</typeparam>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> GetAllQuery<TEntity>() where TEntity : class
        {
            return (this.UnitOfWork.Context as ObjectContext).CreateObjectSet<TEntity>().AsQueryable<TEntity>();
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

            GetDbSet<T>().DeleteObject(entity);
        }

        /// <summary>
        /// Создает сущность
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <returns></returns>
        public override T Create(T entity)
        {
            Throws.IfArgumentNull(entity, "entity");

            GetDbSet<T>().AddObject(entity);

            return entity;
        }
    }
}
