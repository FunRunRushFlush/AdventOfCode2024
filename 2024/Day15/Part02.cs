namespace Day15;
public static class Part02
{

    private static string Instructions { get; set; }
    private static char[,] Warehouse { get; set; }
    private static int WarehouseHeight;
    private static int WarehouseWidth;

    public static (int Y, int X) roboPos;

    public static long Result(string input)
    {
        ParseInput(input);
        int insCounter = 0;
        long GPS = 0;
        foreach (char ins in Instructions)
        {
            insCounter++;
            var pos = SetOffsetCoord((Direction)ins, roboPos);
            GlobalLog.LogLine($"ins: {(Direction)ins}");
            CheckForStone(pos.Y, pos.X, ins);

            DrawGrid(insCounter);

        }

        for (int y = 0; y < WarehouseHeight; y++)
        {
            for (int x = 0; x < WarehouseWidth; x++)
            {
                if (Warehouse[y, x] == 'O')
                {
                    GPS += y * 100 + x;
                }
            }
        }
        GlobalLog.LogLine($"GPS = {GPS}");
        return GPS;
    }
    [System.Diagnostics.Conditional("LOGGING_ENABLED")]
    private static void DrawGrid(int s = 0)
    {

        GlobalLog.LogLine($"Move = {s}");

        for (int h = 0; h < WarehouseHeight; h++)
        {
            for (int w = 0; w < WarehouseWidth; w++)
            {
                string drawPoint = ".";
                if (Warehouse[h, w] > 0)
                {
                    drawPoint = Warehouse[h, w].ToString();
                }


                Console.Write($"{drawPoint}");
            }
            Console.WriteLine();
        }
        Console.WriteLine("-------------------------------------------");
        Console.WriteLine();
    }

    private static bool CheckForStone(int y, int x, int dir)
    {
        if (Warehouse[y, x] == '#')
        {
            GlobalLog.LogLine($"Wall: y:{y} x:{x}");
            return false;
        }
        else if (Warehouse[y, x] == '[' || Warehouse[y, x] == ']')
        {
            if(Warehouse[y, x] == '[' 
                &&((Direction)dir == Direction.Up || (Direction)dir == Direction.Down))
            {
                var offsetPos01 = SetOffsetCoord((Direction)dir, (y, x));
                var result01 = CheckForStone(offsetPos01.Y, offsetPos01.X, dir);
                var offsetPos02 = SetOffsetCoord(Direction.Left, (y, x));
                var result02 = CheckForStone(offsetPos02.Y, offsetPos02.X, dir);

                return result01 && result02;

            }
            else if (Warehouse[y, x] == ']'
                && ((Direction)dir == Direction.Up || (Direction)dir == Direction.Down))
            {
                //Wenn einer der beiden failt failen beide:
                var offsetPos01 = SetOffsetCoord((Direction)dir, (y, x));
                var result01 = CheckForStone(offsetPos01.Y, offsetPos01.X, dir);
                var offsetPos02 = SetOffsetCoord(Direction.Right, (y, x));
                var result02 = CheckForStone(offsetPos02.Y, offsetPos02.X, dir);

                return result01 && result02;
            }
            else
            {
                    var offsetPos = SetOffsetCoord((Direction)dir, (y, x));
                    CheckForStone(offsetPos.Y, offsetPos.X, dir);                
            }
        }
        else if (Warehouse[y, x] == '.')
        {
            var distance = Math.Abs(y - roboPos.Y) + Math.Abs(x - roboPos.X);
            Warehouse[roboPos.Y, roboPos.X] = '.';
            var offsetPos = SetOffsetCoord((Direction)dir, (roboPos.Y, roboPos.X));
            roboPos = offsetPos;
            Warehouse[roboPos.Y, roboPos.X] = '@';
            if (distance > 1)
            {
                var tempPos = roboPos;
                for (int d = 1; d < distance; d+=1)
                {
                    offsetPos = SetOffsetCoord((Direction)dir, (tempPos.Y, tempPos.X));
                    tempPos = offsetPos;
                    if((Direction) dir==Direction.Left && d%2==1)
                    {
                        Warehouse[tempPos.Y, tempPos.X] = ']';
                        offsetPos = SetOffsetCoord((Direction)dir, (tempPos.Y, tempPos.X));
                        //tempPos = offsetPos;
                        Warehouse[offsetPos.Y, offsetPos.X] = '[';

                    }
                    if ((Direction)dir == Direction.Up)
                    {
                        var temp02 = tempPos;
                        Warehouse[tempPos.Y, tempPos.X] = ']';
                        offsetPos = SetOffsetCoord((Direction)dir, (tempPos.Y, tempPos.X));
                        //tempPos = offsetPos;
                        Warehouse[tempPos.Y, tempPos.X] = ']';
                        temp02 = tempPos;
                        offsetPos = SetOffsetCoord(Direction.Left, (temp02.Y, temp02.X));
                        Warehouse[tempPos.Y, tempPos.X] = '[';
                    }

                    if ((Direction)dir == Direction.Up)
                    {
                        var temp02=tempPos;
                        Warehouse[tempPos.Y, tempPos.X] = '[';
                        offsetPos = SetOffsetCoord((Direction)dir, (tempPos.Y, tempPos.X));
                        //tempPos = offsetPos;
                        Warehouse[tempPos.Y, tempPos.X] = '[';
                        temp02 = tempPos;
                        offsetPos = SetOffsetCoord(Direction.Right, (temp02.Y, temp02.X));
                        Warehouse[tempPos.Y, tempPos.X] = ']';
                    }
                }
            }
        }
        return true;
    }

    private enum Direction
    {
        Up = '^',
        Right = '>',
        Down = 'v',
        Left = '<'
    }
    private static bool CheckBounderys((int Y, int X) pos)
    {
        if (pos.X < 1) return false;
        if (pos.Y < 1) return false;
        if (pos.X >= WarehouseWidth - 1) return false;
        if (pos.Y >= WarehouseHeight - 1) return false;

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

    private static void ParseInput(string input)
    {
        //TODO: DAFUQ: Environment.NewLine + Environment.NewLine = eien leere zeile
        var inputSplit = input
            .Split(new[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);


        Instructions = inputSplit[1].Replace(Environment.NewLine, "").Trim();

        //TODO: muss ich noch üben wie genau ich welchen Datentype bekomme
        // string[]; string[][]; char[]; char[][]; string[,] etc
        var rows = inputSplit[0].Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        int height = rows.Length;
        int width = rows[0].Length;

        Warehouse = new char[height, width*2];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (rows[y][x] == '@')
                {
                    Warehouse[y, x * 2] = rows[y][x];
                    roboPos = (y, x*2);
                    Warehouse[y, x * 2 + 1] = '.';
                }
                else if (rows[y][x] == 'O')
                {
                    Warehouse[y, x * 2] = '[';
                    Warehouse[y, x * 2 + 1] = ']';
                }
                else
                {
                    Warehouse[y, x*2] = rows[y][x];
                    Warehouse[y, x * 2+1] = rows[y][x];
                }

                if (Warehouse[y, x] == '@')
                {
                    roboPos = (y, x);
                }
            }
        }
        WarehouseHeight = Warehouse.GetLength(0);
        WarehouseWidth = Warehouse.GetLength(1);

    }
}
