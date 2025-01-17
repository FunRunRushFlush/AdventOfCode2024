namespace Year_2024.Days.Day12;

public class Part02 : IPart
{
    private Dictionary<(int Y, int X), char> region;
    private Dictionary<(int Y, int X), char> allRegion;
    private string[] field;

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
                    var corner = 0;
                    for (int i = 0; i < 4; i++)
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
                GlobalLog.LogLine($"### Price: {tempPrice} = {region.Count} * {fence}; Char={input.Lines[y][x]} Y:{y} X:{x} ###");
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
            }


        }
        return Price.ToString();
    }


    private int CheckForCorner(int slot, (int Y, int X) pos)
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

    private void CheckForRegion((int y, int x) postion, char v)
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