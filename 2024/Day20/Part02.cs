using System.Numerics;

namespace Day20;
public class Part02 : IPart
{
    private Vector2 CarStartPos;
    private Vector2 CarEndPos;
    private Vector2 CarPos;

    private Dir CarDir;

    private const char RaceTile = '.';
    Dictionary<Vector2, int> RaceTime = new Dictionary<Vector2, int>();
    private int Seconds = 0;
    private int Cheats = 0;

    private Vector2 Up = new Vector2(0, -1);
    private Vector2 Right = new Vector2(1, 0);
    private Vector2 Down = new Vector2(0, 1);
    private Vector2 Left = new Vector2(-1, 0);

    private int MapHeight;
    private int MapWidth;
    private int[,] RaceMap;



    public string Result(Input input)
    {
        ParseInput(input.Lines);
        MapHeight=input.Lines.Length;
        MapWidth = input.Lines[0].Length;
        RaceMap = new int[MapHeight,MapWidth]; 
        CarStarDir(input.Lines);
        CarPos = new Vector2(CarStartPos.X, CarStartPos.Y);
        RaceQualifier(input.Lines);

        foreach (var time in RaceTime)
        { 
            if(time.Value > Seconds-100) continue;
            CheatStamp20x20(time);
        }
        return Cheats.ToString();
    }

    private void CheatStamp20x20(KeyValuePair<Vector2, int> time)
    {
        for (int y = (int)time.Key.Y - 20; y<=(int)time.Key.Y + 20 ; y++)
        {
            if(y>= MapHeight|| y<0 ) continue;
            for (int x = (int)time.Key.X - 20;x<= (int)time.Key.X + 20; x++)
            {
                if (x >= MapWidth || x < 0) continue;
                if (RaceMap[y, x] < 100) continue;

                var dis= Math.Abs(y- time.Key.Y) + Math.Abs(x-time.Key.X);
                if (dis > 20 || dis==0) continue;

                //TODO: Learning: Bei engen loops ist es besser auf eine Array als ein Dic zuzugreifen
                if (RaceMap[y, x] - (time.Value + dis) >= 100) Cheats++;
                
            }
        }

    }

    private void CarStarDir(string[] raceTrack)
    {
        for (int i = 0; i < 4; i++)
        {
            var carDirCheck = CarStartPos + DirVec((Dir)i);
            if (raceTrack[(int)carDirCheck.Y][(int)carDirCheck.X] == RaceTile)
            {
                CarDir = (Dir)i;
                break;
            }
        }
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

    private void RaceQualifier(string[] raceTrack)
    {
        while (true)
        {
            RaceTime.Add(CarPos, Seconds);
            RaceMap[(int)CarPos.Y,(int)CarPos.X] = Seconds;
            if (CarPos == CarEndPos) break;

            var carDirCheck = CarPos + DirVec(CarDir);
            if (raceTrack[(int)carDirCheck.Y][(int)carDirCheck.X] == RaceTile
                || raceTrack[(int)carDirCheck.Y][(int)carDirCheck.X] == 'E')
            {
                CarPos = carDirCheck;
                Seconds++;
                continue;

            }
            Dir right = (Dir)(((int)CarDir + 1) % 4);
            var carDirRightCheck = CarPos + DirVec(right);
            if (raceTrack[(int)carDirRightCheck.Y][(int)carDirRightCheck.X] == RaceTile
                || raceTrack[(int)carDirRightCheck.Y][(int)carDirRightCheck.X] == 'E')
            {
                CarPos = carDirRightCheck;
                CarDir = right;
                Seconds++;
                continue;
            }
            Dir left = (Dir)(((int)CarDir + 3) % 4);
            var carDirLeftCheck = CarPos + DirVec(left);
            if (raceTrack[(int)carDirLeftCheck.Y][(int)carDirLeftCheck.X] == RaceTile || raceTrack[(int)carDirLeftCheck.Y][(int)carDirLeftCheck.X] == 'E')
            {
                CarPos = carDirLeftCheck;
                CarDir = left;
                Seconds++;
                continue;
            }

        }
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

                if (RaceTime.ContainsKey(new Vector2(w, h)))
                {
                    drawPoint = "X";
                }

                Console.Write($"{drawPoint}");
            }
            Console.WriteLine();
        }
        Console.WriteLine("-------------------------------------------");
        Console.WriteLine();
    }

    private void ParseInput(string[] lines)
    {
        bool startFound = false;
        bool endFound = false;
        for (int y = 0; y < lines.Length; y++)
        {
            if (startFound && endFound) break;

            var xS = lines[y].IndexOf('S');
            var xE = lines[y].IndexOf('E');
            if (xS >= 0)
            {
                CarStartPos = new Vector2(xS, y);
                startFound = true;
            }
            if (xE >= 0)
            {
                CarEndPos = new Vector2(xE, y);
                endFound = true;
            }
        }
    }
}