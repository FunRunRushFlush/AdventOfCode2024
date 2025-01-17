using Spectre.Console;

public class AdventSolver
{
    private readonly AdventInputManager _inputManager;
    private string SessionToken = string.Empty;

    private IPart? part01;
    private IPart? part01Animation;
    private IPart? part02;
    private IPart? part02Animation;

    public AdventSolver(AdventInputManager inputManager)
    {
        _inputManager = inputManager;
    }

    public async Task RunAsync()
    {
        try
        {

            ShowWelcomeMessage();
            if (string.IsNullOrEmpty(SessionToken))
            {
                await InputOptionSelect();
            }

            AnsiConsole.Clear();
            while (true)
            {
                string daySelection = ShowMainMenu();

                if (daySelection == "Exit")
                {
                    AnsiConsole.Markup("[red]Thank you for using the Advent of Code Solver. Goodbye![/]\n");
                    break;
                }

                int day = int.Parse(daySelection.Replace("Day ", ""));
                await SolveDayAsync(day);
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.Clear();
            AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
            AnsiConsole.Markup("[red]Oooopps... something went wrong![/]\n");
            AnsiConsole.Markup("[blue]Press any key to return to the menu...[/]");
            Console.ReadKey(true);
            await this.RunAsync();
        }
    }

    private async Task InputOptionSelect()
    {
        AnsiConsole.Markup("[yellow]Follow the instructions to provide your inputs and solve the daily challenges![/]\n\n");
        AnsiConsole.Markup("[yellow]You have three options to get your Daily Problem Input:[/]\n");
        AnsiConsole.Markup("[yellow] 1. Enter your Cookie Session Token (recommended)[/]\n");
        AnsiConsole.Markup("[yellow] 2. Copy and paste your input string into the command line for a specific day[/]\n");
        AnsiConsole.Markup("[yellow] 3. Provide the full path to the input.txt file (ensure the correct path to avoid crashes with invalid inputs)[/]\n");

        var inputTypeSelection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[green]Select an option to obtain inputs:[/]")
                .AddChoices("SessionToken", "Copy & Paste", "Input File"));

        if (inputTypeSelection == "SessionToken")
        {
            string sessionToken = GetSessionToken();

            if (string.IsNullOrWhiteSpace(sessionToken))
            {
                AnsiConsole.Markup("[red]No session token provided. Exiting application.[/]\n");
                return;
            }
            SessionToken = sessionToken;
            await _inputManager.LoadAllInputsAsync(2024, sessionToken);
        }
    }

    private void ShowWelcomeMessage()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new FigletText("Advent of Code 2024").Centered().Color(Color.Green));
        AnsiConsole.Markup("[blue]Welcome to the Advent of Code 2024 Solver created by [link=https://github.com/FunRunRushFlush]FunRunRushFlush[/][/]\n");
        AnsiConsole.Markup("[blue][dim](View the source code on GitHub: [link=https://github.com/FunRunRushFlush/AdventOfCode2024]https://github.com/FunRunRushFlush/AdventOfCode2024 )[/][/][/]\n\n");
    }

    private string GetSessionToken()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<string>("[yellow]Please enter your Advent of Code session token: [dim](~135 chars)[/]\n[/]")
                .Secret());
    }

    private string ShowMainMenu()
    {
        ShowWelcomeMessage();

        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[green]Select a Day or Exit:[/]")
                .AddChoices(GetDayChoices()));
    }

    private async Task SolveDayAsync(int day)
    {
        var input = await GetInputForDayAsync(day);

        string partSelection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"[blue]Select a part for Day {day}:[/]")
                .AddChoices("Part 1", "Part 1 - Animation", "Part 2", "Part 2 - Animation", "Back"));

        if (partSelection == "Back") return;

        SetupDayFunction(day);

        AnsiConsole.Clear();

        string result = partSelection switch
        {
            "Part 1" => part01?.Result(input) ?? "[red]Part 1 not implemented[/]",
            "Part 2" => part02?.Result(input) ?? "[red]Part 2 not implemented[/]",
            "Part 1 - Animation" => part01Animation?.Result(input) ?? "[red]Part 1 - Animation not implemented[/]",
            "Part 2 - Animation" => part02Animation?.Result(input) ?? "[red]Part 2 - Animation not implemented[/]",

            _ => throw new InvalidOperationException("Invalid selection.")
        };

        AnsiConsole.Markup($"[yellow]Result for Day {day} - {partSelection}: {result}[/]\n");
        AnsiConsole.Markup("[blue]Press any key to return to the menu...[/]");
        Console.ReadKey(true);
    }

    private async Task<Input> GetInputForDayAsync(int day)
    {
        return _inputManager.TryGetInput(day) ?? throw new InvalidOperationException("Failed to store input.");
    }

    private void SetupDayFunction(int day)
    {
        string dayString = $"Day{day:D2}";
        part01 = CreateInstance($"{dayString}.Part01");
        try
        {
            part01Animation = CreateInstance($"{dayString}.Part01Animation");
        }
        catch (Exception)
        {
            part01Animation = null;
        }

        part02 = CreateInstance($"{dayString}.Part02");
        try
        {
            part02Animation = CreateInstance($"{dayString}.Part02Animation");
        }
        catch (Exception)
        {
            part02Animation = null;
        }
    }

    private IPart CreateInstance(string typeName)
    {
        string fullTypeName = $"Year_2024.Days.{typeName}, Year_2024";
        var type = Type.GetType(fullTypeName) ?? throw new ArgumentException($"Type {fullTypeName} not found");
        return (IPart)Activator.CreateInstance(type);
    }

    private List<string> GetDayChoices()
    {
        var days = Enumerable.Range(1, 25).Select(day => $"Day {day}").ToList();
        days.Add("Exit");
        return days;
    }
}
