// Hab nicht verstanden, dass man auch über die Rennbahn gehen darf -.-

namespace Day20;
public class Part02Old
{
    private (int Y, int X) RaceStartPos;
    private (int Y, int X) RaceEndPos;
    private (int Y, int X) CarPos;
    private Direction CarDirection;

    private HashSet<((int Y, int X) Start, (int Y, int X) End)> SkipPos = new();
    private int[,] RaceTimeMap;
    private Dictionary<(int Y, int X), int> RaceTimeDic = new();
    private int Racetime;
    private int NumberOfCheats;

    private Dictionary<(int Y, int X), int> CheatPath = new();
    private Dictionary<((int Y, int X) CheatStartPos, (int Y, int X) CheatEndPos), int> UniqueCheats = new();

    private Dictionary<int, int> TimeSaveDic = new();
    private (int Y, int X) TryCheatStartPos;


    public long Result(ReadOnlySpan<string> input)
    {
        ParseInput(input);
        RaceTimeMap = new int[input.Length, input[0].Length];
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
                    RaceTimeMap[offSet.Y, offSet.X] = Racetime;
                    RaceTimeDic.Add(offSet, Racetime);
                    CarPos = offSet;
                    CarDirection = (Direction)d;
                    break;
                }
            }
        }

        foreach (var timeSlot in RaceTimeDic)
        {
            CheatPath.Clear();
            foreach (var ele in UniqueCheats)
            {
                if (!TimeSaveDic.TryAdd(ele.Value, 1))
                {
                    TimeSaveDic[ele.Value]++;
                }
 
            }
            UniqueCheats.Clear();
            GlobalLog.LogLine($"timeSlot: Y:{timeSlot.Key.Y} X:{timeSlot.Key.X}");
            for (int d = 0; d < 4; d++)
            {
                //if (CarDirection == (Direction)((d + 2) % 4)) continue;
                var offSet = SetOffsetCoord((Direction)d, timeSlot.Key);

                if (input[offSet.Y][offSet.X] == '#')
                {
                    TryCheatStartPos = timeSlot.Key;
                   if(!CheatPath.TryAdd(offSet, 1))
                    {
                        CheatPath[offSet] = 1;
                    }
                    CheatCrawler(offSet, 1, input);

                }

            }
            DrawGridDic(input);
        }


        GlobalLog.LogLine("TimeSaveDic-List (sorted ascending by Time)");
        foreach (var kvp in TimeSaveDic.OrderBy(x => x.Key))
        {
            GlobalLog.LogLine($"Time: {kvp.Key} Count: {kvp.Value}");
        }


        DrawGrid(RaceTimeMap);

        return UniqueCheats.Count;
    }

    private void CheatCrawler((int Y, int X) pos, int StepCounter, ReadOnlySpan<string> input)
    {
        if (StepCounter < 20)
        {
            for (int d = 0; d < 4; d++)
            {
                //if (CarDirection == (Direction)((d + 2) % 4)) continue;
                var offSet = SetOffsetCoord((Direction)d, pos);
                if (!CheckForOutOfBounds(offSet, input)) continue;
                if (input[offSet.Y][offSet.X] == '#')                {

                        if (!CheatPath.ContainsKey(offSet))
                        {
                            CheatPath.TryAdd(offSet, StepCounter + 1);
                            CheatCrawler(offSet, StepCounter + 1, input);
                        }
                        else if (CheatPath.ContainsKey(offSet) && CheatPath[offSet] > StepCounter)
                        {
                            CheatPath[offSet] = StepCounter + 1;
                            CheatCrawler(offSet, StepCounter + 1, input);
                        }
                    
                }
                else if (RaceTimeDic.ContainsKey(offSet))//+1 wegen dem extrastep??
                {
                    var timeSave = (RaceTimeDic[offSet] - (RaceTimeDic[TryCheatStartPos] + StepCounter + 1));
                    if (timeSave > 49)
                    {

                        if (!UniqueCheats.TryAdd((TryCheatStartPos, offSet), timeSave))
                        {
                            //GlobalLog.LogLine($"CheatCrawler: Y:{pos.Y} X:{pos.X}; StepCounter: {StepCounter}");
                            //GlobalLog.LogLine($"timeSave: {timeSave}");
                            UniqueCheats[(TryCheatStartPos, offSet)] = Math.Max(timeSave, UniqueCheats[(TryCheatStartPos, offSet)]);

                        }
                    }
                }

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

    private bool CheckForOutOfBounds((int Y, int X) pos, ReadOnlySpan<string> input)
    {
        if (pos.X < 0) return false;
        if (pos.Y < 0) return false;
        if (pos.X >= input[0].Length) return false;
        if (pos.Y >= input.Length) return false;


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

    private (int Y, int X) SetDoubleOffsetCoord(Direction direction, (int Y, int X) trailPos)
    {
        var dir = direction switch
        {
            Direction.Up => (-2, 0),
            Direction.Right => (0, 2),
            Direction.Down => (2, 0),
            Direction.Left => (0, -2),
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

    [System.Diagnostics.Conditional("LOGGING_ENABLED")]
    private void DrawGridDic(ReadOnlySpan<string> input)
    {
        var arrayHeight = input.Length;
        var arrayWidth = input[0].Length;

        GlobalLog.LogLine($"DrawGrid");

        for (int h = 0; h < arrayHeight; h++)
        {
            for (int w = 0; w < arrayWidth; w++)
            {
                string drawPoint = input[h][w].ToString();
                Console.ForegroundColor = ConsoleColor.White;
                if (CheatPath.ContainsKey((h,w)))
                {
                    drawPoint = CheatPath[(h,w)].ToString();
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                if (RaceTimeMap[h,w] > 0)
                {
                    drawPoint=RaceTimeMap[h,w].ToString();
                    Console.ForegroundColor = ConsoleColor.Green;
                }
               
                foreach(var ele in UniqueCheats)
                {
                    if (ele.Key.CheatStartPos.Y == h && ele.Key.CheatStartPos.X == w)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                    }
                    if (ele.Key.CheatEndPos.Y == h && ele.Key.CheatEndPos.X == w)
                    {
                        drawPoint = ele.Value.ToString();
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        //ele.Key.CheatEndPos.Y == h;
                    }
                }

                //TODO: $"[{array[h, w],3}]" syntax für besseren Print
                Console.Write($"[{drawPoint,3}]");
            }
            Console.WriteLine();
        }
        Console.WriteLine("-------------------------------------------");
        Console.WriteLine();
    }

}