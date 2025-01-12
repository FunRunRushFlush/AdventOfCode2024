using System;
using System.Numerics;

namespace Year_2024.Days.Day14;


//TODO: anscheined kann man hier https://en.wikipedia.org/wiki/Chinese_remainder_theorem verwenden
//muss ich mir noch anschauen
public class Part02 : IPart
{
    private List<Robot> roboInfoList = new();
    private int bathWidth = 101;
    private int bathHeight = 103;

    private int SecondLimit = 10000;
    private int[,] bathroom;
    int[] Cluster = new int[9];

    public string Result(Input input)
    {
        bathroom = new int[bathHeight, bathWidth];
        ParseInput(input.SpanLines);
        double minFoundEntropie = double.MaxValue;
        int secondOfMinEnttropie = 0;
        List<(double Ent, int Sec)> entropieList = new List<(double, int)>();
        int maxCluster = 0;
        int second = 0;
        for (int i = 1; i < SecondLimit; i++)
        {
            Array.Clear(bathroom, 0, bathroom.Length);
            foreach (var robo in roboInfoList)
            {
                CalculatePosition(robo, 1);
                //QuarterCalc(robo);
                ClusterCalc(robo.Position);
            }


            foreach (int cluster in Cluster)
            {
                if (cluster > maxCluster)
                {
                    maxCluster = cluster;
                    second = i;
                }
            }
            Array.Clear(Cluster, 0, Cluster.Length);
        }


        GlobalLog.LogLine($"Second: {second}");
        GlobalLog.LogLine($"MaxCluster: {maxCluster}");

        return second.ToString();
    }

    private void ClusterCalc(Vector2 pos)
    {

        if (pos.Y >= 0 && pos.Y <= 34)
        {
            if (pos.X >= 0 && pos.X <= 33) Cluster[0]++; // Cluster01
            if (pos.X >= 34 && pos.X <= 67) Cluster[1]++; // Cluster02
            if (pos.X >= 68 && pos.X <= 100) Cluster[2]++; // Cluster03
        }
        else if (pos.Y >= 35 && pos.Y <= 69)
        {
            if (pos.X >= 0 && pos.X <= 33) Cluster[3]++; // Cluster04
            if (pos.X >= 34 && pos.X <= 67) Cluster[4]++; // Cluster05
            if (pos.X >= 68 && pos.X <= 100) Cluster[5]++; // Cluster06
        }
        else if (pos.Y >= 70 && pos.Y <= 102)
        {
            if (pos.X >= 0 && pos.X <= 33) Cluster[6]++; // Cluster07
            if (pos.X >= 34 && pos.X <= 67) Cluster[7]++; // Cluster08
            if (pos.X >= 68 && pos.X <= 100) Cluster[8]++; // Cluster09
        }

    }

    private double CalculateEntropie()
    {
        var numOfRobos = roboInfoList.Count;
        double entropie = 0;
        for (int y = 0; y < bathHeight; y++)
        {
            for (int x = 0; x < bathWidth; x++)
            {
                if (bathroom[y, x] == 0) continue;

                double p = bathroom[y, x] / (double)numOfRobos;
                entropie += Math.Abs(p * Math.Log2(p));

            }
        }
        return entropie;
    }

    //private void QuarterCalc(Robot robo)
    //{
    //    if (robo.Position.X < bathWidth / 2)
    //    {
    //        if (robo.Position.Y < bathHeight / 2)
    //        {
    //            Q1++;
    //        }
    //        else if (robo.Position.Y > bathHeight / 2)
    //        {
    //            Q2++;
    //        }
    //    }
    //    else if (robo.Position.X > bathWidth / 2)
    //    {
    //        if (robo.Position.Y < bathHeight / 2)
    //        {
    //            Q3++;
    //        }
    //        else if (robo.Position.Y > bathHeight / 2)
    //        {
    //            Q4++;
    //        }
    //    }

    //}

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
            int[] data = line.Split(new[] { ' ', ',', 'p', 'v', '=' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(int.Parse).ToArray();
            roboInfoList.Add(new Robot(new Vector2(data[0], data[1]), new Vector2(data[2], data[3])));
        }
    }

    private class Robot
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public int Seconds;

        public Robot(Vector2 position, Vector2 velocity, int seconds = 0)
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
                //if (bathWidth / 2 == w)
                //{
                //    drawPoint = "|";
                //}
                //if (bathHeight / 2 == h)
                //{
                //    drawPoint = "-";
                //}

                Console.Write($"{drawPoint}");
            }
            Console.WriteLine();
        }
    }

}