using ByteLink.Web.Services.Interfaces;

namespace ByteLink.Web.Services;

public class LocalStorageService : ILocalStorageService
{
    private readonly Dictionary<string, object> _storage = new();

    public async Task<T?> GetItemAsync<T>(string key)
    {
        await Task.Delay(1); // Simulate async
        if (_storage.TryGetValue(key, out var value))
        {
            return (T?)value;
        }
        return default;
    }

    public async Task SetItemAsync<T>(string key, T value)
    {
        await Task.Delay(1); // Simulate async
        _storage[key] = value!;
    }

    public async Task RemoveItemAsync(string key)
    {
        await Task.Delay(1); // Simulate async
        _storage.Remove(key);
    }
}
