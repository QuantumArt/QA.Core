// Owners: Alexey Abretov, Nikolay Karlov

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
#pragma warning disable 1591

namespace QA.Core
{
    /// <summary>
    /// Расширяет Enum
    /// </summary>
    public static class EnumExtender
    {
        /// <summary>
        /// Возвращает локализованное значение перечисления
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static string GetLocalizedDescription(this Enum @enum)
        {
            if (@enum == null)
            {
                return null;
            }

            string description = @enum.ToString();
            FieldInfo fieldInfo = @enum.GetType().GetField(description);
            LocalizedEnumAttribute[] attributes = (LocalizedEnumAttribute[])fieldInfo.GetCustomAttributes(
                typeof(LocalizedEnumAttribute), false);

            if (attributes.Any())
            {
                return attributes[0].Description;
            }

            return description;
        }

        #region GetAttributes<T>
        /// <summary>
        /// Получение коллекции атрибутов
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetAttributes<T>(this Enum value) where T : Attribute
        {
            if (value != null)
            {
                return (T[])value
                                 .GetType()
                                 .GetField(value.ToString())
                                 .GetCustomAttributes(typeof(T), true);
            }
            return null;
        }
        #endregion
        #region GetSingleAttribute<T>
        /// <summary>
        /// Получение одного атрибута
        /// </summary>
        /// <typeparam name="T">Тип атрибута</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T GetSingleAttribute<T>(this Enum value) where T : Attribute
        {
            return value.GetAttributes<T>().Single();
        }
        #endregion

        #region GetDescription
        /// <summary>
        /// Получение атрибута описания
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            return value.GetSingleAttribute<DescriptionAttribute>().Description;
        }

        /// <summary>
        /// Получение атрибута описания
        /// </summary>
        /// <returns></returns>
        public static int GetNumberByDescription(
            this Enum @enum,
            string description)
        {
            foreach (FieldInfo fInfo in
                @enum.GetType().GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                string descr = ((Enum)fInfo.GetValue(null)).GetDescription();
                if (descr == description)
                {
                    return (int)fInfo.GetValue(null);
                }
            }

            return int.MinValue;
        }

        #endregion
    }
}
