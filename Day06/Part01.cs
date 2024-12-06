
using CommunityToolkit.HighPerformance;
using MethodTimer;
using System.Drawing;


namespace Day06;

public static class Part01
{
    public static int Result(string[] rawInput)
    {
        return 1;
    }


    public static void InputParser(
        ReadOnlySpan<string> rawInput,
           out ReadOnlySpan2D<bool> rules2D,
           out ReadOnlySpan<int[]> parsedInput)
    {
        //int separatorIndex = Array.IndexOf(rawInput.ToArray(), string.Empty);

            Point position = new Point(0, 0);
            Span<char[]> map2D = new char[rawInput[0].Length][];
            for (int i = 0; i < map2D.Length; i++)
            {
                for (int j = 0; j < rawInput[0].Length; j++)
                {
                    if(rawInput[i][j] == '^')
                        return position



                }
            }
            rules2D = rules;


            var input = rawInput.Slice(separatorIndex + 1);
            Span<int[]> parsedInpu = new int[input.Length][];
            for (int i = 0; i < input.Length; i++)
            {
                parsedInpu[i] = input[i].Split(',').Select(int.Parse).ToArray();
            }
            parsedInput = parsedInpu;
        }
    }