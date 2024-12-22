/*
using System.Collections.Generic;

namespace Day16;
public static class Part01
{
    private static string[] maze;
    private static char[][] charMaze;

    private static (int Y, int X) EndPosition;
    private static (int Y, int X) StartPosition;
      private static  List<Reindeer> reindeerList = new List<Reindeer>();
    private static List<Reindeer> newReindeers = new List<Reindeer>();

    public static long Result(ReadOnlySpan<string> input)
    {
        maze = input.ToArray();
        StartPosition = (maze.Length - 2, 1);
        EndPosition = (1, maze[1].Length - 2);
        GlobalLog.LogLine($"StartPoint: {maze[maze.Length - 2][1]}");
        charMaze = ParseReindeerMaze(input);
        var test = new HashSet<(int Y, int X)>() { (StartPosition.Y, StartPosition.X) };
        reindeerList.Add(new Reindeer(StartPosition.Y, StartPosition.X, Direction.Right, 0, test));

        int stuckStateCounter = 0;
        int maxIterations = 6;
        int iteration = 0;
        while (iteration++ < maxIterations)
        {
            GlobalLog.LogLine($" --------------------- iteration: {iteration} ------------------------");
            newReindeers.Clear();

            foreach (var element in reindeerList)
            {
                if (element.CheckIfInStuckState())
                {
                    stuckStateCounter++;
                    continue;
                }
                else
                {
                    element.StartRunning();
                }

            }
                GlobalLog.LogLine($" --------------------- stuckStateCounter: {stuckStateCounter} ------------------------");

            reindeerList.AddRange(newReindeers);


            if (stuckStateCounter == reindeerList.Count) break;

            stuckStateCounter = 0;
        }
        var minScore = int.MaxValue;
        foreach (var element in reindeerList)
        {
            if (element.CheckIfInFoundTheEndState())
            {
                minScore = Math.Min(minScore, element.GetScore());
                //DrawGrid(element);
            }
        }
        foreach (var element in reindeerList)
        {
            if (minScore ==  element.GetScore())
            {
                DrawGrid(element);
                break;
            }
        }




        GlobalLog.LogLine($"Finished- minScore: {minScore}");
        return minScore;
    }

    [System.Diagnostics.Conditional("LOGGING_ENABLED")]
    private static void DrawGrid(Reindeer minReinPath)
    {
        var path = minReinPath.GetPersonalWalkedPath();
        GlobalLog.LogLine($"ReindeerMove = {path.Count}");
        GlobalLog.LogLine($"Reindeer Score: {minReinPath.GetScore()}");



        for (int h = 0; h < charMaze.Length; h++)
        {
            for (int w = 0; w < charMaze[0].Length ; w++)
            {

                    var drawPoint = charMaze[h][ w].ToString();
                


                Console.Write($"{drawPoint}");
            }
            Console.WriteLine();
        }
        Console.WriteLine("-------------------------------------------");
        Console.WriteLine();
    }


    private static char[][] ParseReindeerMaze(ReadOnlySpan<string> input)
    {
        char[][] result = new char[input.Length][];
        for (int y = 0; y < input.Length; y++)
        {
            result[y] = new char[input[0].Length];
            for (int x = 0; x < input[0].Length; x++)
            {
                result[y][x] = input[y][x];
            }
        }

        return result;
    }

    private record Reindeer
    {
        private int PointScore;
        private Direction FacingDirection;
        private (int Y, int X) Position;
        HashSet<(int Y, int X)> WalkedPath;
        HashSet<(int Y, int X, Direction)> personalWalkedPath;
        bool ImStuck=false;
        bool FoundTheEnd = false;
        public Reindeer(int y, int x, Direction dir, int pScore, HashSet<(int Y, int X)> walkedPath = null, HashSet < (int Y, int X,Direction) > persPath = null)
        {
            Position.Y = y;
            Position.X = x;
            FacingDirection = dir;
            PointScore = pScore;
            if (walkedPath != null)
            {
                if (persPath == null)
                {
                    personalWalkedPath = new HashSet<(int Y, int X, Direction)>();
                }
                else
                {
                    personalWalkedPath = new HashSet<(int Y, int X,Direction)>(persPath);
                    foreach (var p in personalWalkedPath)
                    {
                        GlobalLog.LogLine($"personalWalkedPath : Y:{p.Y},X:{p.X}  ");

                    }
                }
                WalkedPath = walkedPath;
                GlobalLog.LogLine($"Copyed HashSet- Count: {WalkedPath.Count}");
            }
            //else
            //{
            //    WalkedPath = new HashSet<(int Y, int X)>() { (y, x) };
            //    personalWalkedPath = new HashSet<(int Y, int X)>();
            //}
        }
        
         public Reindeer Copy(int posY,int posX,int facingDir,int pointScore, HashSet<(int Y, int X)> walkedPath, HashSet<(int Y, int X)> persPath)
        {
            walkedPath.Add((posY, posX));
            var newPath = new HashSet<(int Y, int X)>(persPath);

            newPath!.Add((posY, posX));
            foreach (var p in newPath)
            {
                GlobalLog.LogLine($"newPath : Y:{p.Y},X:{p.X}  ");

            }

            GlobalLog.LogLine($"newPath : {newPath.Count} ");
            var rein =new Reindeer(posY, posX, (Direction)facingDir, pointScore, walkedPath, newPath);
            DrawGrid(rein);
            return rein;
        }
        public bool CheckIfInStuckState() => ImStuck;
        public bool CheckIfInFoundTheEndState() => FoundTheEnd;
        public int GetScore() => PointScore;
        public Direction GetFacingDir() => FacingDirection;
        public HashSet<(int Y, int X, Direction)> GetPersonalWalkedPath() =>personalWalkedPath;
        public void StartRunning()
        {
            //GlobalLog.LogLine("StartRunning:");
            //    GlobalLog.LogLine($"Position: Y:{Position.Y} X:{Position.X}");
            //GlobalLog.LogLine($"PointScore: {PointScore}, FaxingDir:{FacingDirection}");
            if (!ImStuck)
            {
                CheckIfSplitPossible();
                TryToStep();
            }
        }

        private void TryToStep()
        {
            var offSett = SetOffsetCoord((Direction)FacingDirection, Position);
            if (maze[offSett.Y][offSett.X] == '#' 
                || personalWalkedPath.Contains(offSett.Y,offSett,)
                )
            {
                ImStuck = true;
            }
            else if(maze[offSett.Y][offSett.X] == 'E')
            {
                Position = offSett;
                PointScore++;
                ImStuck = true;
                FoundTheEnd = true;
            }
            else
            {
                Position = offSett;
                personalWalkedPath.Add(Position);
                PointScore++;      
            }

        }

        private void CheckIfSplitPossible()
        {
           var possSplits = GetListOfPossibleSplits(Position);
            foreach (var splits in possSplits)
            {
                var scoreIncrease = CalculatePointIncrease(splits);
                newReindeers.Add(Copy(splits.Y, splits.X, splits.dir, PointScore + scoreIncrease, WalkedPath,personalWalkedPath));
            }
        }

        private int CalculatePointIncrease((int Y, int X, int dir) splits)
        {
            var multi = 1;
            if( (splits.dir+2)%4 == splits.dir) multi = 2;


            return (multi * 1000) +1;       //+1 wegen extraschritt     
        }

        private List<(int Y, int X, int dir)> GetListOfPossibleSplits((int Y, int X) position)
        {
            List<(int Y, int X , int dir )> possPathList = new List<(int Y, int X, int dir)>();
            for (int i = 0; i < 4; i++)
            {
                if(i==(int)FacingDirection) continue;
                if ((i + 2) % 4 == (int)FacingDirection) continue;

                var possPath = SetOffsetCoord((Direction)i, position);
                if (maze[possPath.Y][possPath.X] == '.' && !personalWalkedPath.Contains(possPath))
                {
                    GlobalLog.LogLine($"(maze[possPath.Y][possPath.X] == '.' && !personalWalkedPath ={personalWalkedPath.Contains(possPath)}");
                    possPathList.Add((possPath.Y,possPath.X,i));
                }
            }
            return possPathList;
        }
    }


    private enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }


    //private static bool CheckBounderys((int Y, int X) pos)
    //{
    //    if (pos.X < 1) return false;
    //    if (pos.Y < 1) return false;
    //    if (pos.X >= WarehouseWidth - 1) return false;
    //    if (pos.Y >= WarehouseHeight - 1) return false;

    //    return true;
    //}

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
*/