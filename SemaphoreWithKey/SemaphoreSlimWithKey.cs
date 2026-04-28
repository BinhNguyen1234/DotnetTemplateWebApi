using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
namespace System.Threading 
{ 
    /// <summary> /// Have to use Dispose() method or using block to release the key. /// Don't provide any method to release the key except Dispose(). /// </summary> /// <typeparam name="T"></typeparam> 
    public sealed class SemaphoreSlimWithKey<T> : IDisposable where T : class 
    { 
        public static readonly ConcurrentDictionary<T, Lazy<SemaphoreSlimWithKey<T>>> _lockStore = new(); 
        private volatile int _internalWaitingCount = 0; 
        private SemaphoreSlim _internalSemaphoreSlim = default!; 
        private readonly T _key = default!; 
        private readonly StrongBox<bool> _isDisposedAndLockObject = new(false); 
        
        /// <summary> /// Must call to Release key and clean up resrouce. 
        /// </summary> 
        public void Dispose() 
        { 
            Dispose(true); 
        } 

        private void Dispose(bool disposing) 
        {
            if (!disposing) 
            { 
                return; 
            } 
            lock (_isDisposedAndLockObject) 
            { 
                if (_isDisposedAndLockObject.Value) 
                { 
                    return; 
                } 
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
            _isDisposedAndLockObject.Value = true; 
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
            _key = key; _internalSemaphoreSlim = new SemaphoreSlim(1, 1); 
        }
        
        /// <summary> /// Accquire a Lock that blocks the current thread base on key. 
        /// First thread reach the key will keep runing and make another thread need to waiting after key was be release or dispose. 
        /// </summary> /// <param name="key"></param> /// <exception cref="OperationCanceledException"><paramref name="cancellationToken"/> was 
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
}