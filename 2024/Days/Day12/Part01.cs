
using BenchmarkDotNet.Attributes;
using System.Reflection;

namespace Year_2024.Days.Day12;
public class Part01 : IPart
{
    private Dictionary<(int Y, int X), char> region;
    private Dictionary<(int Y, int X), char> allRegion;
    private string[] field;
    private int FenceCounter = 0;
    private Direction dirEnum = new();

    public string Result(Input input)
    {
        region = new Dictionary<(int Y, int X), char>();
        allRegion = new Dictionary<(int Y, int X), char>();
        field = input.Lines.ToArray();
        int boundery = 0;
        int fence = 0;
        int dicCounterIndex = 0;
        int Price = 0;
        for (int y = 0; y < input.Lines.Length; y++)
        {
            for (int x = 0; x < input.Lines[0].Length; x++)
            {
                var postion = (y, x);
                if (allRegion.ContainsKey(postion)) continue;


                CheckForRegion(postion, input.Lines[y][x]);
                foreach (var cel in region)
                {
                    GlobalLog.LogLine($"cel: Y:{cel.Key.Y} X:{cel.Key.X}");
                    boundery = 4;
                    for (int dir = 0; dir < 4; dir++)
                    {
                        var offsetPos = SetOffsetCoord((Direction)dir, cel.Key);
                        if (region.ContainsKey(offsetPos)) boundery--;

                    }
                    GlobalLog.LogLine($"boundery: {boundery}");
                    fence += boundery;
                }
                GlobalLog.LogLine($"FenceCounter: {fence}");
                GlobalLog.LogLine($" Price: {Price} += {fence} * {region.Count};");

                Price += fence * region.Count;

                fence = 0;
                foreach (var item in region)
                {
                    if (!allRegion.ContainsKey(item.Key))
                    {
                        allRegion[item.Key] = item.Value;
                    }
                }
                region.Clear();
            }
        }
        return Price.ToString();
    }

    private void CheckForRegion((int y, int x) postion, char v)
    {
        GlobalLog.LogLine($"CheckForRegion: Y:{postion.y} X:{postion.x} and Char:{v}");
        if (field[postion.y][postion.x] == v)
        {
            if (region.TryAdd(postion, v))
            {
                for (int dir = 0; dir < 4; dir++)
                {
                    var offsetPos = SetOffsetCoord((Direction)dir, postion);
                    if (!CheckBounderys(offsetPos)) continue;
                    CheckForRegion(offsetPos, v);
                }
            }
        }
    }

    private enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }

    private bool CheckBounderys((int Y, int X) pos)
    {
        if (pos.X < 0) return false;
        if (pos.Y < 0) return false;
        if (pos.X >= field[0].Length) return false;
        if (pos.Y >= field.Length) return false;

        return true;
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
}

