using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Year_2024.Services;

var services = ConfigureServices();
var serviceProvider = services.BuildServiceProvider();

// Hauptlogik ausführen
var solver = serviceProvider.GetRequiredService<AdventSolver>();
await solver.RunAsync();

static ServiceCollection ConfigureServices()
{
    var services = new ServiceCollection();

    services.AddSingleton<AdventInputManager>(); 
    services.AddSingleton<AdventSolver>();       

    return services;
}
