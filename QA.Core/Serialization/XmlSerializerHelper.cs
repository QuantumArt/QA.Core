using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace QA.Core.Serialization
{
    /// <summary>
    /// Сериализатор
    /// </summary>
    public static class XmlSerializerHelper
    {
        /// <summary>
        /// Десериализация
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string text)
        {
            try
            {
                var x = new XmlSerializer(typeof(T));
                UTF8Encoding encoding = new UTF8Encoding();
                using (var ms = new MemoryStream(encoding.GetBytes(text)))
                {
                    return (T)x.Deserialize(ms);
                }
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// Сериализация
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize<T>(T obj)
        {
            try
            {
                var x = new XmlSerializer(typeof(T));

                var encoding = new UTF8Encoding();
                using (var ms = new MemoryStream())
                {
                    using (var tx = XmlWriter.Create(ms))
                    {
                        x.Serialize(tx, obj);

                        return encoding.GetString(ms.GetBuffer());
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
