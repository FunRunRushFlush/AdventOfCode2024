using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftWindowsWPF;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;

namespace Day12;

public static class Part02
{
    private static Dictionary<(int Y, int X), char> region;
    private static Dictionary<(int Y, int X), char> allRegion;
    private static Dictionary<(int Y, int X), char> border;
    private static string[] field;
    private static int FenceCounter = 0;

    public static long Result(ReadOnlySpan<string> input)
    {
        region = new Dictionary<(int Y, int X), char>();
        allRegion = new Dictionary<(int Y, int X), char>();
        List<int> OuterCorner = new();
        border = new();
        //Dictionary<(int Y, int X), (bool Visited, HashSet<Direction> BorderDir)> border = new();
        HashSet<Direction> tempBorderList = new HashSet<Direction>();
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
                    //GlobalLog.Log($"region-cel: Y:{cel.Key.Y} X:{cel.Key.X}");
                    var counter = 0;
                    for (int dir = 0; dir < 4; dir++)
                    {
                        var offsetPos = SetOffsetCoord((Direction)dir, cel.Key);
                        if (!region.ContainsKey(offsetPos))
                        {
                            border.TryAdd(offsetPos,'#');
                            //{
                                OuterCorner.Add(dir);
                            //}
                            //else
                            //{
                            //    counter++;
                            //}


                        }
                    }
                    counter += CornerChecker(OuterCorner, cel.Key);
                    if (counter != 0)
                        GlobalLog.LogLine($"Fence -CornerChecker(DirCounter):{counter}");
                    fence += counter;
                    if (counter != 0)
                        region[cel.Key] = char.Parse(counter.ToString());
                    OuterCorner.Clear();
                    
                }
                foreach (var cel in border)
                {
                    GlobalLog.LogLine($"Border-cel: Y:{cel.Key.Y} X:{cel.Key.X}");
                    var counter = 0;
                    for (int dir = 0; dir < 4; dir++)
                    {
                        var offsetPos = SetOffsetCoord((Direction)dir, cel.Key);
                        if (region.ContainsKey(offsetPos))
                        {
                            OuterCorner.Add(dir);
                        }
                    }
                    counter += CornerChecker(OuterCorner);
                    if (counter != 0)
                        GlobalLog.LogLine($"Fence -CornerChecker(DirCounter):{counter}");
                    fence += counter;
                    if (counter != 0)
                        border[cel.Key] = char.Parse(counter.ToString());
                    OuterCorner.Clear();

                }

                //for (int h = -1; h <= input.Length; h++)
                //{
                //    for (int w = -1; w <= input[0].Length; w++)
                //    {
                //        string drawPoint = ".";
                //        if (region.TryGetValue((h, w), out char c))
                //        {
                //            drawPoint = c.ToString();
                //        }
                //        else if (border.TryGetValue((h, w), out char b))
                //        {
                //            drawPoint = b.ToString();
                //        }

                //        Console.Write($"{drawPoint}");
                //    }
                //    Console.WriteLine();
                //}





                //GlobalLog.Log($"FenceCounter: {fence}");
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
        return Price;
    }

    private static int CornerChecker(List<int> dirCounter, (int Y, int X)? cel = null)
    {

        if (dirCounter.Count <= 1) return 0;
        if(dirCounter.Count ==2)
        {
            if ((dirCounter[0] + dirCounter[1])%2==0) return 0;

            if(cel != null)
            {
                var positionToCheck = cel.Value;
                foreach (var item in dirCounter)
                {
                    positionToCheck = SetOffsetCoord((Direction)item, positionToCheck);
                }

                if (region.ContainsKey(positionToCheck)) return 0;
            }
            return 1;            
        }

        if (dirCounter.Count == 3) return 2;
        
        return 4;
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
    private enum ExtendedDirection
    {
        UpLeft = 0,
        Up = 1,
        UpRight = 2,
        Right = 3,
        DownRight = 4,
        Down = 5,
        DownLeft = 6,
        Left = 7,
            
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