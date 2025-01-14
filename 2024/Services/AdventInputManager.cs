using Spectre.Console;
using System.Text;

public class AdventInputManager
{
    private readonly Dictionary<int, Input> _inputs = new();

    //Seems like not all days have a fix input length ...
    private readonly Dictionary<int, int> _validationRule = new()
    { 
        {1,1000 },
        {2,1000 },
        {3,6 },
        {4,140 },
        //{5,1387 },
        {6,130 },
        {7,850 },
        {8,50 },
        {9,1 },
        //{10,45 },
        {11,1 },
        {12,140 },
        {13,1279 },
        {14,500 },
        {15,71 },
        {16,141 },
        {17,5 },
        {18,3450 },
        {19,402 },
        {20,141 },
        {21,5 },
        //{22,1787 },
        {23,3380 },
        {24,313 },
        {25,3999 }
    };


    public async Task LoadAllInputsAsync(int year, string sessionToken)
    {
        var failedDays = new List<int>();

        await AnsiConsole.Progress()
            .AutoClear(false)
            .HideCompleted(false)
            .AutoClear(true)
            .Columns(new ProgressColumn[]
            {
                new TaskDescriptionColumn(),
                new ProgressBarColumn(),
                new PercentageColumn(),
                new RemainingTimeColumn(),
                new SpinnerColumn(),
            })
            .StartAsync(async ctx =>
            {
                var dayTasks = new Dictionary<int, ProgressTask>();
                for (int day = 1; day <= 25; day++)
                {
                    dayTasks[day] = ctx.AddTask($"[yellow]Loading input for Day {day:D2}...[/]");
                }

                foreach (var day in dayTasks.Keys)
                {
                    if (_inputs.ContainsKey(day))
                    {
                        dayTasks[day].Increment(100);
                        continue;
                    }

                    try
                    {
                        string url = $"https://adventofcode.com/{year}/day/{day}/input";
                        using var client = new HttpClient();
                        client.DefaultRequestHeaders.Add("Cookie", $"session={sessionToken}");

                        var response = await client.GetAsync(url);
                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception($"HTTP {response.StatusCode}");
                        }

                        var content = await response.Content.ReadAsStringAsync();
                        content = content.TrimEnd();
                        SetInput(day, content);

                        dayTasks[day].Increment(100);
                    }
                    catch (Exception ex)
                    {
                        failedDays.Add(day);
                        AnsiConsole.MarkupLine($"[red]Error loading input for Day {day}: {ex.Message}[/]");
                        dayTasks[day].StopTask();
                    }
                }
            });

        if (failedDays.Any())
        {
            AnsiConsole.MarkupLine("[red]Failed to load inputs for the following days:[/]");
            foreach (var day in failedDays)
            {
                AnsiConsole.MarkupLine($"[red]- Day {day}[/]");
            }
        }
        else
        {
            AnsiConsole.MarkupLine("[green]All inputs loaded successfully![/]");
        }
    }

    public Input? TryGetInput(int day)
    {
        if (_inputs.TryGetValue(day, out var input)) return input;

        AnsiConsole.Markup($"[red]Input for Day {day} is not available.[/]\n");
        AnsiConsole.Markup($"[yellow]Select one of the following options to load inputdata for Day{day:D2}:[/]\n");
        AnsiConsole.Markup("[yellow] 1. Copy and paste your input string into the command line for a specific day[/]\n");
        AnsiConsole.Markup("[yellow] 2. Provide the full path to the input.txt file (ensure the correct path to avoid crashes with invalid inputs)[/]\n\n");

        var inputTypeSelection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[green]Select an option to obtain inputs:[/]")
                .AddChoices("Copy & Paste", "Input File"));

        //if (inputTypeSelection == "Copy & Paste")
        //{
        //    AnsiConsole.Markup("[yellow]Please paste your input directly into the command line.[/]\n");
        //    AnsiConsole.Markup("[red]To finish entering input, type '__END' (without quotes) on a new line.[/]\n");
        //    AnsiConsole.Markup("[dim]Example:\n" +
        //                       "1234 5678\n" +
        //                       "8765 4321\n" +
        //                       "...\n" +
        //                       "5555 9989\n" +
        //                       "__END[/]\n\n");
        //    var manualInputBuilder = new StringBuilder();
        //    string? line;

        //    while ((line = Console.ReadLine()) != null && line.Trim().ToUpper() != "__END")
        //    {
        //        manualInputBuilder.AppendLine(line.TrimEnd());
        //    }

        //    string manualInput = manualInputBuilder.ToString().Trim();
        //    if (string.IsNullOrEmpty(manualInput))
        //    {
        //        throw new InvalidOperationException("No input provided");
        //    }

        //    SetInput(day, manualInput);
        //}
        if (inputTypeSelection == "Copy & Paste")
        {

            AnsiConsole.MarkupLine("[yellow]Start typing your input. Press [bold]ESC[/] to finish.[/]");
            var manualInputBuilder = new StringBuilder();

            while (true)
            {
                var keyInfo = Console.ReadKey(intercept: true);

                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    AnsiConsole.MarkupLine("\n[green]Input completed! Processing...[/]");
                    break;
                }

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    manualInputBuilder.AppendLine();
                    Console.WriteLine();
                }
                else
                {
                    manualInputBuilder.Append(keyInfo.KeyChar);
                    Console.Write(keyInfo.KeyChar);
                }
            }

            string manualInput = manualInputBuilder.ToString().Trim();
            if (string.IsNullOrEmpty(manualInput))
            {
                AnsiConsole.MarkupLine("[red]No input provided![/]");
                return null;
            }

            SetInput(day, manualInput);
        }
        else if (inputTypeSelection == "Input File")
        {
            AnsiConsole.Markup("[yellow]Please Provide the full path to the input.txt file: (ensure the correct path to avoid crashes with invalid inputs)[/]\n");
            AnsiConsole.Markup("[yellow][dim](Example: C:\\User\\AoC\\Download\\Day01.txt)[/][/]\n");

            string path = Console.ReadLine()?.Trim();
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Input file for Day{day:D2} not found at {path}");
            }
            var inputText = File.ReadAllText(path).TrimEnd();
            SetInput(day, inputText);
        }

        return TryGetInput(day) ?? throw new InvalidOperationException("Failed to store input.");
    }

    private void SetInput(int day, string manualInput)
    {
        if (SoftValidation(day, manualInput))
        {
            _inputs[day] = new Input(
                Encoding.UTF8.GetBytes(manualInput),
                manualInput,
                manualInput.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToArray());
        }
    }

    private bool SoftValidation(int day, string manualInput)
    {
        var lines = manualInput.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToArray();
        var numOfLines = lines.Length;

        if (_validationRule.ContainsKey(day) && _validationRule[day] != numOfLines)
        {
            AnsiConsole.MarkupLine($"[red]Failed to load inputs for the Day{day:D2}:[/]");
            AnsiConsole.MarkupLine($"[red]Expected number of Input Lines: {_validationRule[day]} - got: {numOfLines} [/]");

            return false;
        }

        return true;
    }
}