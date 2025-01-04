

using System;

namespace Day14;
public class Part01Old : IPart
{
    private List<Robot> roboInfoList = new();
    private int bathWidth = 101;
    private int bathHeight = 103;

    private int SecondLimit = 100;
    private int[,] bathroom;

    public string Result(Input input)
    {
        long q1 = 0;
        long q2 = 0;
        long q3 = 0;
        long q4 = 0;
        bathroom = new int[bathHeight, bathWidth];
        ParseInput(input.SpanLines);

        for (int s = 0; s <= SecondLimit; s++)
        {
            Array.Clear(bathroom, 0, bathroom.Length);
            for (int r = 0; r < roboInfoList.Count; r++)
            {
                var roboPos = CalculateRoboPosition(roboInfoList[r], s);
                bathroom[roboPos.roboY, roboPos.roboX] += 1;
                if (s == SecondLimit)
                {
                    if (roboPos.roboY < bathHeight / 2)
                    {
                        if (roboPos.roboX < bathWidth / 2) q1++;
                        else if (roboPos.roboX > bathWidth / 2) q2++;
                    }
                    else if (roboPos.roboY > bathHeight / 2)
                    {
                        if (roboPos.roboX < bathWidth / 2) q4++;
                        else if (roboPos.roboX > bathWidth / 2) q3++;
                    }
                }
            }

        }
            DrawBathGrid(100);
        GlobalLog.LogLine($"Q1:{q1}; Q2:{q2}; Q3:{q3}; Q4;{q4}");
        return (q1 * q2 * q3 * q4).ToString();
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

    private (int roboX, int roboY) CalculateRoboPosition(Robot robo, int s)
    {

        var roboX = robo.Pos.X + robo.Vel.X * s;
        var roboY = robo.Pos.Y + robo.Vel.Y * s;

        while (roboX >= bathWidth) roboX -= bathWidth;
        while (roboX < 0) roboX += bathWidth;

        while (roboY >= bathHeight) roboY -= bathHeight;
        while (roboY < 0) roboY += bathHeight;

        return (roboX, roboY);
    }

    private void ParseInput(ReadOnlySpan<string> input)
    {
        foreach (var line in input)
        {

            int pStart = line.IndexOf("p=") + 2;
            int vStart = line.IndexOf("v=") + 2;


            var pPart = line.AsSpan(pStart, line.IndexOf(' ', pStart) - pStart);
            var vPart = line.AsSpan(vStart);


            int commaP = pPart.IndexOf(',');
            int pX = int.Parse(pPart[..commaP]);
            int pY = int.Parse(pPart[(commaP + 1)..]);


            int commaV = vPart.IndexOf(',');
            int vX = int.Parse(vPart[..commaV]);
            int vY = int.Parse(vPart[(commaV + 1)..]);

            roboInfoList.Add(new Robot(new Position(pX, pY), new Position(vX, vY)));
        }
    }


    private struct Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    private struct Robot
    {
        public Position StartPos { get; set; }
        public Position Vel { get; set; }
        public Position Pos { get; set; }
        public int Time { get; set; }

        public Robot(Position startPos, Position vel)
        {
            StartPos = startPos;
            Vel = vel;
            Pos = startPos;
            Time = 0;
        }
    }
}

