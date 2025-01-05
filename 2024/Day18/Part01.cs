using BenchmarkDotNet.Attributes;
using System.Numerics;

namespace Day18;
public class Part01 : IPart
{
    private int[,] Map;
    private int[,] StepMap;



    private int MapWidth = 71;
    private int MapHeight = 71;

    private Vector2 StartPos;
    private Vector2 EndPos;

    private int MinSteps;

    private int ParseLimit = 1024;


    private Vector2 Up = new Vector2(0, -1);
    private Vector2 Right = new Vector2(1, 0);
    private Vector2 Down = new Vector2(0, 1);
    private Vector2 Left = new Vector2(-1, 0);



    public string Result(Input input)
    {
        ParseInput(input.Lines);
        StepMap = new int[MapWidth, MapHeight];
        StartPos = Vector2.Zero;
        EndPos = new Vector2(MapWidth - 1, MapHeight - 1);
        SearchBestPath(StartPos, EndPos);

        return MinSteps.ToString();
    }

    //TODO: BFS-Search ! -- muss ich noch paar mal üben
    private void SearchBestPath(Vector2 startPos, Vector2 endPos)
    {
        Queue<(Vector2, int)> searchQue = new();
        searchQue.Enqueue((startPos, 0));

        StepMap[(int)startPos.Y, (int)startPos.X] = 0;

        while (searchQue.Count > 0)
        {
            var (pos, step) = searchQue.Dequeue();

            if (pos==endPos)
            {
                MinSteps = step;
                continue;
            }
            for (int i = 0; i < 4; i++)
            {
                var stepCoord = pos + DirVec((Dir)i);
                if (IsValidStep(stepCoord, step + 1))
                {
                    StepMap[(int)stepCoord.Y, (int)stepCoord.X] = step+1;
                    searchQue.Enqueue((stepCoord, step + 1));
                }
            }
        }

    }

    private bool IsValidStep(Vector2 pos, int step)
    {
        int y = (int)pos.Y;
        int x = (int)pos.X;

        if (y < 0 || y >= MapHeight) return false;
        if (x < 0 || x >= MapWidth) return false;

        if (Map[y, x] == -1) return false;
        if (StepMap[y, x] > 0) return false;

        return true;
    }

    private void SearchBestPathOld(Vector2 pos, int steps)
    {
        int y = (int)pos.Y;
        int x = (int)pos.X;

        if (pos == EndPos)
        {
            MinSteps = steps;
            StepMap[y, x] = steps;
            GlobalLog.LogLine($"MinSteps; {MinSteps}");
            //DrawGrid(Map);
            return;
        }

        StepMap[y, x] = steps;

        for (int i = 0; i < 4; i++)
        {
            var stepCoord = pos + DirVec((Dir)i);
            if (IsValidPath(stepCoord, steps + 1))
            {
                SearchBestPathOld(stepCoord, steps + 1);
            }
        }
    }
    private bool IsValidPath(Vector2 pos, int steps)
    {
        int y = (int)pos.Y;
        int x = (int)pos.X;

        if (y < 0 || y >= MapHeight) return false;
        if (x < 0 || x >= MapWidth) return false;
        if (Map[y, x] == -1 || (StepMap[y, x] <= steps && StepMap[y, x] > 0)) return false;

        if (MinSteps > 0)
        {
            if (MinSteps < steps) return false;

            var minPathX = MapWidth - x - 1;
            var minPathY = MapHeight - y - 1;

            if ((steps + minPathY + minPathX) > MinSteps) return false;
        }

        return true;
    }



    private void ParseInput(string[] lines)
    {
        Map = new int[MapHeight, MapWidth];
        for (int i = 0; i < ParseLimit; i++)
        {
            var coords = lines[i].Split(',').Select(int.Parse).ToArray();
            Map[coords[1], coords[0]] = -1;
        }
    }

    public enum Dir
    {
        up = 0,
        right = 1,
        down = 2,
        left = 3,
    }
    private Vector2 DirVec(Dir index)
    {
        return index switch
        {
            Dir.up => Up,
            Dir.right => Right,
            Dir.down => Down,
            Dir.left => Left,
            _ => throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and 3.")
        };
    }


    [System.Diagnostics.Conditional("LOGGING_ENABLED")]
    private void DrawGrid(int[,] array)
    {
        var arrayHeight = array.GetLength(0);
        var arrayWidth = array.GetLength(1);

        GlobalLog.LogLine($"DrawGrid");

        for (int h = 0; h < arrayHeight; h++)
        {
            for (int w = 0; w < arrayWidth; w++)
            {
                string drawPoint = "#";
                if (array[h, w] > 0)
                {
                    drawPoint = array[h, w].ToString();
                }

                //TODO: $"[{array[h, w],3}]" syntax für besseren Print
                Console.Write($"[{array[h, w],3}]");
            }
            Console.WriteLine();
        }
        Console.WriteLine("-------------------------------------------");
        Console.WriteLine();
    }
}