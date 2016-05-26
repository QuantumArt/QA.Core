using System.Collections.Generic;
using AutoMapper;

namespace QA.Core
{
    /// <summary>
    /// Предоставляет методы сопоставления данных
    /// </summary>
    public static class MappingHelper
    {
        /// <summary>
        /// Преобразует один элемент
        /// </summary>
        /// <typeparam name="TSource">Тип исходного элемента</typeparam>
        /// <typeparam name="TDest">Тип конечного элмента</typeparam>
        /// <param name="source">Элемент для преобразования</param>
        /// <returns></returns>
        public static TDest Map<TSource, TDest>(TSource source)
        {
            return Mapper.Map<TSource, TDest>(source);
        }

        /// <summary>
        /// Преобразует один элемент
        /// </summary>
        /// <typeparam name="TSource">Тип исходного элемента</typeparam>
        /// <typeparam name="TDest">Тип конечного элмента</typeparam>
        /// <param name="source">Элемент для преобразования</param>
        /// <returns></returns>
        public static void Map<TSource, TDest>(TSource source, TDest dest)
        {
            Mapper.Map<TSource, TDest>(source, dest);
        }

        /// <summary>
        /// Перобразует список
        /// </summary>
        /// <typeparam name="TSource">Тип исходного элемента</typeparam>
        /// <typeparam name="TDest">Тип конечного элмента</typeparam>
        /// <param name="source">Элемент для преобразования</param>
        /// <returns></returns>
        public static List<TDest> Map<TSource, TDest>(List<TSource> source)
        {
            return Mapper.Map<List<TSource>, List<TDest>>(source);
        }

        /// <summary>
        /// Создает простой маппинг в обе стороны
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        public static void CreateDualMap<T1, T2>()
        {
            Mapper.CreateMap<T1, T2>();
            Mapper.CreateMap<T2, T1>();
        }

        /// <summary>
        /// Создает простой маппинг
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        public static IMappingExpression<T1, T2> CreateMap<T1, T2>()
        {
            return Mapper.CreateMap<T1, T2>();
        }
    }
}
