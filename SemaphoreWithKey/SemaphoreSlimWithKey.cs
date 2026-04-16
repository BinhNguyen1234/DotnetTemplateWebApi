using System.Collections.Concurrent;

namespace SemaphoreWithKey
{
    public class SemaphoreSlimWithKey<T> : IDisposable where T : class
    {
        public static readonly ConcurrentDictionary<T, Lazy<SemaphoreSlimWithKey<T>>> _lockStore = new();

        private volatile int _internalWaitingCount = 0;

        private SemaphoreSlim _internalSemaphoreSlim = default!;

        private readonly T _key = default!;

        private volatile bool _isDisposed = false;


        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }
            if (!disposing)
            {
                return;
            }

            lock (_internalSemaphoreSlim)
            {
                Release();
                if (_internalWaitingCount == 0)
                {
                    CleanUp();
                }
            }

        }

        private void CleanUp()
        {
            _lockStore.TryRemove(_key, out var _);
            _internalSemaphoreSlim.Dispose();
            _isDisposed = true;
        }

        private void Release()
        {
            Interlocked.Decrement(ref _internalWaitingCount);
            if (_internalSemaphoreSlim.CurrentCount < 1)
            {
                _internalSemaphoreSlim.Release();
            }
        }


        private SemaphoreSlimWithKey(T key)
        {
            _key = key;
            _internalSemaphoreSlim = new SemaphoreSlim(1, 1);
        }



        /// <exception cref="OperationCanceledException"><paramref name="cancellationToken"/> was
        /// canceled.</exception>
        public static async Task<SemaphoreSlimWithKey<T>> AcquireLockWith(T key, CancellationToken cancellationToken = default)
        {
            var lazyLock = _lockStore.GetOrAdd(key, new Lazy<SemaphoreSlimWithKey<T>>(() => new SemaphoreSlimWithKey<T>(key)));
            var semaphore = lazyLock.Value;
            await semaphore.AwaitAsync(cancellationToken);
            return semaphore;
        }


        /// <exception cref="OperationCanceledException"><paramref name="cancellationToken"/> was
        /// canceled.</exception>
        private async Task AwaitAsync(CancellationToken cancellationToken)
        {
            try
            {
                Interlocked.Increment(ref _internalWaitingCount);
                await _internalSemaphoreSlim.WaitAsync(cancellationToken);
            }
            catch
            {
                Dispose();
                throw;
            }
        }
        }
có nên lock trong dispose không
}
