// Owners: Karlov Nikolay, Abretov Alexey
using System;
using System.Collections.Generic;

namespace QA.Core.Web
{
    /// <summary>
    /// Коллекция ошибок валидации
    /// <remarks>
    /// Используется для сериализации ошибок валидации. Ключ - название property, 
    /// объект - как правило сообщение
    /// </remarks>
    /// </summary>
    [Serializable]
    public class ValidationDictionary : Dictionary<string, object>
    {
    }
}
