

using System;
using System.Numerics;

namespace Day14;
public class Part01 : IPart
{
    private List<Robot> roboInfoList = new();
    private int bathWidth = 101;
    private int bathHeight = 103;

    private int SecondLimit = 100;
    private int[,] bathroom;
        long Q1 = 0;
        long Q2 = 0;
        long Q3 = 0;
        long Q4 = 0;

    public string Result(Input input)
    {
        bathroom = new int[bathHeight, bathWidth];
        ParseInput(input.SpanLines);

        foreach (var robo in roboInfoList)
        {
            CalculatePosition(robo, SecondLimit);
            QuarterCalc(robo);

            bathroom[(int)robo.Position.Y, (int)robo.Position.X]++;
        }
        DrawBathGrid(SecondLimit);
        GlobalLog.LogLine($"Q1:{Q1}; Q2:{Q2}; Q3:{Q3}; Q4;{Q4}");
        return (Q1*Q2*Q3*Q4).ToString();
    }

    private void QuarterCalc(Robot robo)
    {
        if (robo.Position.X < bathWidth / 2)
        {
            if (robo.Position.Y < bathHeight / 2)
            {
                Q1++;
            }
            else if (robo.Position.Y > bathHeight / 2)
            {
                Q2++;
            }
        }
        else if (robo.Position.X > bathWidth / 2)
        {
            if (robo.Position.Y < bathHeight / 2)
            {
                Q3++;
            }
            else if (robo.Position.Y > bathHeight / 2)
            {
                Q4++;
            }
        }

    }

    private void CalculatePosition(Robot robo, int seconds)
    {
        var vec = robo.Position + robo.Velocity * seconds;
        robo.Position = CalcTeleportation(vec);
        robo.Seconds += seconds;
    }

    private Vector2 CalcTeleportation(Vector2 vec)
    {
        float AdjustedX = (vec.X % bathWidth + bathWidth) % bathWidth;
        float AdjustedY = (vec.Y % bathHeight + bathHeight) % bathHeight;

        return new Vector2(AdjustedX, AdjustedY);
    }

    private void ParseInput(ReadOnlySpan<string> spanLines)
    {
        foreach (string line in spanLines)
        {
            int[] data = line.Split(new[] {' ', ',', 'p','v','='}, StringSplitOptions.RemoveEmptyEntries)
                                .Select(int.Parse).ToArray();
            roboInfoList.Add(new Robot(new Vector2(data[0], data[1]), new Vector2(data[2], data[3])));
        }
    }

    private class Robot
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public int Seconds;

        public Robot(Vector2 position, Vector2 velocity, int seconds=0)
        {
            Position = position;
            Velocity = velocity;
            Seconds = seconds;
        }
    }
    [System.Diagnostics.Conditional("LOGGING_ENABLED")]
    private void DrawBathGrid(int s = 0)
    {
        GlobalLog.LogLine($"###################################################");
        GlobalLog.LogLine($"### Seconds = {s}###");
        GlobalLog.LogLine($"###################################################");
        for (int h = 0; h < bathHeight; h++)
        {
            for (int w = 0; w < bathWidth; w++)
            {
                string drawPoint = ".";
                if (bathroom[h, w] > 0)
                {
                    drawPoint = bathroom[h, w].ToString();
                }
                if (bathWidth / 2 == w)
                {
                    drawPoint = "|";
                }
                if (bathHeight / 2 == h)
                {
                    drawPoint = "-";
                }

                Console.Write($"{drawPoint}");
            }
            Console.WriteLine();
        }
    }

}