


namespace Day16;
public static class Part01
{
    private static char[][] maze;
    private static ReadOnlyMemory<string> mazeMemory;

    public static long Result(ReadOnlySpan<string> input)
    {
        //mazeMemory = input.ToArray();
        maze = ParseReindeerMaze(input);
        return 1;
    }


    public static long Result_ToArray(ReadOnlySpan<string> input)
    {
        mazeMemory = input.ToArray();
        //ParseReindeerMaze(input);
        return 1;
    }
    private static char[][] ParseReindeerMaze(ReadOnlySpan<string> input)
    {
        char[][] result = new char[input.Length][];
        for (int y=0; y<input.Length; y++)
        {
            for(int x=0; x < input[0].Length; x++)
            {
                result[y][x] = input[y][x];
            }
        }

        return result;
    }

    private class Reindeer
    {
        private int PointScore;
        private Direction FacingDirection;
        private (int Y, int X) Position;
        HashSet<(int Y, int X)> WalkedPath;
        public Reindeer(int y, int x, Direction dir, int pScore, HashSet<(int Y, int X)> walkedPath)
        {
            Position.Y = y;
            Position.X = x;
            FacingDirection = dir;
            PointScore = pScore;
            WalkedPath = walkedPath;
        }

        public Reindeer Copy()
        {
            return new Reindeer(Position.Y, Position.X,FacingDirection,PointScore,WalkedPath);
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