using System.Reactive.Disposables;

namespace AsteriskAriService.Extensions
{
    public static class DisposableExtensions
    {
        /// <summary>
        /// Attach to a collection for release
        /// </summary>
        /// <param name="disposable"></param>
        /// <param name="composite"></param>
        /// <returns></returns>
        public static IDisposable AddTo(this IDisposable disposable, CompositeDisposable composite)
        {
            composite.Add(disposable);
            return disposable;
        }
    }
}