using Spectre.Console;
using System.Numerics;
using static System.Net.Mime.MediaTypeNames;

namespace Year_2024.Days.Day06;

public class Part01Animation : IPart
{
    //y <--> x vertauscht
    private HashSet<Vector2> WallMap = new HashSet<Vector2>();
    private HashSet<Vector2> UniqueSteps = new HashSet<Vector2>();
    private Dir PlayerDir = Dir.Up;
    private Vector2 PlayerPosition;
    private Vector2 WallDetection = new Vector2(-1, 0);
    private int MapHeight;
    private int MapWidth;



    private const char Wall = '#';
    private const char Player = '^';
    public string Result(Input rawInput)
    {
        ParseStartingInput(rawInput.Lines);
        MapHeight = rawInput.Lines.Length;
        MapWidth = rawInput.Lines[0].Length;
        UniqueSteps.Add(PlayerPosition);

        AnsiConsole.Live(new Panel("Initializing...").Expand().Border(BoxBorder.Rounded))
            .Start(ctx =>
            {
                var coocking = new Panel(Animation_CookingRecipe())
                    .Expand()
                    .Border(BoxBorder.Rounded);

                ctx.UpdateTarget(coocking);


                Console.ReadKey(true);

                while (true)
                {
                    if (CheckOutOfBounds()) break;
                    //GlobalLog.Log($"X: {PlayerPosition.Y} Y:{PlayerPosition.X} ");

                    if (!CheckForWall())
                    {
                        PlayerPosition += WallDetection;
                        UniqueSteps.Add(PlayerPosition);
                        //GlobalLog.Log($"X: {PlayerPosition.Y} Y:{PlayerPosition.X} ");
                    }

                    var gridPanel = new Panel(DrawGrid(rawInput.Lines))
           .Header("[yellow]Current Map State[/]", Justify.Center)
           .Border(BoxBorder.Rounded);

                    var statsPanel = new Panel($"[green]Unique Steps:[/] {UniqueSteps.Count}")
                        .Border(BoxBorder.Rounded);

                    var layout = new Grid()
                        .AddColumn()
                        .AddColumn()
                        .AddRow(gridPanel, statsPanel);

                    ctx.UpdateTarget(layout);
                }
            });
        //DrawGrid(rawInput.Lines);
        return UniqueSteps.Count.ToString();

    }

    private Panel Animation_CookingRecipe()
    {
        var steps = new List<string>
        {
            "1. Parse the input to initialize the map and player position",
            "2. Identify walls and track unique steps",
            "3. Update player position based on direction",
            "4. Detect and handle collisions with walls",
            "5. Continue until out of bounds or ESC is pressed",
            "6. Display the final count of unique steps"
        };

        var recipeTable = new Table()
            .Title("[bold green]Cooking Recipe[/]")
            .Border(TableBorder.Rounded)
            .AddColumn("[yellow]Steps[/]")
            .Centered();

        foreach (var step in steps)
        {
            recipeTable.AddRow(step);
        }

        return new Panel(recipeTable).Expand();
    }

    private string DrawGrid(string[] lines)
    {
        var sb = new System.Text.StringBuilder();

        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                string drawChar = ".";
                var position = new Vector2(y, x);

                if (WallMap.Contains(position))
                {
                    drawChar = $"[blue]#[/]";
                }
                else if (position == PlayerPosition)
                {
                    drawChar = $"[red]@[/]";
                }
                else if (UniqueSteps.Contains(position))
                {
                    drawChar = $"[green]X[/]"; ;
                }

                sb.Append(drawChar);
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }


    private bool CheckOutOfBounds()
    {
        var nextStepPos = PlayerPosition + WallDetection;

        if (nextStepPos.Y >= MapWidth || nextStepPos.Y < 0)
            return true;
        if (nextStepPos.X >= MapHeight || nextStepPos.X < 0)
            return true;

        return false;
    }

    private bool CheckForWall()
    {
        if (WallMap.Contains(PlayerPosition + WallDetection))
        {
            PlayerDir = (Dir)(((int)PlayerDir + 1) % 4);
            WallDetection = ChangeWallDetectionVec();
            return true;
        }
        return false;
    }

    private Vector2 ChangeWallDetectionVec()
    {
        return PlayerDir switch
        {
            Dir.Up => new Vector2(-1, 0),
            Dir.Down => new Vector2(1, 0),
            Dir.Right => new Vector2(0, 1),
            Dir.Left => new Vector2(0, -1),
            _ => throw new NotImplementedException()
        };
    }

    private void ParseStartingInput(string[] lines)
    {

        bool playerFound = false;
        for (int y = 0; y < lines.Length; y++)
        {
            var line = lines[y];
            if (!playerFound)
            {
                int xPos = line.IndexOf(Player);
                if (xPos > -1)
                {
                    PlayerPosition = new Vector2(y, xPos);
                    playerFound = true;
                }
            }

            int x = line.IndexOf(Wall);
            while (x != -1)
            {
                WallMap.Add(new Vector2(y, x));
                x = line.IndexOf(Wall, x + 1);
            }
        }
    }


    enum Dir
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3,
    }
}