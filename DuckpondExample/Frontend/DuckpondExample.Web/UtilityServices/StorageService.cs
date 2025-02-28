using Blazored.LocalStorage;

namespace DuckpondExample.Web.UtilityServices;


/// <summary>
/// Service for managing storage using local storage or temporary storage based on configuration.
/// Implements the <see cref="ILocalStorageService"/> interface.
/// </summary>
public class StorageService : ILocalStorageService
{
    private ILogger<StorageService> logger;
    private ILocalStorageService localStorage;
    private TemporaryStorageService temporaryStorage;
    private bool _isInitialized = false;
    /// <summary>
    /// Event triggered when an item is changing.
    /// </summary>
    public event EventHandler<ChangingEventArgs> Changing;

    /// <summary>
    /// Event triggered when an item has changed.
    /// </summary>
    public event EventHandler<ChangedEventArgs> Changed;

    /// <summary>
    /// Indicates whether to use local storage.
    /// </summary>
    public bool _UseLocalStorage { get; private set; } = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="StorageService"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="localStorage">The local storage service instance.</param>
    /// <param name="temporaryStorage">The temporary storage service instance.</param>
    public StorageService(ILogger<StorageService> logger, ILocalStorageService localStorage, TemporaryStorageService temporaryStorage)
    {
        this.logger = logger;
        this.localStorage = localStorage;
        this.temporaryStorage = temporaryStorage;
    }

    /// <summary>
    /// Initializes the storage service by checking if local storage should be used.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async ValueTask Initalize()
    {
        if (_isInitialized) return;
        var item = await localStorage.ContainKeyAsync("useLocalStorage");
        if (item)
        {
            _UseLocalStorage = await localStorage.GetItemAsync<bool>("useLocalStorage");
        }
        else
        {
            _UseLocalStorage = false;
        }
        _isInitialized = true;
    }

    /// <summary>
    /// Sets whether to use local storage and optionally remembers the choice.
    /// </summary>
    /// <param name="rememberMe">If set to true, the choice is remembered in local storage.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async ValueTask SetUseLocalStorage(bool rememberMe)
    {
        await Initalize();
        _UseLocalStorage = rememberMe;
        if (rememberMe)
        {
            await localStorage.SetItemAsync("useLocalStorage", true);
        }
        else
        {
            await localStorage.ClearAsync();
        }
    }

    public async ValueTask ClearAsync(CancellationToken cancellationToken = default)
    {
        await Initalize();
        if (_UseLocalStorage)
        {
            await localStorage.ClearAsync(cancellationToken);
        }
        else
        {
            await temporaryStorage.ClearAsync(cancellationToken);
        }
    }

    public async ValueTask<T?> GetItemAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        await Initalize();
        if (_UseLocalStorage)
        {
            return await localStorage.GetItemAsync<T>(key, cancellationToken);
        }
        else
        {
            return await temporaryStorage.GetItemAsync<T>(key, cancellationToken);
        }
    }

    public async ValueTask<string?> GetItemAsStringAsync(string key, CancellationToken cancellationToken = default)
    {
        await Initalize();
        if (_UseLocalStorage)
        {
            return await localStorage.GetItemAsStringAsync(key, cancellationToken);
        }
        else
        {
            return await temporaryStorage.GetItemAsStringAsync(key, cancellationToken);
        }
    }

    public async ValueTask<string?> KeyAsync(int index, CancellationToken cancellationToken = default)
    {
        await Initalize();
        if (_UseLocalStorage)
        {
            return await localStorage.KeyAsync(index, cancellationToken);
        }
        else
        {
            return await temporaryStorage.KeyAsync(index, cancellationToken);
        }
    }

    public async ValueTask<IEnumerable<string>> KeysAsync(CancellationToken cancellationToken = default)
    {
        await Initalize();
        if (_UseLocalStorage)
        {
            return await localStorage.KeysAsync(cancellationToken);
        }
        else
        {
            return await temporaryStorage.KeysAsync(cancellationToken);
        }
    }

    public async ValueTask<bool> ContainKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        await Initalize();
        if (_UseLocalStorage)
        {
            return await localStorage.ContainKeyAsync(key, cancellationToken);
        }
        else
        {
            return await temporaryStorage.ContainKeyAsync(key, cancellationToken);
        }
    }

    public async ValueTask<int> LengthAsync(CancellationToken cancellationToken = default)
    {
        await Initalize();
        if (_UseLocalStorage)
        {
            return await localStorage.LengthAsync(cancellationToken);
        }
        else
        {
            return await temporaryStorage.LengthAsync(cancellationToken);
        }
    }

    public async ValueTask RemoveItemAsync(string key, CancellationToken cancellationToken = default)
    {
        await Initalize();
        if (_UseLocalStorage)
        {
            await localStorage.RemoveItemAsync(key, cancellationToken);
        }
        else
        {
            await temporaryStorage.RemoveItemAsync(key, cancellationToken);
        }
    }

    public async ValueTask RemoveItemsAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default)
    {
        await Initalize();
        if (_UseLocalStorage)
        {
            await localStorage.RemoveItemsAsync(keys, cancellationToken);
        }
        else
        {
            await temporaryStorage.RemoveItemsAsync(keys, cancellationToken);
        }
    }

    public async ValueTask SetItemAsync<T>(string key, T data, CancellationToken cancellationToken = default)
    {
        await Initalize();
        if (_UseLocalStorage)
        {
            await localStorage.SetItemAsync(key, data, cancellationToken);
        }
        else
        {
            await temporaryStorage.SetItemAsync(key, data, cancellationToken);
        }
    }

    public async ValueTask SetItemAsStringAsync(string key, string data, CancellationToken cancellationToken = default)
    {
        await Initalize();
        if (_UseLocalStorage)
        {
            await localStorage.SetItemAsStringAsync(key, data, cancellationToken);
        }
        else
        {
            await temporaryStorage.SetItemAsStringAsync(key, data, cancellationToken);
        }
    }
}
