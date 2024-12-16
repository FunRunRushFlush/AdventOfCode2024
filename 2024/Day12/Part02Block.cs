using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftWindowsWPF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;

namespace Day12;

public static class Part02Block
{
    private static Dictionary<(int Y, int X), char> region;
    private static Dictionary<(int Y, int X), char> allRegion;
    private static string[] field;

    public static long Result(ReadOnlySpan<string> input)
    {
        region = new Dictionary<(int Y, int X), char>();
        allRegion = new Dictionary<(int Y, int X), char>();
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
                    var corner = 0;
                    for(int i = 0; i<4;i++)
                    {
                        corner += CheckForCorner(i, cel.Key);
                        if (corner > 0)
                        {
                            region[cel.Key] = char.Parse(corner.ToString());
                            GlobalLog.LogLine($"FoundCorner: region = {cel.Key.ToString()} Postion {i};");
                        }
                    }
                    fence += corner;
                    
                }

                     
                var tempPrice = fence * region.Count;
                GlobalLog.LogLine($"###################################################");
                GlobalLog.LogLine($"### Price: {tempPrice} = {region.Count} * {fence}; Char={input[y][x]} Y:{y} X:{x} ###");
                GlobalLog.LogLine($"###################################################");

                //for (int h = -1; h <= input.Length; h++)
                //{
                //    for (int w = -1; w <= input[0].Length; w++)
                //    {
                //        string drawPoint = ".";
                //        if (region.TryGetValue((h, w), out char c))
                //        {
                //            drawPoint = c.ToString();
                //        }


                //        Console.Write($"{drawPoint}");
                //    }
                //    Console.WriteLine();
                //}
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
            }

            
        }
        return Price;
    }


    private static int CheckForCorner(int slot, (int Y, int X) pos)
    {
        GlobalLog.LogLine($"CheckForCorner: slot:{slot}, posY:{pos.Y} posX:{pos.X} ");
        if (slot == 0)
        {
            if (region.ContainsKey((pos.Y, pos.X + 1))
                && !region.ContainsKey((pos.Y + 1, pos.X + 1))
                && region.ContainsKey((pos.Y + 1, pos.X)))
            {
                return 1;
            }

            if (!region.ContainsKey((pos.Y, pos.X + 1))
                && !region.ContainsKey((pos.Y + 1, pos.X + 1))
                && !region.ContainsKey((pos.Y + 1, pos.X)))
            {
                return 1;
            }

            //edgecase: Part02_05Example_368
            if (!region.ContainsKey((pos.Y, pos.X + 1))
                && region.ContainsKey((pos.Y + 1, pos.X + 1))
                && !region.ContainsKey((pos.Y + 1, pos.X)))
            {
                return 1;
            }

        }
        if (slot == 1)
        {
            if (region.ContainsKey((pos.Y, pos.X - 1))
                  && !region.ContainsKey((pos.Y + 1, pos.X - 1))
                  && region.ContainsKey((pos.Y + 1, pos.X)))
            {
                return 1;
            }

            if (!region.ContainsKey((pos.Y, pos.X - 1))
                   && !region.ContainsKey((pos.Y + 1, pos.X - 1))
                   && !region.ContainsKey((pos.Y + 1, pos.X)))
            {
                return 1;
            }
            //edgecase: Part02_05Example_368
            if (!region.ContainsKey((pos.Y, pos.X - 1))
             && region.ContainsKey((pos.Y + 1, pos.X - 1))
             && !region.ContainsKey((pos.Y + 1, pos.X)))
            {
                return 1;
            }
        }
        if (slot == 2)
        {
            if (!region.ContainsKey((pos.Y - 1, pos.X - 1))
                  && region.ContainsKey((pos.Y - 1, pos.X))
                  && region.ContainsKey((pos.Y, pos.X - 1)))
            {
                return 1;
            }

            if (!region.ContainsKey((pos.Y - 1, pos.X - 1))
                  && !region.ContainsKey((pos.Y - 1, pos.X))
                  && !region.ContainsKey((pos.Y, pos.X - 1)))
            {
                return 1;
            }
            //edgecase: Part02_05Example_368
            if (region.ContainsKey((pos.Y - 1, pos.X - 1))
              && !region.ContainsKey((pos.Y - 1, pos.X))
              && !region.ContainsKey((pos.Y, pos.X - 1)))
            {
                return 1;
            }
        }
        if (slot == 3)
        {
            if (region.ContainsKey((pos.Y - 1, pos.X))
                  && !region.ContainsKey((pos.Y - 1, pos.X + 1))
                  && region.ContainsKey((pos.Y, pos.X + 1)))
            {
                return 1;
            }

            if (!region.ContainsKey((pos.Y - 1, pos.X))
                  && !region.ContainsKey((pos.Y - 1, pos.X + 1))
                  && !region.ContainsKey((pos.Y, pos.X + 1)))
            {
                return 1;
            }
            //edgecase: Part02_05Example_368
            if (!region.ContainsKey((pos.Y - 1, pos.X))
              && region.ContainsKey((pos.Y - 1, pos.X + 1))
              && !region.ContainsKey((pos.Y, pos.X + 1)))
            {
                return 1;
            }

        }
        return 0;
    }

    private static void CheckForRegion((int y, int x) postion, char v)
    {
        //GlobalLog.Log($"CheckForRegion: Y:{postion.y} X:{postion.x} and Char:{v}");
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