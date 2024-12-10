namespace Day10;


public static class Part02
{
    private static int trailHeadCounter = 0;
    private static int MapHeight = 0;
    private static int MapWidth = 0;


    //static HashSet<(int Y, int X, int StartY, int StartX)> uniqueHead = new HashSet<(int Y, int X, int StartY, int StartX)>();


    public static long Result(ReadOnlySpan<string> input)
    {
        MapHeight = input.Length;
        MapWidth = input[0].Length;



        for (int y = 0; y < MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                GlobalLog.Log($"input[y][x] : Y:{y} X:{x}");
                if (CharToInt(input[y][x]) == 0)
                {
                    PathChecker(CharToInt(input[y][x]), (y, x), input);
                }
            }
        }

        GlobalLog.Log($"trailHeadCounter : {trailHeadCounter}");
        return trailHeadCounter;
    }

    private static void PathChecker(int trailNum, (int Y, int X) trailPos, ReadOnlySpan<string> input)
    {
        GlobalLog.Log($"PathChecker: trailNum:{trailNum}, trailPos: Y:{trailPos.Y} X:{trailPos.X}");
        if (trailNum == 9)
        {
            trailHeadCounter++;
            GlobalLog.Log($"trailHeadCounter : {trailHeadCounter}");
            return;
        }
        foreach (var trail in (ExtractNearbyTrails(trailNum, trailPos, input)))
        {
            if (trail.trailNum == 0 || trail.trailNum == trailNum) continue;
            PathChecker(trail.trailNum, trail.trailPos, input);
        }

    }

    private static TrailPath[] ExtractNearbyTrails(int trailNum, (int Y, int X) trailPos, ReadOnlySpan<string> input)
    {
        TrailPath[] trailPath = new TrailPath[4];
        for (int i = 0; i < 4; i++)
        {
            var temp = SetOffsetCoord((Direction)i, trailPos);
            if ((temp.Y >= MapHeight || temp.Y < 0) ||
                (temp.X >= MapWidth || temp.X < 0)) continue;

            if (CharToInt(input[temp.Y][temp.X]) == trailNum + 1)
            {
                trailPath[i].trailNum = trailNum + 1;
                trailPath[i].trailPos = temp;
            }
        }
        return trailPath;
    }

    private enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }

    private struct TrailPath
    {
        public int trailNum { get; set; }
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

        int Y = trailPos.Y + dir.Item1;
        int X = trailPos.X + dir.Item2;

        return (Y, X);
    }
    private static int CharToInt(char c)
    {
        return (int)(c - '0');
    }
    public static long Result_Improved01(ReadOnlySpan<string> input)
    {
        MapHeight = input.Length;
        MapWidth = input[0].Length;

        for (int y = 0; y < MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                GlobalLog.Log($"input[y][x] : Y:{y} X:{x}");
                if (CharToInt(input[y][x]) == 0)
                {
                    PathChecker_Imp(CharToInt(input[y][x]), (y, x), input);
                }
            }
        }

        GlobalLog.Log($"trailHeadCounter : {trailHeadCounter}");
        return trailHeadCounter;
    }

    private static void PathChecker_Imp(int startTrailNum, (int Y, int X) startTrailPos, ReadOnlySpan<string> input)
    {
        var stack = new Stack<TrailPath>();
        stack.Push(new TrailPath(startTrailNum, startTrailPos));

        // Temporärer Speicher für benachbarte Trails
        Span<TrailPath> trailPaths = stackalloc TrailPath[4];

        while (stack.Count > 0)
        {
            var current = stack.Pop();

            if (current.trailNum == 9)
            {
                trailHeadCounter++;
                continue;
            }

            // Finde alle benachbarten Trails
            int trailCounter = ExtractNearbyTrails_Imp(current.trailNum, current.trailPos, input, trailPaths);

            for (int t = 0; t < trailCounter; t++)
            {
                var trail = trailPaths[t];
                if (trail.trailNum != 0 && trail.trailNum != current.trailNum)
                {
                    stack.Push(trail);
                }
            }
        }
    }
    private static int ExtractNearbyTrails_Imp(int trailNum, (int Y, int X) trailPos, ReadOnlySpan<string> input, Span<TrailPath> trailPaths)
    {
        int count = 0;
        for (int i = 0; i < 4; i++)
        {
            var temp = SetOffsetCoord((Direction)i, trailPos);
            if ((temp.Y >= MapHeight || temp.Y < 0) ||
                (temp.X >= MapWidth || temp.X < 0)) continue;

            if (CharToInt(input[temp.Y][temp.X]) == trailNum + 1)
            {
                trailPaths[count++] = new TrailPath(trailNum + 1, temp);
            }
        }
        return count;
    }

}