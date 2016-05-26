using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QA.Core.Performance
{    internal class ConcurrentBuffer<T> : ConcurrentQueue<T>
    {
        private readonly object _sync = new object();
        public int Size { get; private set; }
        public T Latest { get; private set; }
        public ConcurrentBuffer(int size)
        {
            Size = size;
        }

        public new void Enqueue(T obj)
        {
            base.Enqueue(obj);
            lock (_sync)
            {
                Latest = obj;
                while (base.Count > Size)
                {
                    T outObj;
                    base.TryDequeue(out outObj);
                }
            }
        }
    }
}
