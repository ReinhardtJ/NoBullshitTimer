using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using NoBullshitTimer.Client;
using NoBullshitTimer.Client.Application;
using NoBullshitTimer.Client.Domain;
using NoBullshitTimer.Client.Framework;
using NoBullshitTimer.Client.Repositories;
using NoBullshitTimer.Client.Stores;

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

var serviceProvider = builder.Services.BuildServiceProvider();
var jsRuntime = serviceProvider.GetRequiredService<IJSRuntime>();
var storageService = await LocalStorageService.Create(jsRuntime);
builder.Services.AddSingleton<ILocalStorageService>(storageService);
builder.Services.AddSingleton<IWorkoutRepository, LocalStorageWorkoutRepository>();
serviceProvider = builder.Services.BuildServiceProvider();
var workoutRepository = serviceProvider.GetRequiredService<IWorkoutRepository>();
var workoutStore = await WorkoutStore.Create(workoutRepository);
builder.Services.AddSingleton<IWorkoutStore>(workoutStore);
builder.Services.AddSingleton<WorkoutMapper>();
builder.Services.AddSingleton<IntervalTimer>();
await builder.Build().RunAsync();
