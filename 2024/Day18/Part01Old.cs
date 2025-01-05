using Microsoft.CodeAnalysis;

namespace Day18;
public class Part01Old : IPart
{
    //private int[,] MemoryField = new int[7,7];
    private int[,] MemoryField = new int[71,71];

    //private int MaxAllowedBytes = 12;
    private int MaxAllowedBytes = 1024;
    private int FieldHeight = 0;
    private int FieldWidth = 0;
    public string Result(Input input)
    {
        InputParser(input.Lines);
        FieldHeight= MemoryField.GetLength(0);
        FieldWidth = MemoryField.GetLength(1);
        DrawGrid(MemoryField);

        MemoryField[0,0] = 1;
        (int Y, int X) Pos = (0,0);
        TryToEscape(Pos,1);

        DrawGrid(MemoryField);

        var solution = MemoryField[FieldHeight - 1, FieldWidth - 1] - 1; //-1, da steplogik start=1 
        return solution.ToString();
    }

    private void TryToEscape((int Y, int X) pos, int step)
    {
        var nextStep = step +1;
        for (int i = 0; i < 4; i++)
        {
            var offSet = SetOffsetCoord((Direction)i, pos);
            if (CheckForValidStepOption(offSet, nextStep))
            {
       
                MemoryField[offSet.Y, offSet.X] = nextStep;
                TryToEscape(offSet,nextStep);
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

    private bool CheckForValidStepOption((int Y, int X) pos, int stepCount)
    {
        if (pos.X < 0) return false;
        if (pos.Y < 0) return false;
        if (pos.X >= MemoryField.GetLength(1)) return false;
        if (pos.Y >= MemoryField.GetLength(0)) return false;
        if(stepCount>= MemoryField[pos.Y,pos.X] && MemoryField[pos.Y, pos.X] != 0) return false;
        if(MemoryField[FieldHeight-1, FieldWidth-1]>0 && MemoryField[FieldHeight - 1, FieldWidth - 1] < stepCount) return false;

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

    private void InputParser(ReadOnlySpan<string> input)
    {
        var limit = MaxAllowedBytes > input.Length ? input.Length : MaxAllowedBytes;
        for (int i = 0; i < limit; i++)
        {
            ReadOnlySpan<char> line = input[i].AsSpan();
            int commaIndex = line.IndexOf(',');
            var xSpan = line.Slice(0, commaIndex);
            var ySpan = line.Slice(commaIndex + 1);

            int x = int.Parse(xSpan);
            int y = int.Parse(ySpan);

            MemoryField[y, x] = -1;
        }
    }

    [System.Diagnostics.Conditional("LOGGING_ENABLED")]
    private void DrawGrid(int[,] array)
    {
        var arrayHeight = array.GetLength(0);
        var arrayWidth = array.GetLength(1);

        GlobalLog.LogLine($"DrawGrid");

        for (int h = 0; h < arrayHeight; h++)
        {
            for (int w = 0; w < arrayWidth; w++)
            {
                string drawPoint = ".";
                //if (array[h, w] > 0)
                //{
                    drawPoint = array[h, w].ToString();
                //}

                //TODO: $"[{array[h, w],3}]" syntax für besseren Print
                Console.Write($"[{array[h, w],3}]");
            }
            Console.WriteLine();
        }
        Console.WriteLine("-------------------------------------------");
        Console.WriteLine();
    }

    public long ResultOld(ReadOnlySpan<string> input)
    {
        InputParserOld(input);
        DrawGrid(MemoryField);
        return 0;
    }

    private void InputParserOld(ReadOnlySpan<string> input)
    {
        var limit = MaxAllowedBytes > input.Length ? input.Length : MaxAllowedBytes;
        for (int i = 0; i < limit; i++)
        {
            //(int X,int Y)Pos = (input[i][0],input[i][2]);
            var pos = input[i].Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse).ToArray();
            MemoryField[pos[1], pos[0]] = -1;
        }
    }

}