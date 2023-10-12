using CSA_AzureApp;
using CSA_AzureApp.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var configurationBuilder = new ConfigurationBuilder()
    .AddJsonFile("wwwroot/appsettings.json", optional: true, reloadOnChange: true);

var configuration = configurationBuilder.Build();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register the configuration as a singleton service
builder.Services.AddSingleton<IConfiguration>(configuration);

// Now you can access the configuration in your services
builder.Services.AddSingleton<IImageAnalysisService, ImageAnalysisService>();
builder.Services.AddSingleton<IExtractTextService, ExtractTextService>();

await builder.Build().RunAsync();
