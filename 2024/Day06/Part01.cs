
using CommunityToolkit.HighPerformance;
using System.Data;
using System.Drawing;


namespace Day06;

public class Part01 : IPart
{
    public string Result(Input rawInput)
    {
        GetGuardPosition(rawInput.Lines, out Point guPos);
        GlobalLog.LogLine($"{guPos}");
        Guard guard = new Guard(guPos);
        bool loop = true;
        bool dirClear = false;
        int xDir = 0;
        int yDir = 0;
        while (loop)
        {
            try
            {
                (xDir, yDir) = guard.CheckPathKord();
                if (rawInput.Lines[yDir][xDir] == '#')
                {
                    GlobalLog.LogLine($"Blocker @: Y:{yDir} X:{xDir}");
                    dirClear=false;
                }
                    
                while (!dirClear)
                {
                    (xDir, yDir) = guard.CheckPathKord();
                    if (rawInput.Lines[yDir][xDir] == '#')
                    {
                        GlobalLog.LogLine($"Blocker @: Y:{yDir} X:{xDir}");
                        guard.TurnRight();
                    }
                    else 
                    {
                        dirClear=true;
                    }
                }

                guard.MoveOneStep();
                var pos = guard.GetPosition();
                var dir = guard.GetPosition();
                GlobalLog.LogLine($"GuardPosition : Y:{pos.Y} X:{pos.X}");
                GlobalLog.LogLine($"GuardDirection : Y:{dir.Y} X:{dir.X}");


            }
            catch (IndexOutOfRangeException ex)
            {
                loop = false;
            }

        }

        return guard.GetUniqueCoordinat().ToString();
    }

    public void GetGuardPosition(ReadOnlySpan<string> rawInput, out Point position)
    {
        Point positionTemp = new Point(0, 0);
        for (int i = 0; i < rawInput.Length; i++)
        {
            for (int j = 0; j < rawInput[0].Length; j++)
            {
                if (rawInput[i][j] == '^')
                {
                    positionTemp.Y = i;
                    positionTemp.X = j;
                }

            }
        }
        position = positionTemp;
    }
}
