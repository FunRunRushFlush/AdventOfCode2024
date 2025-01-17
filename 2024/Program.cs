using BenchmarkDotNet.Running;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;



try
{
    if (args.Length > 0)
    {
        if (args.Contains("benchmark", StringComparer.OrdinalIgnoreCase))
        {
            AnsiConsole.MarkupLine("[yellow]Starting benchmark tests...[/]");
            BenchmarkRunner.Run<DayBenchmark>();
            AnsiConsole.MarkupLine("[green]Benchmarks completed successfully.[/]");
            return;
        }
        
        DisplayHelp();  
        
        Console.ReadKey(true);
        return;
    }

    var services = ConfigureServices();
    var serviceProvider = services.BuildServiceProvider();

    var solver = serviceProvider.GetRequiredService<AdventSolver>();
    await solver.RunAsync();

    static ServiceCollection ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddSingleton<AdventInputManager>();
        services.AddSingleton<AdventSolver>();

        return services;
    }

}
catch (Exception ex)
{
    AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
    AnsiConsole.Markup("[red]An unexpected error occurred.[/]");
}



static void DisplayHelp()
{
    AnsiConsole.MarkupLine("[bold green]Usage:[/]");
    AnsiConsole.MarkupLine("[yellow]dotnet run <options>[/]");
    AnsiConsole.MarkupLine("\nOptions:");
    AnsiConsole.MarkupLine("[cyan]  benchmark[/]       Run the benchmarking tests (must be in Release mode!).");
    AnsiConsole.MarkupLine("\nExamples:");
    AnsiConsole.MarkupLine("[dim]  dotnet run -c Release benchmark[/]");
    AnsiConsole.MarkupLine("[dim]  dotnet run  (runs the basic program)[/]");
}

