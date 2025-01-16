using Spectre.Console;
using System.Numerics;

namespace Year_2024.Days.Day07;

public class Part01Animation : IPart
{

    public string Result(Input rawInput)
    {
        long resCounter = 0;

        AnsiConsole.Live(new Panel("Initializing...").Expand())
            .Start(ctx =>
            {
                var coocking = new Panel(Animation_CookingRecipe())
                               .Expand()
                               .Border(BoxBorder.Rounded);

                ctx.UpdateTarget(coocking);


                Console.ReadKey(true);

                foreach (var line in rawInput.Lines)
                {
                    long[] calc = line.Split(new[] { ' ', ':' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(long.Parse).ToArray();

                    long targetNum = calc[0];
                    long[] nums = calc[1..];
                    var numsStrg = string.Join(" ", nums);
                    
                    var grid = new Grid().Expand().Centered();
                    grid.AddColumn(); // Eine Spalte für vertikales Layout

                    // Processing-Text
                    grid.AddRow(new Markup($"[yellow]Processing Target:[/] [bold blue]{targetNum}[/]: {numsStrg}"));

                    ctx.UpdateTarget(grid);

                    // Berechnungsschritte-Tabelle generieren
                    var stepsTable = CanBeCalculatedWithSteps(targetNum, nums);

                    if (stepsTable != null)
                    {
                        // Tabelle hinzufügen
                        grid.AddRow(stepsTable);

                        // Erfolgsnachricht hinzufügen
                        grid.AddRow(new Markup($"[yellow]Solution found for:[/] [blue]{targetNum}:[/] [fuchsia]{numsStrg}[/]"));

                        ctx.UpdateTarget(grid);
                    }
                    else
                    {
                        // Fehlermeldung hinzufügen
                        grid.AddRow(new Markup($"[red]No solution found for this input:[/] [bold blue]{targetNum}[/]"));

                        ctx.UpdateTarget(grid);
                    }


                        var key = Console.ReadKey(true).Key;
   
                        if (key == ConsoleKey.Escape)
                        {
                            break;
                        }

                   
                }
            });

        return resCounter.ToString();
    }



    public Table? CanBeCalculatedWithSteps(long targetNum, long[] nums)
    {
        var stepsTable = new Table()
            .AddColumn("[blue]Operation[/]")
            .AddColumn("[green]Result[/]")
            .Centered();

        if (nums.Length == 2)
        {

            if (targetNum / nums[1] == nums[0] && targetNum % nums[1] == 0)
            {
                stepsTable.AddRow($"[blue]{targetNum}[/] ÷ [fuchsia]{nums[1]}[/]", $"[fuchsia]{nums[0]}[/]");
     
                return stepsTable; 
            }

            if (targetNum - nums[1] == nums[0])
            {
                stepsTable.AddRow($"[blue]{targetNum}[/] - [fuchsia]{nums[1]}[/]", $"[fuchsia]{nums[0]}[/]");
         
                return stepsTable; 
            }

            return null; // Keine Lösung
        }

        if (nums.Length > 2)
        {
            long lastNum = nums[^1];

            if (targetNum % lastNum == 0)
            {
                stepsTable.AddRow($"[blue]{targetNum}[/] ÷ [fuchsia]{lastNum}[/]", $"[green]{targetNum / lastNum}[/]");
                if (CanBeCalculated(targetNum / lastNum, nums[..^1], stepsTable))
                {
                    return stepsTable; 
                }
            }

            if (targetNum - lastNum >= 0)
            {
                stepsTable.AddRow($"[blue]{targetNum}[/] - [fuchsia]{lastNum}[/]", $"[green]{targetNum - lastNum}[/]");
                if (CanBeCalculated(targetNum - lastNum, nums[..^1], stepsTable))
                {
                    return stepsTable; 
                }
            }

        }

        return null; // Keine Lösung gefunden
    }

    private bool CanBeCalculated(long targetNum, long[] nums, Table stepsTable)
    {
        if (nums.Length == 2)
        {
            if (targetNum / nums[1] == nums[0] && targetNum % nums[1] == 0)
            {
                stepsTable.AddRow($"[white]{targetNum}[/] ÷ [fuchsia]{nums[1]}[/]", $"[fuchsia]{nums[0]}[/]");
                return true;
            }

            if (targetNum - nums[1] == nums[0])
            {
                stepsTable.AddRow($"[white]{targetNum}[/] - [fuchsia]{nums[1]}[/]", $"[fuchsia]{nums[0]}[/]");
                return true;
            }

            return false;
        }

        if (nums.Length > 2)
        {
            long lastNum = nums[^1];

            // Temporäre Tabelle
            var tempTable = new Table()
                .AddColumn("[blue]Operation[/]")
                .AddColumn("[green]Result[/]")
                .Centered();

           
            if (targetNum % lastNum == 0)
            {
                tempTable.AddRow($"[white]{targetNum}[/] ÷ [fuchsia]{lastNum}[/]", $"[green]{targetNum / lastNum}[/]");
                if (CanBeCalculated(targetNum / lastNum, nums[..^1], tempTable))
                {
                    
                    foreach (var row in tempTable.Rows)
                    {
                        stepsTable.AddRow(row);
                    }
                    return true;
                }
            }

            // Temporäre Tabelle 
            tempTable = new Table()
                .AddColumn("[blue]Operation[/]")
                .AddColumn("[green]Result[/]")
                .Centered();

           
            if (targetNum - lastNum >= 0)
            {
                tempTable.AddRow($"[white]{targetNum}[/] - [fuchsia]{lastNum}[/]", $"[green]{targetNum - lastNum}[/]");
                if (CanBeCalculated(targetNum - lastNum, nums[..^1], tempTable))
                {
                    
                    foreach (var row in tempTable.Rows)
                    {
                        stepsTable.AddRow(row);
                    }
                    return true;
                }
            }
        }

        return false; // Keine Lösung gefunden
    }

    private Panel Animation_CookingRecipe()
    {
        var steps = new List<string>
    {
        "[bold yellow]Step 1:[/] Parse each line to extract the target number and operands. Example: `[blue]277851[/]: 49 70 27 7 3`.",
        "[bold yellow]Step 2:[/] To save a lot of calculations, I calculated the solution backwards using the inverse operators - and ÷ (e.g., `3672960 ÷ 240 = 15304`).",
        "[bold yellow]Step 3:[/] With this strategy, you can immediately discard many divisions that do not result in a whole number.",
        "[bold yellow]Step 4:[/] Continue dividing or subtracting operands until the target number is validated or disproven."
    };

        var recipeTable = new Table()
            .Title("[bold green]--- Day 7: Bridge Repair ---[/]")
            .Border(TableBorder.Rounded)
            .AddColumn("[yellow]Cooking Recipe[/]")
            .Centered();

        foreach (var step in steps)
        {
            recipeTable.AddRow(step);
        }

        var exampleTable = new Table()
            .Title("[bold blue]Example Calculation[/]")
            .Border(TableBorder.Rounded)
            .AddColumn("[bold yellow]Operation[/]")
            .AddColumn("[bold green]Result[/]")
            .Centered();

        exampleTable.AddRow("[blue]277851[/] ÷ [fuchsia]3[/]", "15304");
        exampleTable.AddRow("92617 - [fuchsia]7[/]", "728");
        exampleTable.AddRow("92610 ÷ [fuchsia]27[/]", "3430");
        exampleTable.AddRow("3430 ÷ [fuchsia]70[/]", "[fuchsia]49[/]");

        var combinedPanel = new Panel(new Rows(recipeTable, exampleTable, new Markup($"[yellow]Solution found for:[/] [blue]277851[/]: [fuchsia]49 70 27 7 3[/]")))
            .Expand()
            .Border(BoxBorder.Rounded)
            .Header("[bold yellow]How the Calculation Works[/]");

        return combinedPanel;
    }
}
