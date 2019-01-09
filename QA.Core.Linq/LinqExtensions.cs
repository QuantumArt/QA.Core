// Owners: Karlov Nikolay, Abretov Alexey

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
#pragma warning disable 1591

namespace QA.Core.Linq
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
        /// <param name="grouppingSelector"></param>
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

        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> list, int numberOfParts)
        {
            int i = 0;
            var splits = from item in list
                         group item by i++ % numberOfParts into part
                         select part.AsEnumerable();
            return splits;
        }

        public static IEnumerable<IList<T>>Section<T>(this IEnumerable<T> source, int length)
        {
            if (length <= 0)
                throw new ArgumentOutOfRangeException("length");

            var section = new List<T>(length);

            foreach (var item in source)
            {
                section.Add(item);

                if (section.Count == length)
                {
                    yield return section.AsReadOnly();
                    section = new List<T>(length);
                }
            }

            if (section.Count > 0)
                yield return section.AsReadOnly();
        }
    }
}
