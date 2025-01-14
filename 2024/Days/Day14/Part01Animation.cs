using Spectre.Console;
using System.Numerics;
using System.Text;

namespace Year_2024.Days.Day14;
public class Part01Animation : IPart
{
    private List<Robot> roboInfoList = new();
    private int bathWidth = 101;
    private int bathHeight = 103;

    private int SecondLimit = 100;
    private int[,] bathroom;
    long Q1 = 0;
    long Q2 = 0;
    long Q3 = 0;
    long Q4 = 0;

    public string Result(Input input)
    {
        bathroom = new int[bathHeight, bathWidth];
        ParseInput(input.SpanLines);

        int currentSecond = 0;
        int nextSecond = 0;

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
                    Array.Clear(bathroom);
                    Q1 = 0;
                    Q2 = 0;
                    Q3 = 0;
                    Q4 = 0;


                    foreach (var robo in roboInfoList)
                    {
                        CalculatePosition(robo, nextSecond);
                        QuarterCalc(robo);
                        bathroom[(int)robo.Position.Y, (int)robo.Position.X]++;
                    }

                    var gridPanel = new Panel(Animation_DrawBath_Text())
             .Header($"[yellow]Bath Grid at Second {currentSecond}[/]", Justify.Center)
             .Border(BoxBorder.Rounded);

                  
                    var qValuesPanel = new Panel($"""
                        [green]Q1:[/] {Q1:D3} | [blue]Q2:[/] {Q2:D3}
                        --------+--------
                        [red]Q4:[/] {Q4:D3} | [yellow]Q3:[/] {Q3:D3}

                        [white]Score:[/] {Q1 * Q2 * Q3 * Q4}
                    """)
                        .Border(BoxBorder.Rounded);
             

                    
                    var layout = new Grid()
                        .AddColumn() 
                        .AddColumn() 
                        .AddRow(gridPanel, qValuesPanel);

                   
                    ctx.UpdateTarget(layout);


                    var key = Console.ReadKey(true).Key;

                        if (key == ConsoleKey.RightArrow)
                        {
                            currentSecond++;
                            nextSecond = 1;
                        }
                        else if (key == ConsoleKey.LeftArrow)
                        {

                            currentSecond--;
                            nextSecond = -1;
                        }
                        else if (key == ConsoleKey.UpArrow)
                        {

                            currentSecond -= 100;
                            nextSecond = -100;
                        }
                        else if (key == ConsoleKey.DownArrow)
                        {
                            currentSecond += 100;
                            nextSecond = +100;
                        }
                        else if (key == ConsoleKey.Escape)
                        {
                            break;
                        }
                        else
                        {
                            currentSecond += 0;
                            nextSecond = 0;
                        }
                        Thread.Sleep(5);
                    
                }
            });

        return (Q1 * Q2 * Q3 * Q4).ToString();
    }

    private string Animation_DrawBath_Text()
    {
        var sb = new StringBuilder();
        int cellWidth = 2;

        for (int h = 0; h < bathHeight; h++)
        {
            for (int w = 0; w < bathWidth; w++)
            {
                string drawPoint = ".";

                if (bathroom[h, w] > 0)
                {
                    drawPoint = "🤖";
                }
                else if (bathWidth / 2 == w)
                {
                    drawPoint = "[yellow]|[/]";
                }
                else if (bathHeight / 2 == h)
                {
                    drawPoint = "[yellow]--[/]";
                }


                sb.Append(drawPoint.PadRight(cellWidth));
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    private Panel Animation_CookingRecipe()
    {
        var steps = new List<string>
    {
        "1. Initialize the bath grid",
        "2. Parse input to get robot data",
        "3. For each robot:",
        "    - Calculate position based on velocity and time",
        "    - Handle teleportation within bath bounds",
        "    - Update robot's position in the bath grid",
        "4. Calculate the number of robots in each quarter",
        "5. Compute the final result as Q1 * Q2 * Q3 * Q4"
    };

        var recipeTable = new Table()
            .Title("Cooking Recipe")
            .Border(TableBorder.Rounded)
            .AddColumn("[green]--- Day 14: Restroom Redoubt --- Part01[/]")
            .Centered();

        for (int i = 0; i < steps.Count; i++)
        {
            recipeTable.AddRow(steps[i]);
        }

        var panel = new Panel(recipeTable)
            .Expand();


        return panel;
    }

    private void QuarterCalc(Robot robo)
    {
        if (robo.Position.X < bathWidth / 2)
        {
            if (robo.Position.Y < bathHeight / 2)
            {
                Q1++;
            }
            else if (robo.Position.Y > bathHeight / 2)
            {
                Q4++;
            }
        }
        else if (robo.Position.X > bathWidth / 2)
        {
            if (robo.Position.Y < bathHeight / 2)
            {
                Q2++;
            }
            else if (robo.Position.Y > bathHeight / 2)
            {
                Q3++;
            }
        }

    }

    private void CalculatePosition(Robot robo, int seconds)
    {
        var vec = robo.Position + robo.Velocity * seconds;
        robo.Position = CalcTeleportation(vec);
        robo.Seconds += seconds;
    }

    private Vector2 CalcTeleportation(Vector2 vec)
    {
        float AdjustedX = (vec.X % bathWidth + bathWidth) % bathWidth;
        float AdjustedY = (vec.Y % bathHeight + bathHeight) % bathHeight;

        return new Vector2(AdjustedX, AdjustedY);
    }

    private void ParseInput(ReadOnlySpan<string> spanLines)
    {
        foreach (string line in spanLines)
        {
            int[] data = line.Split(new[] { ' ', ',', 'p', 'v', '=' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(int.Parse).ToArray();
            roboInfoList.Add(new Robot(new Vector2(data[0], data[1]), new Vector2(data[2], data[3])));
        }
    }

    private class Robot
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public int Seconds;

        public Robot(Vector2 position, Vector2 velocity, int seconds = 0)
        {
            Position = position;
            Velocity = velocity;
            Seconds = seconds;
        }
    }

}
