namespace Year_2024.Days.Day15;
public class Part01 : IPart
{

    private string Instructions { get; set; }
    private char[,] Warehouse { get; set; }
    private int WarehouseHeight;
    private int WarehouseWidth;

    public (int Y, int X) roboPos;

    public string Result(Input input)
    {
        ParseInput(input.Text);
        int insCounter = 0;
        long GPS = 0;
        foreach (char ins in Instructions)
        {
            insCounter++;
            var pos = SetOffsetCoord((Direction)ins, roboPos);
            GlobalLog.LogLine($"ins: {(Direction)ins}");
            CheckForStone(pos.Y, pos.X, ins);
            //DrawGrid(insCounter);
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

    private void CheckForStone(int y, int x, int dir)
    {
        if (Warehouse[y, x] == '#')
        {
            GlobalLog.LogLine($"Wall: y:{y} x:{x}");
            return;
        }
        else if (Warehouse[y, x] == 'O')
        {
            var offsetPos = SetOffsetCoord((Direction)dir, (y, x));
            CheckForStone(offsetPos.Y, offsetPos.X, dir);
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
                for (int d = 1; d < distance; d++)
                {
                    offsetPos = SetOffsetCoord((Direction)dir, (tempPos.Y, tempPos.X));
                    tempPos = offsetPos;
                    Warehouse[tempPos.Y, tempPos.X] = 'O';
                }
            }
        }
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
        //TODO: DAFUQ: Environment.NewLine + Environment.NewLine = eine leere zeile
        var inputSplit = input
            .Split(new[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);


        Instructions = inputSplit[1].Replace(Environment.NewLine, "").Trim();

        //TODO: muss ich noch üben wie genau ich welchen Datentype bekomme
        // string[]; string[][]; char[]; char[][]; string[,] etc
        var rows = inputSplit[0].Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        int height = rows.Length;
        int width = rows[0].Length;

        Warehouse = new char[height, width];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Warehouse[y, x] = rows[y][x];

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
