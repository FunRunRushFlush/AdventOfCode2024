using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;

namespace Day12;

public static class Part02Try
{
    private static Dictionary<(int Y, int X), char> region;
    private static Dictionary<(int Y, int X), char> allRegion;
    private static Dictionary<(int Y, int X), (bool Visited, HashSet<Direction> BorderDir)> border;
    private static HashSet<Direction> tempBorderList;
    private static string[] field;
    private static int FenceCounter = 0;
    private static Direction dirEnum = new();

    public static long Result(ReadOnlySpan<string> input)
    {
        region = new Dictionary<(int Y, int X), char>();
        allRegion = new Dictionary<(int Y, int X), char>();
        border = new();
        tempBorderList = new HashSet<Direction>();
        field = input.ToArray();
        int boundery = 0;
        int fence = 0;
        int dicCounterIndex = 0;
        int Price = 0;
        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[0].Length; x++)
            {
                var postion = (y, x);
                if (allRegion.ContainsKey(postion)) continue;


                CheckForRegion(postion, input[y][x]);
                foreach (var cel in region)
                {
                    GlobalLog.LogLine($"region-cel: Y:{cel.Key.Y} X:{cel.Key.X}");

                    for (int dir = 0; dir < 4; dir++)
                    {
                        var offsetPos = SetOffsetCoord((Direction)dir, cel.Key);
                        if (!region.ContainsKey(offsetPos))
                        {
                            border.TryAdd(offsetPos, (false, new HashSet<Direction> { (Direction)((dir + 2) % 4) }));
                        }

                        if (border.ContainsKey(offsetPos))
                        {
                            border[offsetPos].BorderDir.Add((Direction)((dir + 2) % 4));
                        }

                    }

                }
                if (input[y][x] == 'J')
                {

                }
                foreach (var cel in border)
                {
                    if (cel.Value.Visited) continue;

                    var result = CheckFenceRegion(cel.Key);
                     fence += result;
                    GlobalLog.LogLine($"fence: {fence} += {result}");
                    tempBorderList.Clear();
                }


                GlobalLog.LogLine($"FenceCounter: {fence}");
                var tempPrice = fence * region.Count;
                GlobalLog.LogLine($"###################################################");
                GlobalLog.LogLine($"### Price: {tempPrice} = {region.Count} * {fence}; ###");
                GlobalLog.LogLine($"###################################################");

                Price += tempPrice;

                fence = 0;
                foreach (var item in region)
                {
                    if (!allRegion.ContainsKey(item.Key))
                    {
                        allRegion[item.Key] = item.Value;
                    }
                }
                region.Clear();
                border.Clear();
            }
        }
        GlobalLog.LogLine("");
        GlobalLog.LogLine($"###################################################");
        GlobalLog.LogLine($"###################################################");
        GlobalLog.LogLine($"############### Final Price: {Price} ##############");
        GlobalLog.LogLine($"###################################################");
        GlobalLog.LogLine($"###################################################");
        return Price;
    }

    private static int CheckFenceRegion((int Y, int X) key, (int Y, int X)? lastKey = null)
    {
        int result = 0;
        foreach (var dirHas in border[key].BorderDir)
        {            
            tempBorderList.Add(dirHas);
        }
        if (lastKey != null)
        {
            foreach (var dirHas in border[lastKey.Value].BorderDir)
            {
                tempBorderList.Remove(dirHas);
            }
        }
        
        result = tempBorderList.Count;

        border[key] = (true, border[key].BorderDir); 
        for (int dir = 0; dir < 4; dir++)
        {
            var offsetPos = SetOffsetCoord((Direction)dir, key);

            if (border.ContainsKey(offsetPos) && border[offsetPos].Visited != true)
            {
                
                result +=CheckFenceRegion(offsetPos, key);
            }
            else if (border.ContainsKey(offsetPos) && border[offsetPos].Visited && offsetPos!=lastKey)
            {
                foreach (var dirHas in border[offsetPos].BorderDir)
                {
                    if(tempBorderList.Remove(dirHas))
                    {
                        result -= 1;
                    }
                }

            }

        }
        return result;
    }
    private static void CheckForRegion((int y, int x) postion, char v)
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

    private static bool CheckBounderys((int Y, int X) pos)
    {
        if (pos.X < 0) return false;
        if (pos.Y < 0) return false;
        if (pos.X >= field[0].Length) return false;
        if (pos.Y >= field.Length) return false;

        return true;
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
}