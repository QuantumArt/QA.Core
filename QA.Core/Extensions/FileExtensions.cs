using System.IO;

namespace QA.Core
{
    /// <summary>
    /// Расширения для работы с файлами
    /// </summary>
    public static class FileExtensions
    {
        /// <summary>
        /// Считывает все содержимое и закрывает поток.
        /// </summary>
        /// <param name="fileStream">Поток</param>
        /// <returns>Все байты потока</returns>
        public static byte[] ReadAllBytes(this Stream fileStream)
        {
            byte[] buffer;
            int length = (int)fileStream.Length;
            buffer = new byte[length];
            int count;
            int sum = 0;

            while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
            {
                sum += count;
            }

            return buffer;
        }

        /// <summary>
        /// Считывает все содержимое и закрывает поток.
        /// </summary>
        /// <param name="fileStream">Поток</param>
        /// <returns>Все байты потока</returns>
        public static byte[] ReadAllBytesAndClose(this Stream fileStream)
        {
            try
            {
                return ReadAllBytes(fileStream);
            }
            finally
            {
                fileStream.Close();
            }
        }
    }
}
