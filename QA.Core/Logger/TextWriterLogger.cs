using System;
using System.IO;
using System.Linq;

namespace QA.Core.Logger
{
    /// <summary>
    /// Логгер, который использует  TextWriter
    /// </summary>
    public class TextWriterLogger : NullLogger
    {
        private TextWriter _writer;
        public TextWriterLogger(TextWriter writer)
        {
            Throws.IfArgumentNull(writer, _ => writer);
            _writer = writer;
        }
        protected override void WriteMessage(string level, string message, string error)
        {
           _writer.WriteLine(string.Format("{0}: {1}, {2}", level, message, error));
        }
    }
}
