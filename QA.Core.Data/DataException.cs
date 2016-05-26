// Owners: Karlov Nikolay, Abretov Alexey
using System;

namespace QA.Core.Data
{
    /// <summary>
    /// Исключение при работе с данными
    /// </summary>
    [Serializable]
    public class DataException : Exception
    {
        public DataException(string message)
            : base(message)
        {
        }

        public DataException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public static void Throw(string message) 
        {
            throw new DataException(message, null);
        }

        public static void Throw(string message, Exception innerException)
        {
            throw new DataException(message, innerException);
        }
    }
}
