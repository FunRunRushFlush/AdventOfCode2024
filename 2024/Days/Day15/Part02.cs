namespace Year_2024.Days.Day15;
public class Part02 : IPart
{

    private string Instructions { get; set; }
    private char[,] Warehouse { get; set; }
    private int WarehouseHeight;
    private int WarehouseWidth;

    private const char StoneRightSide = ']';
    private const char StoneLeftSide = '[';
    private const char Player = '@';
    private const char Wall = '#';
    private const char smallStone = 'O';
    private const char freeSpace = '.';

    private Dictionary<(int Y, int X), char> tempRender = new Dictionary<(int Y, int X), char>();

    public (int Y, int X) roboPos;

    public string Result(Input input)
    {
        ParseInput(input.Text);
        int insCounter = 0;
        long GPS = 0;
        DrawGrid(insCounter);
        foreach (char ins in Instructions)
        {
            insCounter++;
            var pos = SetOffsetCoord((Direction)ins, roboPos);
            GlobalLog.LogLine($"ins: {(Direction)ins}");
            CheckForStone(pos.Y, pos.X, ins);
            tempRender.Clear();

            //DrawGrid(insCounter);
            //Console.ReadLine();
        }

        for (int y = 0; y < WarehouseHeight; y++)
        {
            for (int x = 0; x < WarehouseWidth; x++)
            {
                if (Warehouse[y, x] == StoneLeftSide)
                {
                    GPS += y * 100 + x;
                }
            }
        }
        GlobalLog.LogLine($"GPS = {GPS}");
        return GPS.ToString();
    }
    [System.Diagnostics.Conditional("LOGGING_ENABLED")]
    private void DrawGrid(int s = 0)
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
    private void TryAddWithLog((int Y, int X) pos, char value)
    {
        GlobalLog.LogLine($"tempRender.Add((y:{pos.Y},x:{pos.X}), {value}");
        tempRender.TryAdd(pos, value);
    }

    private int CheckForStone(int y, int x, int dir)
    {
        GlobalLog.LogLine($"CheckForStone: y:{y} x:{x}");
        var counter = 0;
        var checkingTile = Warehouse[y, x];

        var partDir = checkingTile == StoneLeftSide ? Direction.Right : Direction.Left;
        var partOffSett = SetOffsetCoord(partDir, (y, x));
        var partChar = Warehouse[partOffSett.Y, partOffSett.X];
        TryAddWithLog((y, x), checkingTile);
        TryAddWithLog(partOffSett, partChar);


        if (checkingTile == Wall)
        {
            GlobalLog.LogLine($"Wall: y:{y} x:{x}");
            return 0;
        }
        else if (checkingTile == StoneLeftSide || checkingTile == StoneRightSide)
        {
            if (checkingTile == StoneLeftSide
                && ((Direction)dir == Direction.Up || (Direction)dir == Direction.Down))
            {
                //tempRender.Add((y, x), checkingTile);
                //tempRender.Add(partOffSett, partChar);
                var offsetPos = SetOffsetCoord((Direction)dir, (y, x));

                counter += CheckForWideStone(offsetPos.Y, offsetPos.X, dir);
                offsetPos = SetOffsetCoord(Direction.Right, (offsetPos.Y, offsetPos.X));
                counter += CheckForWideStone(offsetPos.Y, offsetPos.X, dir);

            }
            else if (checkingTile == StoneRightSide
                && ((Direction)dir == Direction.Up || (Direction)dir == Direction.Down))
            {
                //tempRender.Add((y, x), checkingTile);
                //tempRender.Add(partOffSett, checkingTile);
                var offsetPos = SetOffsetCoord((Direction)dir, (y, x));

                counter += CheckForWideStone(offsetPos.Y, offsetPos.X, dir);
                offsetPos = SetOffsetCoord(Direction.Left, (offsetPos.Y, offsetPos.X));
                counter += CheckForWideStone(offsetPos.Y, offsetPos.X, dir);
            }
            else
            {
                var offsetPos = SetOffsetCoord((Direction)dir, (y, x));
                CheckForStone(offsetPos.Y, offsetPos.X, dir);
            }
        }
        else if (checkingTile == freeSpace)
        {
            var distance = Math.Abs(y - roboPos.Y) + Math.Abs(x - roboPos.X);
            Warehouse[roboPos.Y, roboPos.X] = freeSpace;
            var offsetPos = SetOffsetCoord((Direction)dir, (roboPos.Y, roboPos.X));
            roboPos = offsetPos;
            Warehouse[roboPos.Y, roboPos.X] = Player;
            if (distance > 1)
            {
                var tempPos = roboPos;
                for (int d = 1; d < distance; d += 1)
                {
                    offsetPos = SetOffsetCoord((Direction)dir, (tempPos.Y, tempPos.X));
                    tempPos = offsetPos;
                    if ((Direction)dir == Direction.Left && d % 2 == 1)
                    {
                        Warehouse[tempPos.Y, tempPos.X] = StoneRightSide;
                        offsetPos = SetOffsetCoord((Direction)dir, (tempPos.Y, tempPos.X));
                        //tempPos = offsetPos;
                        Warehouse[offsetPos.Y, offsetPos.X] = StoneLeftSide;

                    }
                    else if ((Direction)dir == Direction.Right && d % 2 == 1)
                    {

                        Warehouse[tempPos.Y, tempPos.X] = StoneLeftSide;
                        offsetPos = SetOffsetCoord((Direction)dir, (tempPos.Y, tempPos.X));
                        //tempPos = offsetPos;
                        Warehouse[offsetPos.Y, offsetPos.X] = StoneRightSide;


                    }

                }
            }
        }

        if (counter > 1)
        {
            var distance = Math.Abs(y - roboPos.Y) + Math.Abs(x - roboPos.X);
            Warehouse[roboPos.Y, roboPos.X] = freeSpace;
            var offsetPos = SetOffsetCoord((Direction)dir, (roboPos.Y, roboPos.X));
            RenderNewStoneLocation(dir);
            roboPos = offsetPos;
            Warehouse[roboPos.Y, roboPos.X] = Player;

        }
        return counter;
    }

