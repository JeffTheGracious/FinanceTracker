using System.Text.Json;
using Microsoft.JSInterop;

namespace AiFinanceTracker.Services;

public class LocalStorageService
{
    private readonly IJSRuntime _js;
    public LocalStorageService(IJSRuntime js) { _js = js; }

    public async Task SetItemAsync<T>(string key, T item)
    {
        var json = JsonSerializer.Serialize(item);
        await _js.InvokeVoidAsync("localStorage.setItem", key, json);
    }

    public async Task<T?> GetItemAsync<T>(string key)
    {
        var json = await _js.InvokeAsync<string>("localStorage.getItem", key);
        if (string.IsNullOrEmpty(json)) return default;
        return JsonSerializer.Deserialize<T>(json);
    }
}
