using Blazored.LocalStorage;

using DuckpondExample.Shared.Common.Extensions;

namespace DuckpondExample.Web.UtilityServices;

/// <summary>
/// Service for managing temporary storage using local storage.
/// Implements the <see cref="ILocalStorageService"/> interface.
/// </summary>
public class TemporaryStorageService : ILocalStorageService
{
    public event EventHandler<ChangingEventArgs> Changing;
    public event EventHandler<ChangedEventArgs> Changed;

    private Dictionary<string, object> _storage = new Dictionary<string, object>();

    /// <summary>
    /// Clears all items from the storage.
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async ValueTask ClearAsync(CancellationToken cancellationToken = default)
    {
        _storage.Clear();
    }

    /// <summary>
    /// Checks if the storage contains a specific key.
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the key exists.</returns>
    public async ValueTask<bool> ContainKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        return _storage.ContainsKey(key);
    }

    /// <summary>
    /// Gets an item from the storage as a string.
    /// </summary>
    /// <param name="key">The key of the item to get.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the item as a string.</returns>
    public async ValueTask<string?> GetItemAsStringAsync(string key, CancellationToken cancellationToken = default)
    {
        return _storage.ContainsKey(key) ? _storage[key].ToString() : null;
    }

    /// <summary>
    /// Gets an item from the storage.
    /// </summary>
    /// <typeparam name="T">The type of the item to get.</typeparam>
    /// <param name="key">The key of the item to get.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the item.</returns>
    public async ValueTask<T?> GetItemAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        return _storage.ContainsKey(key) ? (T)_storage[key] : default;
    }

    /// <summary>
    /// Gets the key at a specific index.
    /// </summary>
    /// <param name="index">The index of the key to get.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the key.</returns>
    public async ValueTask<string?> KeyAsync(int index, CancellationToken cancellationToken = default)
    {
        return _storage.ElementAt(index).Key;
    }

    /// <summary>
    /// Gets all keys from the storage.
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable of keys.</returns>
    public async ValueTask<IEnumerable<string>> KeysAsync(CancellationToken cancellationToken = default)
    {
        return _storage.Keys;
    }

    /// <summary>
    /// Gets the number of items in the storage.
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of items.</returns>
    public async ValueTask<int> LengthAsync(CancellationToken cancellationToken = default)
    {
        return _storage.Count;
    }

    /// <summary>
    /// Removes an item from the storage.
    /// </summary>
    /// <param name="key">The key of the item to remove.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async ValueTask RemoveItemAsync(string key, CancellationToken cancellationToken = default)
    {
        _storage.Remove(key);
    }

    /// <summary>
    /// Removes multiple items from the storage.
    /// </summary>
    /// <param name="keys">The keys of the items to remove.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async ValueTask RemoveItemsAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default)
    {
        keys.ForEach(key => _storage.Remove(key));
    }

    /// <summary>
    /// Sets an item in the storage as a string.
    /// </summary>
    /// <param name="key">The key of the item to set.</param>
    /// <param name="data">The data to set.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async ValueTask SetItemAsStringAsync(string key, string data, CancellationToken cancellationToken = default)
    {
        if (_storage.ContainsKey(key))
        {
            _storage[key] = data;
        }
        else
        {
            _storage.Add(key, data);
        }
    }

    /// <summary>
    /// Sets an item in the storage.
    /// </summary>
    /// <typeparam name="T">The type of the item to set.</typeparam>
    /// <param name="key">The key of the item to set.</param>
    /// <param name="data">The data to set.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async ValueTask SetItemAsync<T>(string key, T data, CancellationToken cancellationToken = default)
    {
        if (data == null) return;

        if (_storage.ContainsKey(key))
        {
            _storage[key] = data;
        }
        else
        {
            _storage.Add(key, data);
        }
    }
}
