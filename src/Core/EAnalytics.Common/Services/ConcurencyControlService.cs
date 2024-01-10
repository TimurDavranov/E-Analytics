using System.Collections.Concurrent;

namespace EAnalytics.Common.Services;

public interface IConcurencyControlService
{
    SemaphoreSlim GetOrAdd(object key, object separator);
    void Release(object key, object separator);
}

public class ConcurencyControlService : IConcurencyControlService
{
    private static ConcurrentDictionary<object, SemaphoreSlim> ConcurrentDictionary = new();

    public SemaphoreSlim GetOrAdd(object key, object separator)
    {
        var semaphore = ConcurrentDictionary.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));
        return semaphore;
    }

    public void Release(object key, object separator)
    {
        var semaphore = GetOrAdd(key, separator);
        semaphore.Release();
        ConcurrentDictionary.TryRemove(key, out _);
    }
}