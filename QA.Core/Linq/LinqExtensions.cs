// Owners: Karlov Nikolay, Abretov Alexey

using System;
using System.Collections.Generic;
using System.Linq;

namespace QA.Core
{
    /// <summary>
    /// Расширения для Linq
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Метод возвращает повторы в коллекции
        /// </summary>
        /// <param name="source">Исходная коллекция.</param>
        /// <param name="distinct">Указать <b>true</b> Чтобы вернуть только уникальные.</param>
        /// <returns>Список элементов с повторами.</returns>
        /// <remarks>Метод-расширение IEnumerable&lt;T&gt;</remarks>
        public static IEnumerable<T> Duplicates<T>(this IEnumerable<T> source, bool distinct = true)
        {
            return source.Duplicates(x => x, distinct);
        }

        /// <summary>
        /// Метод возвращает повторы в коллекции. Повторы определяются по селектору
        /// </summary>
        /// <param name="source">Исходная коллекция.</param>
        /// <param name="selector">Селектор для извлечения поленй, по которым производится сравнение.</param>
        /// <param name="distinct">Указать <b>true</b> Чтобы вернуть только уникальные.</param>
        /// <returns>Список элементов с повторами.</returns>
        /// <remarks>Метод-расширение IEnumerable&lt;T&gt;</remarks>
        public static IEnumerable<TResult> Duplicates<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, bool distinct = true)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            // берем повторные элементы
            IEnumerable<TResult> result = source.Select(selector).GroupBy(a => a).SelectMany(a => a.Skip(1));

            // только уникальные?
            if (distinct == true)
            {
                result = result.Distinct();
            }

            return result;
        }

        /// <summary>
        /// Метод возвращает повторы в коллекции. Повторы определяются по селектору
        /// </summary>
        /// <param name="source">Исходная коллекция.</param>
        /// <param name="selector">Селектор для извлечения поленй, по которым производится сравнение.</param>
        /// <param name="distinct">Указать <b>true</b> Чтобы вернуть только уникальные.</param>
        /// <returns>Список элементов с повторами.</returns>
        /// <remarks>Метод-расширение IEnumerable&lt;T&gt;</remarks>
        public static IEnumerable<TResult> Duplicates<TSource, TResult, TKey>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, Func<TResult, TKey> grouppingSelector, bool distinct = true)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            // берем повторные элементы
            IEnumerable<TResult> result = source.Select(selector).GroupBy(grouppingSelector).SelectMany(a => a.Skip(1));

            // только уникальные?
            if (distinct == true)
            {
                result = result.Distinct();
            }

            return result;
        }
    }
}