    private void RenderNewStoneLocation(int dir)
    {
        foreach (var element in tempRender)
        {
            Warehouse[element.Key.Y, element.Key.X] = freeSpace;
        }
        foreach (var element in tempRender)
        {
            var offsetPos = SetOffsetCoord((Direction)dir, element.Key);
            Warehouse[offsetPos.Y, offsetPos.X] = element.Value;
        }
    }

    private int CheckForWideStone(int wY, int wX, int dir)
    {
        GlobalLog.LogLine($"CheckForWideStone: y:{wY} x:{wX}");
        var counter = 0;
        var checkingTile = Warehouse[wY, wX];

        var partDir = checkingTile == StoneLeftSide ? Direction.Right : Direction.Left;
        var partOffSett = SetOffsetCoord(partDir, (wY, wX));
        var partChar = Warehouse[partOffSett.Y, partOffSett.X];


        if (checkingTile == Wall)
        {
            GlobalLog.LogLine($"Wall: y:{wY} x:{wX}");
            return -1000;
        }
        else if (checkingTile == StoneLeftSide || checkingTile == StoneRightSide)
        {
            TryAddWithLog((wY, wX), checkingTile);
            TryAddWithLog(partOffSett, partChar);
            if (checkingTile == StoneLeftSide
                && ((Direction)dir == Direction.Up || (Direction)dir == Direction.Down))
            {


                var offsetPos = SetOffsetCoord((Direction)dir, (wY, wX));

                counter += CheckForWideStone(offsetPos.Y, offsetPos.X, dir);
                var temp = SetOffsetCoord(Direction.Right, (offsetPos.Y, offsetPos.X));
                counter += CheckForWideStone(temp.Y, temp.X, dir);

            }
            else if (checkingTile == StoneRightSide
                && ((Direction)dir == Direction.Up || (Direction)dir == Direction.Down))
            {

                var offsetPos = SetOffsetCoord((Direction)dir, (wY, wX));

                counter += CheckForWideStone(offsetPos.Y, offsetPos.X, dir);
                offsetPos = SetOffsetCoord(Direction.Left, (offsetPos.Y, offsetPos.X));
                counter += CheckForWideStone(offsetPos.Y, offsetPos.X, dir);
            }

        }


        else if (checkingTile == freeSpace)
        {
            return 1;
        }
        return counter;
    }

    private enum Direction
    {
        Up = '^',
        Right = '>',
        Down = 'v',
        Left = '<'
    }
    private bool CheckBounderys((int Y, int X) pos)
    {
        if (pos.X < 1) return false;
        if (pos.Y < 1) return false;
        if (pos.X >= WarehouseWidth - 1) return false;
        if (pos.Y >= WarehouseHeight - 1) return false;

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

    private void ParseInput(string input)
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

        Warehouse = new char[height, width * 2];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (rows[y][x] == Player)
                {
                    Warehouse[y, x * 2] = rows[y][x];
                    roboPos = (y, x * 2);
                    Warehouse[y, x * 2 + 1] = freeSpace;
                }
                else if (rows[y][x] == smallStone)
                {
                    Warehouse[y, x * 2] = StoneLeftSide;
                    Warehouse[y, x * 2 + 1] = StoneRightSide;
                }
                else
                {
                    Warehouse[y, x * 2] = rows[y][x];
                    Warehouse[y, x * 2 + 1] = rows[y][x];
                }

                if (Warehouse[y, x] == Player)
                {
                    roboPos = (y, x);
                }
            }
        }
        WarehouseHeight = Warehouse.GetLength(0);
        WarehouseWidth = Warehouse.GetLength(1);

    }
}
