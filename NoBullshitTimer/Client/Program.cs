using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using NoBullshitTimer.Client;
using NoBullshitTimer.Client.Application;
using NoBullshitTimer.Client.Domain;
using NoBullshitTimer.Client.Framework;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("NoBullshitTimer.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

// Supply HttpClient instances that include access tokens when making requests
// to the server project
builder.Services.AddScoped(
    sp => sp.GetRequiredService<IHttpClientFactory>()
            .CreateClient("NoBullshitTimer.ServerAPI")
);


builder.Services.AddSingleton<LocalStorageService>(
    provider => LocalStorageService.FromState(provider.GetRequiredService<IJSRuntime>()).GetAwaiter().GetResult()
);
builder.Services.AddSingleton<WorkoutMapper>();
builder.Services.AddSingleton<IWorkoutStore, WorkoutStore>();
builder.Services.AddSingleton<IWorkoutRepository, LocalStorageWorkoutRepository>();
builder.Services.AddSingleton<IntervalTimer>();
await builder.Build().RunAsync();
