using System.Collections.Concurrent;

namespace AsteriskAriService.Session
{
    public class SessionStorage
    {
        private ConcurrentDictionary<object, object> Items { get; } = new();
        
        public void Set(object data)
        {
            Items[data.GetType()] = data;
        }

        public T? Get<T>()
        {
            return Items.TryGetValue(typeof(T), out var value)
                ? (T?)value
                : default;
        }
    }
}