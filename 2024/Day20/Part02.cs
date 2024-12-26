
using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Day20;
public class Part02
{
    private (int Y, int X) RaceStartPos;
    private (int Y, int X) RaceEndPos;
    private (int Y, int X) CarPos;
    private Direction CarDirection;


    private Dictionary<(int Y, int X), int> RaceTimeDic = new();
    private int Racetime;

    private int CheatCounter;
    public void ParseOnly(ReadOnlySpan<string> input)
    {
        ParseInput(input);
    }

    public long Result(ReadOnlySpan<string> input)
    {
        ParseInput(input);
  
        CarPos = RaceStartPos;
        RaceTimeDic.Add(RaceStartPos, 0);
        //StartDirection
        for (int d = 0; d < 4; d++)
        {
            var offSet = SetOffsetCoord((Direction)d, CarPos);
            if (CheckForValidStepOption(offSet, input))
            {
                CarDirection = (Direction)d;
            }
        }


        while (true)
        {
            if (CarPos == RaceEndPos) break;
            for (int d = 0; d < 4; d++)
            {
                if (CarDirection == (Direction)((d + 2) % 4)) continue;
                var offSet = SetOffsetCoord((Direction)d, CarPos);
                if (CheckForValidStepOption(offSet, input))
                {
                    Racetime++;
                    RaceTimeDic.Add(offSet, Racetime);
                    CarPos = offSet;
                    CarDirection = (Direction)d;
                    break;
                }
            }
        }

        foreach (var timeSlot in RaceTimeDic)
        {

            CheatStamp20x20(timeSlot);
        }


        return CheatCounter;
    }

    private void CheatStamp20x20(KeyValuePair<(int Y, int X), int> timeSlot)
    {
        foreach (var raceTime in RaceTimeDic)
        {
            var test = Math.Abs(timeSlot.Key.Y - raceTime.Key.Y) + Math.Abs(timeSlot.Key.X - raceTime.Key.X);
            if (test > 0 && test <= 20 && raceTime.Value - ((timeSlot.Value + test) ) >= 100)
            {
                CheatCounter++;
            }
        }
    }

   
   

    private void ParseInput(ReadOnlySpan<string> input)
    {
        bool sFound = false;
        bool eFound = false;
        for (int y = 0; y < input.Length; y++)
        {
            if (!sFound)
            {
                var checkS = input[y].IndexOf('S');
                if (checkS > -1)
                {
                    RaceStartPos = (y, checkS);
                    sFound = true;
                }
            }
            if (!eFound)
            {
                var checkE = input[y].IndexOf('E');
                if (checkE > -1)
                {
                    RaceEndPos = (y, checkE);
                    eFound = true;
                }
            }

            if (sFound && eFound) break;
        }
    }

    private enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }

    private bool CheckForValidStepOption((int Y, int X) pos, ReadOnlySpan<string> input)
    {
        if (pos.X < 0) return false;
        if (pos.Y < 0) return false;
        if (pos.X >= input[0].Length) return false;
        if (pos.Y >= input.Length) return false;
        if (input[pos.Y][pos.X] == '#') return false;

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



}