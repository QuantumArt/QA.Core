using System.IO;

namespace QA.Core.Logger
{
    /// <summary>
    /// Реализация журналирования на основе TextWriter.
    /// </summary>
    public class TextWriterLogger : NullLogger
    {
        private readonly TextWriter _writer;

        /// <summary>
        /// Инициализация экземпляра <see cref="TextWriterLogger"/>
        /// </summary>
        public TextWriterLogger(TextWriter writer)
        {
            Throws.IfArgumentNull(writer, _ => writer);
            _writer = writer;
        }

        /// <summary>
        /// Запись сообщения в трейс-лог
        /// </summary>
        protected override void WriteMessage(string level, string message, string error)
        {
            try
            {
                _writer.WriteLine($"{level}: {message}, {error}");
            }
            catch
            {
                // ignored
            }
        }
    }
}
