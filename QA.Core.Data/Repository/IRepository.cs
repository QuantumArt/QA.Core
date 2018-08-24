// Owners: Alexey Abretov, Nikolay Karlov
using System;
using System.Collections.Generic;
#pragma warning disable 1591


namespace QA.Core.Data.Repository
{
    /// <summary>
    /// Контракт репозитория
    /// </summary>
    /// <typeparam name="T">Тип сущности</typeparam>
    /// <typeparam name="TId">Тип ключа</typeparam>
    public interface IRepository<T, TId>
        where T : class
        where TId : struct, IComparable, IConvertible
    {
        /// <summary>
        /// Получение списка всех объектов данного типа
        /// </summary>
        IList<T> GetAll();

        /// <summary>
        /// Получение сущности по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        T GetById(TId id);

        /// <summary>
        /// Получение сущности по идентификатору
        /// </summary>
        /// <param name="ids">Список идентификаторов объектов</param>
        IList<T> GetById(params TId[] ids);

        /// <summary>
        /// Создание новой сущности в БД
        /// </summary>
        T Create(T entity);

        /// <summary>
        /// Удаление сущности по идентификатору
        /// </summary>
        void Delete(TId id);
    }
}
