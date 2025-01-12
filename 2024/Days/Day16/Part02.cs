using System.Numerics;

namespace Year_2024.Days.Day16;
public class Part02 : IPart
{

    private const char Wall = '#';
    private const char End = 'E';
    private const char Start = 'S';
    private const char Tile = '.';

    private char[,] Maze;

    private HashSet<Vector2> WallSet = new HashSet<Vector2>();
    private int Score = int.MaxValue;
    private Vector2 EndPosition;


    private Vector2 Up = new Vector2(0, -1);
    private Vector2 Right = new Vector2(1, 0);
    private Vector2 Down = new Vector2(0, 1);
    private Vector2 Left = new Vector2(-1, 0);

    public string Result(Input input)
    {
        var test = ParseInputDebug(input.Lines);
        var startPosition = ParseInput(input.Lines);
        Dir startDir = Dir.right;

        var uniqueTiles = SearchBestPath(startPosition, startDir);

        return uniqueTiles.ToString();
    }


    private int SearchBestPath(Vector2 startPos, Dir startDir)
    {
        var queue = new PriorityQueue<(Vector2 Pos, Dir Dir, int Score, HashSet<Vector2> Path), int>();
        queue.Enqueue((startPos, startDir, 0, new HashSet<Vector2>() { startPos }), 0);

        Dictionary<(Vector2 Pos, Dir Dir), int> visited = new();
        HashSet<Vector2> UniqueTiles = new HashSet<Vector2>();
        while (queue.Count > 0)
        {
            var (pos, dir, score, path) = queue.Dequeue();

            if (Score < score) continue;
            if (!visited.TryAdd((pos, dir), score))
            {
                if (visited[(pos, dir)] >= score)
                {
                    visited[(pos, dir)] = score;
                }
                else
                {
                    continue;
                }
            }


            if (pos == EndPosition)
            {
                Score = score;
                foreach (var step in path)
                {
                    UniqueTiles.Add(step);
                }

                GlobalLog.LogLine($"Found the End: {score}");
                continue;
            }
            var stVec = pos + DirVec(dir);
            if (!WallSet.Contains(stVec))
            {
                queue.Enqueue((stVec, dir, score + 1, new(path) { stVec }), score + 1);
            }
            Dir left = (Dir)(((int)dir + 3) % 4);
            Dir right = (Dir)(((int)dir + 1) % 4);
            var leVec = pos + DirVec(left);
            var riVec = pos + DirVec(right);
            if (!WallSet.Contains(leVec))
            {
                queue.Enqueue((leVec, left, score + 1001, new(path) { leVec }), score + 1001);
            }
            if (!WallSet.Contains(riVec))
            {
                queue.Enqueue((riVec, right, score + 1001, new(path) { riVec }), score + 1001);
            }
        }


        return UniqueTiles.Count;
    }

    private Vector2 ParseInput(string[] lines)
    {
        //Maze = new int[lines.Length,lines[0].Length];
        Vector2 startPos = new Vector2();
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[0].Length; x++)
            {
                if (lines[y][x] == Wall) WallSet.Add(new Vector2(x, y));
                if (lines[y][x] == End) EndPosition = new Vector2(x, y);
                if (lines[y][x] == Start) startPos = new Vector2(x, y);
            }
        }
        return startPos;
    }
    private Vector2 ParseInputDebug(string[] lines)
    {
        Maze = new char[lines.Length, lines[0].Length];
        Vector2 startPos = new Vector2();
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[0].Length; x++)
            {
                if (lines[y][x] == Wall) Maze[y, x] = Wall;
                if (lines[y][x] == End) Maze[y, x] = End;
                if (lines[y][x] == Start) Maze[y, x] = Start;
                if (lines[y][x] == Tile) Maze[y, x] = Tile;

            }
        }
        return startPos;
    }


    public enum Dir
    {
        up = 0,
        right = 1,
        down = 2,
        left = 3,
    }
    private Vector2 DirVec(Dir index)
    {
        return index switch
        {
            Dir.up => Up,
            Dir.right => Right,
            Dir.down => Down,
            Dir.left => Left,
            _ => throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and 3.")
        };
    }




}