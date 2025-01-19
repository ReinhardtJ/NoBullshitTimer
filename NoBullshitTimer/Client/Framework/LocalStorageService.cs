using System.Text.Json;
using Microsoft.JSInterop;
using NoBullshitTimer.Client.Domain;

namespace NoBullshitTimer.Client.Framework;

public interface ILocalStorageService
{
    Task SetItemAsync(string key, string value);
    Task<T?> GetItemAsync<T>(string key);
    Task RemoveItemAsync(string key);
}

public class LocalStorageService : ILocalStorageService
{
    private readonly IJSRuntime _jsRuntime;

    private LocalStorageService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public static async Task<LocalStorageService> Create(IJSRuntime jsRuntime)
    {
        var self = new LocalStorageService(jsRuntime);
        var workouts = await self.GetItemAsync<Dictionary<string, Workout>>("workouts");

        if (workouts == null)
            await self.SetItemAsync("workouts", JsonSerializer.Serialize(WorkoutPresets.DefaultPresets()));

        return self;
    }

    public async Task SetItemAsync(string key, string value)
    {
        await _jsRuntime.InvokeVoidAsync("localStorageInterop.setItem", key, value);
    }

    /// <summary>
    /// Get an item from local storage
    /// </summary>
    /// <param name="key"></param>
    /// <typeparam name="T">The type of the item</typeparam>
    /// <returns>the item T or null if the item wasn't found in the local storage</returns>
    public async Task<T?> GetItemAsync<T>(string key)
    {
        var serializedItem = await _jsRuntime.InvokeAsync<string?>("localStorageInterop.getItem", key);
        if (serializedItem == null)
            return default;
        return JsonSerializer.Deserialize<T>(serializedItem);
    }

    public async Task RemoveItemAsync(string key)
    {
        await _jsRuntime.InvokeVoidAsync("localStorageInterop.removeItem", key);
    }   
}