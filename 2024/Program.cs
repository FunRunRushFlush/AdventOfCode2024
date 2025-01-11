using BenchmarkDotNet.Running;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

Input? input = null;

AnsiConsole.Clear();
AnsiConsole.Write(
    new FigletText("Advent of Code 2024")
        .Centered()
        .Color(Color.Green));

AnsiConsole.Markup("[blue]Welcome to the Advent of Code 2024 Solver Console app created by [link=https://github.com/FunRunRushFlush]FunRunRushFlush[/][/]\n");
AnsiConsole.Markup("[blue][dim](View the source code on GitHub: [link=https://github.com/FunRunRushFlush/AdventOfCode2024]https://github.com/FunRunRushFlush/AdventOfCode2024 )[/][/][/]\n\n");
AnsiConsole.Markup("[yellow]Please paste your Advent of Code session token (you can find it in your browser cookies) and press Enter:[/]\n");

string sessionCookie = Console.ReadLine()?.Trim();

if (string.IsNullOrWhiteSpace(sessionCookie))
{
    AnsiConsole.Markup("[red]No session token provided. Exiting application.[/]\n");
    return;
}

Day01.Part01Animation dayP01 = new();
Day01.Part02 dayP02 = new();

while (true)
{
    AnsiConsole.Clear();
    AnsiConsole.Write(
        new FigletText("Advent of Code 2024")
            .Centered()
            .Color(Color.Green));

    var daySelection = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("[green]Select a Day:[/]")
            .AddChoices("Day 1", "Day 2", "Exit"));

    if (daySelection == "Exit")
    {
        AnsiConsole.Markup("[red]Thank you for using the Advent of Code 2024 app. Goodbye![/]\n");
        break;
    }

    var subLevel = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title($"[blue]Select a part for {daySelection}:[/]")
            .AddChoices("Load Input", "Part 1", "Part 2", "Back"));

    if (subLevel == "Back")
    {
        continue;
    }

    if (daySelection == "Day 1" && subLevel == "Load Input")
    {
        try
        {
            input = await LoadInputFromApi(2024, 1, sessionCookie);
            AnsiConsole.Markup("[green]Input loaded successfully from Advent of Code API![/]\n");
        }
        catch (Exception ex)
        {
            AnsiConsole.Markup($"[red]Failed to load input: {ex.Message}[/]\n");
        }
    }

    if (daySelection == "Day 1" && subLevel == "Part 1")
    {
        if (input == null)
        {
            AnsiConsole.Markup("[red]No input loaded. Please load the input first.[/]\n");
            continue;
        }

        var result = dayP01.Result(input);
        AnsiConsole.Markup($"[yellow]Solution for Day 1 - Part 1: {result}[/]\n");
    }
    else if (daySelection == "Day 1" && subLevel == "Part 2")
    {
        if (input == null)
        {
            AnsiConsole.Markup("[red]No input loaded. Please load the input first.[/]\n");
            continue;
        }

        var result = dayP02.Result(input);
        AnsiConsole.Markup($"[yellow]Solution for Day 1 - Part 2: {result}[/]\n");
    }

    AnsiConsole.Markup("[blue]Press any key to return to the main menu...[/]");
    Console.ReadKey(true);
}

static async Task<Input> LoadInputFromApi(int year, int day, string sessionCookie)
{
    string url = $"https://adventofcode.com/{year}/day/{day}/input";

    using HttpClient client = new();
    client.DefaultRequestHeaders.Add("Cookie", $"session={sessionCookie}");

    HttpResponseMessage response = await client.GetAsync(url);

    if (!response.IsSuccessStatusCode)
    {
        throw new Exception($"Failed to fetch input. HTTP Status: {response.StatusCode}");
    }

    string content = await response.Content.ReadAsStringAsync();

    return new Input(
        System.Text.Encoding.UTF8.GetBytes(content),
        content,
        content.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
    );
}
