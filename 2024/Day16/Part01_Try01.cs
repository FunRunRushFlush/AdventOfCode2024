using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using System.ComponentModel;

namespace Day16;
public static class Part01_Try01
{
    private static string[] maze;
    private static int[,] PointMap;

    private static (int Y, int X) EndPosition;
    private static (int Y, int X) StartPosition;
    private static int maxAllowdScore = int.MaxValue; 
    private static List<Reindeer> reindeerList = new List<Reindeer>();
    private static List<Reindeer> newReindeers = new List<Reindeer>();

    public static long Result(ReadOnlySpan<string> input)
    {
        maze = input.ToArray();
        StartPosition = (maze.Length - 2, 1);
        EndPosition = (1, maze.Length - 2);
        PointMap = new int[maze.Length, maze[0].Length];
        newReindeers.Add(new Reindeer(StartPosition.Y, StartPosition.X,Direction.Right,0));
        while (true)
        {  
            for (int i = 0; i < newReindeers.Count; i++)
            {
                var reindeer = newReindeers[i];
                if (reindeer.CheckForStuckState())
                {
                    continue;
                }
                else
                {
                    reindeer.TryToMove();
                }

            }

            for (int i = newReindeers.Count-1; i >-1; i--)
            {
                var reindeer = newReindeers[i];
                if (reindeer.CheckForStuckState())
                {
                    GlobalLog.LogLine($"Remove RTeindeer");
                    newReindeers.Remove(reindeer);
                }
            }

                if (newReindeers.Count == 0) break;
        }

        DrawGrid();

        return PointMap[EndPosition.Y, EndPosition.X];
    }

    [System.Diagnostics.Conditional("LOGGING_ENABLED")]
    private static void DrawGrid(int s = 0)
    {

        GlobalLog.LogLine($"Move = {s}");

        for (int h = 0; h < maze.Length; h++)
        {
            for (int w = 0; w < maze[0].Length; w++)
            {




                Console.Write($"[{PointMap[h,w]}],");
            }
            Console.WriteLine();
        }
        Console.WriteLine("-------------------------------------------");
        Console.WriteLine();
    }


    private class Reindeer
    {
        private int Score = 0;
        private (int Y, int X) Position;
        private Direction FacingDir;
        private bool ImStuck = false;


        public Reindeer(int Y, int X, Direction dir, int score)
        {
            Score = score;
            Position = (Y, X);
            FacingDir = dir;
            GlobalLog.LogLine($"New Reindeer: Y:{Y} X:{X} Score:{score}");
        }


        public void TryToMove()
        {
            CheckForSplitting();
            TryToStep();
        }
        public bool CheckForStuckState() => ImStuck;
        private void TryToStep()
        {
            var offSetPos = SetOffsetCoord(FacingDir, Position);
            var tempScore = Score + 1; //+ one Step
            if (CheckIfStepIsAllowed(offSetPos, tempScore))
            {
                Position = offSetPos;
                Score = tempScore;
            }
            else
            {
                ImStuck = true;
            }
        }

        private void CheckForSplitting()
        {
            for (int i = 0; i < 4; i++)
            {
                if ((Direction)i == FacingDir) continue;
                if ((Direction)((i + 2) % 4) == FacingDir) continue;

                var offSetPos = SetOffsetCoord((Direction)i, Position);
                var tempScore = Score + 1000 + 1; //Turn + one Step
                if (CheckIfStepIsAllowed(offSetPos, tempScore))
                {
                    newReindeers.Add(CreateReindeer(offSetPos, (Direction)i, tempScore));
                }
            }
        }


        private Reindeer CreateReindeer((int Y, int X) pos, Direction dir, int score)
        {
            return new Reindeer(pos.Y, pos.X, dir, score);
        }

        private bool CheckIfStepIsAllowed((int Y, int X) pos, int score)
        {
            if (maze[pos.Y][pos.X] == '#') return false;

            if(score> maxAllowdScore) return false;
            if (PointMap[pos.Y, pos.X] == 0 || PointMap[pos.Y, pos.X] >= score)
            {
                PointMap[pos.Y, pos.X] = score;
                if (maze[pos.Y][pos.X] == 'E')
                {
                    maxAllowdScore = Math.Min(maxAllowdScore, score);
                    return false;
                }

                return true;
            }

            return false;
        }

    }


    private enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
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