using Spectre.Console;
using System.Linq;
using System.Numerics;

namespace Year_2024.Days.Day06;

public class Part01Animation : IPart
{
    private HashSet<Vector2> WallMap = new HashSet<Vector2>();
    private Dictionary<Vector2, Dir> UniqueSteps = new Dictionary<Vector2, Dir>();
    private Dir PlayerDir = Dir.Up;
    private Vector2 PlayerPosition;
    private Vector2 WallDetection = new Vector2(-1, 0);
    private int MapHeight;
    private int MapWidth;

    private Vector2 LoopPlayerStartPos;
    private Dir LoopPlayerStartDir;
    private Vector2 LoopStartWallDetection;
    private Dictionary<Vector2, Dir> LoopUniqueSteps = new Dictionary<Vector2, Dir>();

    private const char Wall = '#';
    private const char Player = '^';

    public string Result(Input rawInput)
    {
        ParseStartingInput(rawInput.Lines);
        UniqueSteps.Add(PlayerPosition, PlayerDir);

        LoopPlayerStartPos = PlayerPosition;
        LoopPlayerStartDir = PlayerDir;
        LoopStartWallDetection = WallDetection;

        MapHeight = rawInput.Lines.Length;
        MapWidth = rawInput.Lines[0].Length;
        int uniqueLoopSteps = 0;
        bool animationEsc = false;

        AnsiConsole.Clear();
        AnsiConsole.Markup(Animation_CookingRecipe().ToString());
        Console.ReadKey(true);

        while (true)
        {
            if (CheckOutOfBounds()) break;

            if (!CheckForWall())
            {
                PlayerPosition += WallDetection;
                UniqueSteps.TryAdd(PlayerPosition, PlayerDir);
            }
        }

        foreach (var ele in UniqueSteps)
        {
            LoopUniqueSteps.Clear();

            WallMap.Add(ele.Key);
            PlayerPosition = ele.Key;
            PlayerDir = ele.Value;
            WallDetection = WallDetectionVec();
            PlayerPosition -= WallDetection;
            LoopUniqueSteps.Add(PlayerPosition, PlayerDir);

            while (true)
            {
                if (CheckOutOfBounds()) break;

                if (!CheckForWall())
                {
                    PlayerPosition += WallDetection;
                    if (!LoopUniqueSteps.TryAdd(PlayerPosition, PlayerDir) && LoopUniqueSteps[PlayerPosition] == PlayerDir)
                    {
                        uniqueLoopSteps++;

                        if (!animationEsc)
                        {
                            AnsiConsole.Clear();
                            AnsiConsole.Markup(DrawGrid(rawInput.Lines));
                            AnsiConsole.Markup($"[green]Unique Loops:[/] {uniqueLoopSteps}");

                            var key = Console.ReadKey(true).Key;
                            if (key == ConsoleKey.Escape)
                            {
                                animationEsc = true;
                            }
                        }
                        break;
                    }
                }
            }
            WallMap.Remove(ele.Key);
        }

        return uniqueLoopSteps.ToString();
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
                    drawChar = "[blue]#[/]";
                }
                else if (position == PlayerPosition)
                {
                    drawChar = "[red]#[/]";
                }
                else if (LoopUniqueSteps.ContainsKey(position))
                {
                    var dirSym = DirectionSymbol(LoopUniqueSteps[position]);
                    drawChar = $"[green]{dirSym}[/]";
                }

                sb.Append(drawChar);
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }

    private string DirectionSymbol(Dir dir)
    {
        return dir switch
        {
            Dir.Up => "^",
            Dir.Down => "v",
            Dir.Left => "<",
            Dir.Right => ">",
            _ => throw new NotImplementedException()
        };
    }

    private Panel Animation_CookingRecipe()
    {
        var steps = new List<string>
        {
            "1. Parse the input to initialize the map, walls, and player position",
            "2. Track unique steps taken by the player",
            "3. Update player position and direction based on walls",
            "4. Detect and count unique loops in the player's path",
            "5. Display the results in the console"
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

    private bool CheckOutOfBounds()
    {
        var nextStepPos = PlayerPosition + WallDetection;

        return nextStepPos.Y >= MapWidth || nextStepPos.Y < 0 || nextStepPos.X >= MapHeight || nextStepPos.X < 0;
    }

    private bool CheckForWall()
    {
        if (WallMap.Contains(PlayerPosition + WallDetection))
        {
            PlayerDir = (Dir)(((int)PlayerDir + 1) % 4);
            WallDetection = WallDetectionVec();
            return true;
        }
        return false;
    }

    private Vector2 WallDetectionVec()
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

    private enum Dir
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3,
    }
}
