using System.Text.Json;
using Microsoft.JSInterop;
using NoBullshitTimer.Client.Domain;

namespace NoBullshitTimer.Client.Framework;


public class LocalStorageService
{
    private readonly IJSRuntime _jsRuntime;

    public static async Task<LocalStorageService> FromState(IJSRuntime jsRuntime)
    {
        var service = new LocalStorageService(jsRuntime);
        var localStorage = await service.GetLocalStorage();

        if (!localStorage.ContainsKey("workouts"))
            await service.SetItemAsync("workouts", JsonSerializer.Serialize(WorkoutPresets.DefaultPresets()));

        return service;
    }

    private LocalStorageService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task SetItemAsync(string key, string value)
    {
        await _jsRuntime.InvokeVoidAsync("localStorageInterop.setItem", key, value);
    }

    public async Task<T> GetItemAsync<T>(string key)
    {
        var serializedItem = await _jsRuntime.InvokeAsync<string>("localStorageInterop.getItem", key);
        if (serializedItem == null)
            throw new KeyNotFoundException($"key {key} not in local storage");
        return JsonSerializer.Deserialize<T>(serializedItem);
    }

    public async Task RemoveItemAsync(string key)
    {
        await _jsRuntime.InvokeVoidAsync("localStorageInterop.removeItem", key);
    }

    public async Task ClearAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorageInterop.clear");
    }

    public async Task<Dictionary<string, object>> GetLocalStorage()
    {
        var serializedLocalStorage = await _jsRuntime.InvokeAsync<string>("localStorageInterop.getLocalStorage");
        var localStorage = JsonSerializer.Deserialize<Dictionary<string, object>>(serializedLocalStorage);
        if (localStorage == null)
            throw new NullReferenceException("local storage not expected to be null");

        return localStorage;
    }
}