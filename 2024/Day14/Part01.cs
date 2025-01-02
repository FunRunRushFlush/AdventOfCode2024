

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
        }

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
            else
            {
                Q2++;
            }
        }
        else
        {
            if (robo.Position.Y < bathHeight / 2)
            {
                Q3++;
            }
            else
            {
                Q4++;
            }
        }

    }

    private void CalculatePosition(Robot robo, int seconds)
    {
        var vec = robo.Position* robo.Velocity* seconds;
        robo.Position = CalcTeleportation(vec);
        robo.Seconds += seconds;
    }

    private Vector2 CalcTeleportation(Vector2 vec)
    {
        Vector2 res = new Vector2();
        if (vec.X < 0)
        {
            res.X = bathWidth + (vec.X % bathWidth);
        }
        if (vec.X >= bathWidth)
        {
            res.X = (vec.X % bathWidth);
        }
        if (vec.Y < 0)
        {
            res.Y = bathHeight + (vec.Y % bathHeight);
        }
        if (vec.X >= bathWidth)
        {
            res.Y = (vec.Y % bathHeight);
        }
        return res;
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
}