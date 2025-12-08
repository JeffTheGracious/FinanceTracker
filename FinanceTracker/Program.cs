using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using AiFinanceTracker.Services;
using FinanceTracker;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register Services
builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<TransactionService>();
builder.Services.AddScoped<ThemeService>();
builder.Services.AddScoped<AIInsightService>();

await builder.Build().RunAsync();
