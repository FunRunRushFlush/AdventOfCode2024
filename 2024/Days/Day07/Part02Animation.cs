using Spectre.Console;

namespace Year_2024.Days.Day07;

public class Part02Animation : IPart
{
    public string Result(Input rawInput)
    {
        long resCounter = 0;

        AnsiConsole.Live(new Panel("[bold yellow]Initializing Part 2...[/]").Expand())
            .Start(ctx =>
            {
                var introPanel = new Panel(Animation_Part2Intro())
                    .Expand()
                    .Border(BoxBorder.Rounded);

                ctx.UpdateTarget(introPanel);

                Console.ReadKey(true);

                foreach (var line in rawInput.Lines)
                {
                    long[] calc = line.Split(new[] { ' ', ':' }, StringSplitOptions.RemoveEmptyEntries)
                                      .Select(long.Parse).ToArray();

                    long targetNum = calc[0];
                    long[] nums = calc[1..];

                    var grid = new Grid().Expand().Centered();
                    grid.AddColumn();

                    var numsStrg = string.Join(" ", nums);
                    grid.AddRow(new Markup($"[yellow]Processing Target:[/] [bold blue]{targetNum}[/]: {numsStrg}"));

                    var stepsTable = CanBeCalculatedWithSteps(targetNum, nums);
                    if (stepsTable != null)
                    {
                        grid.AddRow(stepsTable);
                        grid.AddRow(new Markup($"[yellow]Solution found for:[/] [blue]{targetNum}[/]: [fuchsia]{numsStrg}[/]"));

                        resCounter += targetNum;
                    }
                    else
                    {
                        grid.AddRow(new Markup($"[red]No solution found for this input:[/] [bold blue]{targetNum}[/]"));
                    }

                    ctx.UpdateTarget(grid);

                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.Escape)
                    {
                        break;
                    }
                }
            });

        return resCounter.ToString();
    }

    private Table? CanBeCalculatedWithSteps(long targetNum, long[] nums)
    {
        var stepsTable = new Table()
            .AddColumn("[blue]Operation[/]")
            .AddColumn("[green]Result[/]")
            .Centered();

        if (TryCalculate(targetNum, nums, stepsTable))
        {
            return stepsTable;
        }

        return null;
    }

    private bool TryCalculate(long targetNum, long[] nums, Table stepsTable)
    {
        if (nums.Length == 2)
        {
            if (nums[1] != 0 && targetNum % nums[1] == 0 && targetNum / nums[1] == nums[0])
            {
                stepsTable.AddRow($"[blue]{targetNum}[/] ÷ [fuchsia]{nums[1]}[/]", $"[fuchsia]{nums[0]}[/]");
                return true;
            }

            if (targetNum - nums[1] == nums[0])
            {
                stepsTable.AddRow($"[blue]{targetNum}[/] - [fuchsia]{nums[1]}[/]", $"[fuchsia]{nums[0]}[/]");
                return true;
            }

            if (nums[0].ToString() + nums[1].ToString() == targetNum.ToString())
            {
                stepsTable.AddRow($"[blue]{targetNum}[/] [bold white](string split)[/]",
                                  $"[fuchsia]{nums[0]}[/] & [fuchsia]{nums[1]}[/]");
                return true;
            }

            return false;
        }

        if (nums.Length > 2)
        {
            long lastNum = nums[^1];

            if (lastNum != 0 && targetNum % lastNum == 0)
            {
                var tempTable = CreateEmptyStepsTable();
                tempTable.AddRow(
                    $"[white]{targetNum}[/] ÷ [fuchsia]{lastNum}[/]",
                    $"[green]{targetNum / lastNum}[/]");

                if (TryCalculate(targetNum / lastNum, nums[..^1], tempTable))
                {
                    AppendTableRows(stepsTable, tempTable);
                    return true;
                }
            }

            if (targetNum - lastNum >= 0)
            {
                var tempTable = CreateEmptyStepsTable();
                tempTable.AddRow(
                    $"[white]{targetNum}[/] - [fuchsia]{lastNum}[/]",
                    $"[green]{targetNum - lastNum}[/]");

                if (TryCalculate(targetNum - lastNum, nums[..^1], tempTable))
                {
                    AppendTableRows(stepsTable, tempTable);
                    return true;
                }
            }

            var strTargetNum = targetNum.ToString();
            var suffix = lastNum.ToString();
            if (strTargetNum.EndsWith(suffix) && suffix.Length != strTargetNum.Length)
            {
                var newTargetNum = long.Parse(strTargetNum[..(strTargetNum.Length - suffix.Length)]);

                var tempTable = CreateEmptyStepsTable();
                tempTable.AddRow(
                    $"[white]{targetNum}[/] [bold white](remove suffix {suffix})[/]",
                    $"[green]{newTargetNum}[/]");

                if (TryCalculate(newTargetNum, nums[..^1], tempTable))
                {
                    AppendTableRows(stepsTable, tempTable);
                    return true;
                }
            }
        }

        return false;
    }

    private Table CreateEmptyStepsTable()
    {
        return new Table()
            .AddColumn("[blue]Operation[/]")
            .AddColumn("[green]Result[/]")
            .Centered();
    }

    private void AppendTableRows(Table stepsTable, Table tempTable)
    {
        foreach (var row in tempTable.Rows)
        {
            stepsTable.AddRow(row);
        }
    }

    private Panel Animation_Part2Intro()
    {
        var recipeTable = new Table()
            .Title("[bold green]--- Day 7: Bridge Repair (Part2) ---[/]")
            .Border(TableBorder.Rounded)
            .AddColumn("[yellow]Strategy for Part2[/]")
            .Centered();

        recipeTable.AddRow("[bold yellow]Step 1:[/] We use the same backward logic as in Part01 but also consider string concatenation (e.g., 12 and 3 => 123).");
        recipeTable.AddRow("[bold yellow]Step 2:[/] In each step, we test ÷, -, and if applicable, removing a numeric suffix from the target.");
        recipeTable.AddRow("[bold yellow]Step 3:[/] Once a valid solution is found, we add the target number to our result counter. Multiple solutions may exist, but finding the first one suffices.");
        recipeTable.AddRow("[bold yellow]Step 4:[/] Press ESC during the live animation to abort early.");

        return new Panel(recipeTable)
            .Header("[bold yellow]Part2: Reverse Calculation + String Trick[/]")
            .Expand();
    }
}
