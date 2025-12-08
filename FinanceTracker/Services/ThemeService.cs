using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace AiFinanceTracker.Services
{
    public class ThemeService
    {
        private readonly IJSRuntime _js;
        public bool IsDarkMode { get; private set; } = false;

        // <-- event so consumers can react when theme changes
        public event Action? OnThemeChanged;

        public ThemeService(IJSRuntime js)
        {
            _js = js;
        }

        public string CurrentTheme => IsDarkMode ? "dark" : "";

        public async Task ToggleTheme()
        {
            IsDarkMode = !IsDarkMode;
            await _js.InvokeVoidAsync("localStorage.setItem", "darkMode", IsDarkMode.ToString());
            OnThemeChanged?.Invoke();          // notify subscribers immediately
        }

        public async Task InitializeAsync()
        {
            var stored = await _js.InvokeAsync<string>("localStorage.getItem", "darkMode");
            if (!string.IsNullOrEmpty(stored) && bool.TryParse(stored, out var mode))
            {
                IsDarkMode = mode;
            }
            OnThemeChanged?.Invoke();          // ensure UI applies theme on startup
        }
    }
}
