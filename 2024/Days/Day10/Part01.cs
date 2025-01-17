namespace Year_2024.Days.Day10;
public class Part01 : IPart
{
    private int trailHeadCounter = 0;
    private int MapHeight = 0;
    private int MapWidth = 0;
    private Direction dirEnum;
    private string[] Input;
    HashSet<(int Y, int X, int StartY, int StartX)> uniqueHead = new HashSet<(int Y, int X, int StartY, int StartX)>();
    private int StartX = 0;
    private int StartY = 0;

    public string Result(Input input)
    {
        MapHeight = input.Lines.Length;
        MapWidth = input.Lines[0].Length;
        Input = input.Lines.ToArray();


        for (int y = 0; y < MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                GlobalLog.LogLine($"input[y][x] : Y:{y} X:{x}");
                if (CharToInt(input.Lines[y][x]) == 0)
                {
                    StartY = y;
                    StartX = x;
                    PathChecker(CharToInt(input.Lines[y][x]), (y, x));
                }
            }
        }

        GlobalLog.LogLine($"trailHeadCounter : {trailHeadCounter}");

        return trailHeadCounter.ToString();
    }

    private void PathChecker(int trailNum, (int Y, int X) trailPos)
    {
        GlobalLog.LogLine($"PathChecker: trailNum:{trailNum}, trailPos: Y:{trailPos.Y} X:{trailPos.X}");
        if (trailNum == 9 && uniqueHead.Add((trailPos.Y, trailPos.X, StartY, StartX)))
        {
            trailHeadCounter++;
            GlobalLog.LogLine($"trailHeadCounter : {trailHeadCounter}");
            return;
        }
        foreach (var trail in ExtractNearbyTrails(trailNum, trailPos))
        {
            if (trail.trailNum == 0 || trail.trailNum == trailNum) continue;
            PathChecker(trail.trailNum, trail.trailPos);
        }

    }

    private TrailPath[] ExtractNearbyTrails(int trailNum, (int Y, int X) trailPos)
    {
        TrailPath[] trailPath = new TrailPath[4];
        for (int i = 0; i < 4; i++)
        {
            var temp = SetOffsetCoord((Direction)i, trailPos);
            if (temp.Y >= MapHeight || temp.Y < 0 ||
                temp.X >= MapWidth || temp.X < 0) continue;

            if (CharToInt(Input[temp.Y][temp.X]) == trailNum + 1)
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

    private (int Y, int X) SetOffsetCoord(Direction direction, (int Y, int X) trailPos)
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
    private int CharToInt(char c)
    {
        return c - '0';
    }


}

