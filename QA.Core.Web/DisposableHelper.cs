using System;

namespace QA.Core.Web
{
    /// <summary>
    /// Хелпер для disposable объектов
    /// </summary>
    public class DisposableHelper : IDisposable
    {
        private Action _end;

        /// <summary>
        /// When the object is create, write "begin" function
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        public DisposableHelper(Action begin, Action end)
        {
            _end = end;

            begin();
        }

        /// <summary>
        /// When the object is disposed (end of using block), write "end" function
        /// </summary>
        public void Dispose()
        {
            _end();
        }
    }
}
