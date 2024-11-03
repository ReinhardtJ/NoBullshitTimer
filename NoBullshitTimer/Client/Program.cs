using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NoBullshitTimer.Client;
using NoBullshitTimer.Client.Application;
using NoBullshitTimer.Client.Domain;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("NoBullshitTimer.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("NoBullshitTimer.ServerAPI"));

builder.Services.AddSingleton<WorkoutMapper>();
builder.Services.AddSingleton<IWorkoutStore, WorkoutStore>();
builder.Services.AddSingleton<IWorkoutRepository, InMemoryWorkoutRepository>();
builder.Services.AddSingleton<IntervalTimer>();
await builder.Build().RunAsync();
