
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Day10;
public static class Part01
{
    private static int trailHeadCounter = 0;
    private static int MapHeight = 0;
    private static int MapWidth = 0;
    private static Direction dirEnum;
    private static string[] Input;
        static HashSet<(int Y, int X ,int StartY, int StartX)> uniqueHead = new HashSet<(int Y, int X, int StartY, int StartX)>();
    private static int StartX = 0;
    private static int StartY = 0;

    public static long Result(ReadOnlySpan<string> input)
    {
        MapHeight = input.Length;
        MapWidth = input[0].Length;
        Input = input.ToArray();


        for (int y = 0; y< MapHeight;y++)
        {
            for(int x = 0; x<MapWidth;x++)
            {
                    GlobalLog.LogLine($"input[y][x] : Y:{y} X:{x}");
                if(CharToInt(input[y][x])==0)
                {
                    StartY = y;
                    StartX = x;
                    PathChecker(CharToInt(input[y][x]), (y, x));
                }
            }
        }

        GlobalLog.LogLine($"trailHeadCounter : {trailHeadCounter}");

        return trailHeadCounter;
    }

    private static void PathChecker(int trailNum, (int Y, int X) trailPos)
    {
        GlobalLog.LogLine($"PathChecker: trailNum:{trailNum}, trailPos: Y:{trailPos.Y} X:{trailPos.X}");
        if (trailNum == 9 && uniqueHead.Add((trailPos.Y, trailPos.X, StartY, StartX)))
        {
            trailHeadCounter++;
            GlobalLog.LogLine($"trailHeadCounter : {trailHeadCounter}");
            return;
        }
        foreach (var trail in (ExtractNearbyTrails(trailNum, trailPos)))
        {
            if (trail.trailNum == 0 || trail.trailNum == trailNum) continue;
            PathChecker(trail.trailNum, trail.trailPos);
        }

    }

    private static TrailPath[] ExtractNearbyTrails(int trailNum, (int Y, int X) trailPos)
    {
        TrailPath[] trailPath = new TrailPath[4];
        for (int i = 0; i < 4; i++)
        {
            var temp = SetOffsetCoord((Direction)i, trailPos);
            if ((temp.Y >= MapHeight || temp.Y <0) ||
                (temp.X >= MapWidth || temp.X < 0)) continue;

            if (CharToInt(Input[temp.Y][temp.X]) == trailNum+1)
            {
                trailPath[i].trailNum = trailNum + 1;
                trailPath[i].trailPos = temp;
            }

        }
        return trailPath;
    }

    private enum Direction
    {
        Up=0,
        Right=1,
        Down=2,
        Left=3
    }

    private struct TrailPath
    {
        public int trailNum {  get; set; }
        public (int Y, int X) trailPos { get; set; }

        public TrailPath(int value, (int Y, int X) data)
        {
            trailNum = value;
            trailPos = data;
        }
    }

    private static (int Y, int X) SetOffsetCoord(Direction direction, (int Y, int X) trailPos)
    {
        var dir = direction switch
        {
            Direction.Up => (-1, 0),
            Direction.Right => (0, 1),
            Direction.Down => (1, 0),
            Direction.Left => (0, -1),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), $"Invalid direction: {direction}")
        };

        int Y = trailPos.Y+dir.Item1;
        int X = trailPos.X + dir.Item2;

        return (Y, X);        
    }
    private static int CharToInt(char c)
    {
        return (int)(c - '0');
    }


}

