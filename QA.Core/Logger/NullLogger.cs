using System;
using System.Linq;

namespace QA.Core.Logger
{
    /// <summary>
    /// Логгер, который использует 
    /// System.Diagnostics.Trace.WriteLine.
    /// </summary>
    public class NullLogger : ILogger
    {
        public void ErrorException(string message, Exception exception, params object[] parameters)
        {
            var errorMsg = CreateErrorMessage(exception);
            WriteMessage("Error", message, errorMsg);
        }

        public void Info(string message, params object[] parameters)
        {
            WriteMessage("Info", message);
        }

        public void Info(Func<string, string> message, params object[] parameters)
        {
            WriteMessage("Info", message(null));
        }

        public void Debug(string message, params object[] parameters)
        {
            WriteMessage("Debug", message);
        }

        public void Debug(Func<string, string> message, params object[] parameters)
        {
            WriteMessage("Debug", message(null));
        }

        public void Fatal(string message, Exception exception, params object[] parameters)
        {
            var errorMsg = CreateErrorMessage(exception);
            WriteMessage("Fatal", message, errorMsg);
        }

        public void Error(Func<string, string> message, params object[] parameters)
        {
            WriteMessage("Error", message(null));
        }

        public void Error(string message, params object[] parameters)
        {
            WriteMessage("Error", message);
        }

        public void Fatal(string message, params object[] parameters)
        {
            WriteMessage("Fatal", message);
        }

        public void Fatal(Func<string, string> message, params object[] parameters)
        {
            WriteMessage("Fatal", message(null));
        }

        public void Dispose() { }

        private void WriteMessage(string level, string message)
        {
            WriteMessage(level, message, null);
        }

        protected virtual void WriteMessage(string level, string message, string error)
        {
            System.Diagnostics.Trace.WriteLine(string.Format("***************************************************************"));
            System.Diagnostics.Trace.WriteLine(string.Format("{0} {1}: {2} {3}", DateTime.Now, level.ToUpper(), message, error));
            System.Diagnostics.Trace.WriteLine(string.Format("***************************************************************"));
        }

        private static string CreateErrorMessage(Exception exception)
        {
            var errorMsg = string.Join("\n\r", exception.Flat().Select(x => x.Message));
            return errorMsg;
        }


        #region ILogger Members


        public void Log(Func<string> message, Action<System.Collections.Generic.IDictionary<object, object>> propertiesSetter, EventLevel eventLevel)
        {
            WriteMessage(eventLevel.ToString(), message());
        }

        public void Log(Func<string> message, EventLevel eventLevel)
        {
            WriteMessage(eventLevel.ToString(), message());
        }

        #endregion
    }
}
