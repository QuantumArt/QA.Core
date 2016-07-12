using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QA.Core
{
    public sealed class AsyncLock
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly Task<IDisposable> _releaser;

        public AsyncLock()
        {
            _releaser = Task.FromResult((IDisposable)new Releaser(_semaphore));
        }

        public Task<IDisposable> LockAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if(cancellationToken == default(CancellationToken))
            {
                cancellationToken = CancellationToken.None;
            }

            var wait = _semaphore.WaitAsync();
            return wait.IsCompleted ?
                        _releaser :
                        wait.ContinueWith((_, state) => (IDisposable)state,
                            _releaser.Result, cancellationToken,
            TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }

        private sealed class Releaser : IDisposable
        {
            private readonly SemaphoreSlim _semaphore;
            internal Releaser(SemaphoreSlim toRelease)
            {
                _semaphore = toRelease;
            }
            public void Dispose()
            {
                _semaphore.Release();
            }
        }
    }
}
