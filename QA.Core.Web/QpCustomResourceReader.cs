using System;
using System.Collections;
using System.Resources;

namespace QA.Core.Web
{
    internal sealed class QpCustomResourceReader : IResourceReader
    {
        private IDictionary _resources;

        public QpCustomResourceReader(IDictionary resources)
        {
            _resources = resources;
        }

        IDictionaryEnumerator IResourceReader.GetEnumerator()
        {
            return _resources.GetEnumerator();
        }

        void IResourceReader.Close()
        {
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _resources.GetEnumerator();
        }

        void IDisposable.Dispose()
        {
            return;
        }
    }
}