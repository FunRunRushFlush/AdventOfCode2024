
using CommunityToolkit.HighPerformance;
using Microsoft.Diagnostics.Runtime.Utilities;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Day06;
public class Part02Old : IPart
{
    public string Result(Input rawInput)
    {
        GetGuardPosition(rawInput.Lines, out Point guPos);
        Console.WriteLine(guPos);

        bool loop = true;
        bool dirClear = false;
        int xDir = 0;
        int yDir = 0;

        int numberOfLoops = 0;
        for (int i = 0; i < rawInput.Lines.Length; i++)
        {
            for (int j = 0; j < rawInput.Lines[0].Length; j++)
            {
                GuardPart02 guard = new GuardPart02(guPos);
                loop = true;
                if (rawInput.Lines[i][j] == '#' || (guPos.Y, guPos.X) == (i, j))
                {
                    continue;
                }

                GlobalLog.LogLine($"#_@: Y:{i} X:{j}");

                while (loop)
                {
                    try
                    {
                        (xDir, yDir) = guard.CheckPathKord();
                        if (rawInput.Lines[yDir][xDir] == '#' || (yDir, xDir) == (i, j))
                        {
                            GlobalLog.LogLine($"Blocker @: Y:{yDir} X:{xDir}");
                            dirClear = false;
                        }

                        while (!dirClear)
                        {
                            (xDir, yDir) = guard.CheckPathKord();
                            if (rawInput.Lines[yDir][xDir] == '#' || (yDir, xDir) == (i, j))
                            {
                                GlobalLog.LogLine($"Blocker @: Y:{yDir} X:{xDir}");
                                guard.TurnRight();
                            }
                            else
                            {
                                dirClear = true;
                            }
                        }

                        guard.MoveOneStep();
                        var pos = guard.GetPosition();

                        if (guard.GetLoopCounter() > 0)
                        {
                            numberOfLoops++;
                            loop = false;
                        }

                        GlobalLog.LogLine($"GuardPosition : Y:{pos.Y} X:{pos.X}");
                    }
                    catch (IndexOutOfRangeException ex)
                    {
                        loop = false;
                    }
                }

            }

        }
        return numberOfLoops.ToString();
    }

    public int Result_Improved01(string[] rawInput)
    {
        GetGuardPosition(rawInput, out Point guPos);
        Console.WriteLine(guPos);

        bool loop = true;
        bool dirClear = false;
        int xDir = 0;
        int yDir = 0;

        int numberOfLoops = 0;
        for (int i = 0; i < rawInput.Length; i++)
        {
            for (int j = 0; j < rawInput[0].Length; j++)
            {
                GuardPart02 guard = new GuardPart02(guPos);
                loop = true;
                if (rawInput[i][j] == '#' || (guPos.Y, guPos.X) == (i, j))
                {
                    continue;
                }

                //GlobalLog.Log($"#_@: Y:{i} X:{j}");

                while (loop)
                {
                    try
                    {
                        (xDir, yDir) = guard.CheckPathKord();
                        if (rawInput[yDir][xDir] == '#' || (yDir, xDir) == (i, j))
                        {
                            //GlobalLog.Log($"Blocker @: Y:{yDir} X:{xDir}");
                            dirClear = false;
                        }

                        while (!dirClear)
                        {
                            (xDir, yDir) = guard.CheckPathKord();
                            if (rawInput[yDir][xDir] == '#' || (yDir, xDir) == (i, j))
                            {
                                //GlobalLog.Log($"Blocker @: Y:{yDir} X:{xDir}");
                                guard.TurnRight();
                            }
                            else
                            {
                                dirClear = true;
                            }
                        }

                        guard.MoveOneStep();
                        var pos = guard.GetPosition();

                        if (guard.GetLoopCounter() > 0)
                        {
                            numberOfLoops++;
                            loop = false;
                        }

                        //GlobalLog.Log($"GuardPosition : Y:{pos.Y} X:{pos.X}");
                    }
                    catch (IndexOutOfRangeException ex)
                    {
                        loop = false;
                    }
                }

            }

        }
        return numberOfLoops;
    }

    public void GetGuardPosition(Span<string> rawInput, out Point position)
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