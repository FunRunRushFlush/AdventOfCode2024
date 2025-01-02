
using System.Numerics;


namespace Day06;

public class Part02 : IPart
{
    //y <--> x vertauscht
    private HashSet<Vector2> WallMap = new HashSet<Vector2>();
    private Dictionary<Vector2, Dir> UniqueSteps = new Dictionary<Vector2, Dir>();
    private Dir PlayerDir = Dir.Up;
    private Vector2 PlayerPosition;
    private Vector2 WallDetection = new Vector2(-1, 0);
    private int MapHeight;
    private int MapWidth;

    private Vector2 LoopPlayerStartPos;
    private Dir LoopPlayerStartDir;
    private Vector2 LoopStartWallDetection;
    private Dictionary<Vector2, Dir> LoopUniqueSteps = new Dictionary<Vector2, Dir>(); 



    private const char Wall = '#';
    private const char Player = '^';
    public string Result(Input rawInput)
    {
        ParseStartingInput(rawInput.Lines);
        UniqueSteps.Add(PlayerPosition, PlayerDir);


        LoopPlayerStartPos = PlayerPosition;
        LoopPlayerStartDir = PlayerDir;
        LoopStartWallDetection = WallDetection;


        MapHeight = rawInput.Lines.Length;
        MapWidth = rawInput.Lines[0].Length;

        

        while (true)
        {
            if (CheckOutOfBounds()) break;
            //GlobalLog.Log($"X: {PlayerPosition.Y} Y:{PlayerPosition.X} ");

            if (!CheckForWall())
            {
                PlayerPosition += WallDetection;
                UniqueSteps.TryAdd(PlayerPosition,PlayerDir);
                //GlobalLog.Log($"X: {PlayerPosition.Y} Y:{PlayerPosition.X} ");
            }
        }
        //DrawGrid(rawInput.Lines);
        bool foundTheLoop= false;
        int uniqueLoopSteps=0;
        foreach(var ele in UniqueSteps)
        {
            //if(foundTheLoop) break;

            LoopUniqueSteps.Clear();

            WallMap.Add(ele.Key);
            PlayerPosition = ele.Key;
            PlayerDir= ele.Value;
            WallDetection = WallDetectionVec();
            PlayerPosition -= WallDetection;
            LoopUniqueSteps.Add(PlayerPosition, PlayerDir);

            while (true)
            {
                if (CheckOutOfBounds()) break;
                //GlobalLog.Log($"X: {PlayerPosition.Y} Y:{PlayerPosition.X} ");

                if (!CheckForWall())
                {
                    PlayerPosition += WallDetection;
                    if(!LoopUniqueSteps.TryAdd(PlayerPosition, PlayerDir) && LoopUniqueSteps[PlayerPosition]==PlayerDir)
                    {
                        foundTheLoop = true;
                        uniqueLoopSteps++;
                        break;
                    }
                    //GlobalLog.Log($"X: {PlayerPosition.Y} Y:{PlayerPosition.X} ");
                }
            }
            WallMap.Remove(ele.Key);

        }
        return uniqueLoopSteps.ToString();

    }

    [System.Diagnostics.Conditional("LOGGING_ENABLED")]
    private void DrawGrid(string[] array)
    {
        var arrayHeight = array.Length;
        var arrayWidth = array[0].Length;

        GlobalLog.LogLine($"DrawGrid");

        for (int h = 0; h < arrayHeight; h++)
        {
            for (int w = 0; w < arrayWidth; w++)
            {
                string drawPoint = ".";
                //if (array[h, w] > 0)
                //{
                drawPoint = array[h][w].ToString();
                //}

                if (UniqueSteps.ContainsKey(new Vector2(h, w)))
                {
                    drawPoint = "X";
                }
                if (h == PlayerPosition.X && w == PlayerPosition.Y)
                {
                    drawPoint = "@";
                }
                Console.Write($"{drawPoint}");
            }
            Console.WriteLine();
        }
        Console.WriteLine("-------------------------------------------");
        Console.WriteLine();
    }

    private bool CheckOutOfBounds()
    {
        var nextStepPos = PlayerPosition + WallDetection;

        if (nextStepPos.Y >= MapWidth || nextStepPos.Y < 0)
            return true;
        if (nextStepPos.X >= MapHeight || nextStepPos.X < 0)
            return true;

        return false;
    }

    private bool CheckForWall()
    {
        if (WallMap.Contains(PlayerPosition + WallDetection))
        {
            PlayerDir = (Dir)(((int)PlayerDir + 1) % 4);
            WallDetection = WallDetectionVec();
            return true;
        }
        return false;
    }

    private Vector2 WallDetectionVec()
    {
        return PlayerDir switch
        {
            Dir.Up => new Vector2(-1, 0),
            Dir.Down => new Vector2(1, 0),
            Dir.Right => new Vector2(0, 1),
            Dir.Left => new Vector2(0, -1),
            _ => throw new NotImplementedException()
        };
    }

    private void ParseStartingInput(string[] lines)
    {

        bool playerFound = false;
        for (int y = 0; y < lines.Length; y++)
        {
            var line = lines[y];
            if (!playerFound)
            {
                int xPos = line.IndexOf(Player);
                if (xPos > -1)
                {
                    PlayerPosition = new Vector2(y, xPos);
                    playerFound = true;
                }
            }

            int x = line.IndexOf(Wall);
            while (x != -1)
            {
                WallMap.Add(new Vector2(y, x));
                x = line.IndexOf(Wall, x + 1);
            }
        }
    }


    enum Dir
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3,
    }
}