using ArchnemesisRecipies;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using AutoMapper;
using System.Reflection;
using Blazored.LocalStorage;
using ArchnemesisRecipies.Utility;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// root components
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// services
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddBlazoredLocalStorage();

// Non-DI instance of automapper
AutoMapperConfig.Configure();

await builder.Build().RunAsync();
