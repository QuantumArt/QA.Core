using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QA.Core
{
    /// <summary>
    /// Кеш-провайдер, который не кеширует.
    /// </summary>
    public class NoCacheProvider : IVersionedCacheProvider, ICacheProvider
    {
        /// <summary>
        /// Добавление данных в кэш
        /// </summary>
        /// <param name="data">данные</param>
        /// <param name="key">тэг, в общем случае представляет имя класса сервиса + имя метода + список параметров</param>
        /// <param name="tags">список зависимых контентов</param>
        /// <param name="expiration">время жизни в кэше</param>
        public void Add(object data, string key, string[] tags, TimeSpan expiration) { }

        /// <summary>
        /// Получение данных их кэша
        /// </summary>
        /// <param name="key">тэг, в общем случае представляет имя класса сервиса + имя метода + список параметров</param>
        /// <param name="tags">список зависимых контентов</param>
        /// <returns>закэшированне данные, если они присутствуют в кэше</returns>
        public object Get(string key, string[] tags) { return null; }

        /// <summary>
        /// Сообщает, что данные этого тега (контента) обновились
        /// </summary>
        /// <param name="tag">тег</param>
        public void InvalidateByTag(InvalidationMode mode, string tag) { }

        /// <summary>
        /// Удаляет из кеша элемент, у которого данный ключ и набор зависимых тегов.
        /// </summary>
        /// <param name="key">тэг, в общем случае представляет имя класса сервиса + имя метода + список параметров</param>
        /// <param name="tags">список зависимых контентов</param>
        public void Invalidate(string key, string[] tags) { }

        /// <summary>
        /// Получает данные из кеша по ключу
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns></returns>
        public object Get(string key) { return null; }

        /// <summary>
        /// Очищает кеш
        /// </summary>
        /// <param name="key">Ключ</param>
        public void Invalidate(string key) { }

        /// <summary>
        /// Проверяет наличие данных в кеше
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns></returns>
        public bool IsSet(string key) { return false; }

        /// <summary>
        /// Записывает данные в кеш
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="data">Данные</param>
        /// <param name="expiration">Время кеширования</param>
        public void Set(string key, object data, TimeSpan expiration) { }

        /// <summary>
        /// Записывает данные в кеш
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="data">Данные</param>
        /// <param name="cacheTime">Время кеширования в секундах</param>
        public void Set(string key, object data, int cacheTime) { }

        /// <summary>
        /// Проверяет существование объекта и в случае существования возвращает данные.
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="result">дангные</param>
        /// <returns></returns>
        public bool TryGetValue(string key, out object result)
        {
            result = null;
            return false;
        }      

        void IDisposable.Dispose() { }


        public void InvalidateByTags(InvalidationMode mode, params string[] tags)
        {
            
        }

        /// <summary>
        /// Потокобезопасно берет объект из кэша, если его там нет, то вызывает функцию для получения данных
        /// и кладет результат в кэш
        /// </summary>
        /// <typeparam name="T">тип объектов в кэше</typeparam>
        /// <param name="key">тэг, в общем случае представляет имя класса сервиса + имя метода + список параметров</param>
        /// <param name="tags">список зависимых контентов</param>
        /// <param name="expiration">время жизни в кэше</param>
        /// <param name="getData">функция для получения данных, если объектов кэше нет. нужно использовать анонимный делегат</param>
        /// <returns>закэшированне данные, если они присутствуют в кэше или результат выполнения функции</returns>
        public T GetOrAdd<T>(string key, string[] tags, TimeSpan expiration, Func<T> getData)
        {
            return getData();
        }


        /// <summary>
        /// Потокобезопасно берет объект из кэша, если его там нет, то вызывает функцию для получения данных
        /// и кладет результат в кэш
        /// </summary>
        /// <typeparam name="T">тип объектов в кэше</typeparam>
        /// <param name="key">тэг, в общем случае представляет имя класса сервиса + имя метода + список параметров</param>    
        /// <param name="expiration">время жизни в кэше</param>
        /// <param name="getData">функция для получения данных, если объектов кэше нет. нужно использовать анонимный делегат</param>
        /// <returns>закэшированне данные, если они присутствуют в кэше или результат выполнения функции</returns>
        public T GetOrAdd<T>(string key, TimeSpan expiration, Func<T> getData)
        {
            return getData();
        }
    }
}
