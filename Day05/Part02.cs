
using CommunityToolkit.HighPerformance;
using Microsoft.Diagnostics.Runtime.Utilities;
using System.Text.RegularExpressions;

namespace Day05;
public static class Part02
{
    public static int Result(string[] rawInput)
    {
        InputParser(rawInput, out ReadOnlySpan2D<bool> rules2D,
       out ReadOnlySpan<int[]> parsedInput);
        int midCounter = 0;
        for (int i = 0; i < parsedInput.Length; i++)
        {
            bool exitedEarly = false;

            for (int j = 0; j < parsedInput[i].Length - 1; j++)
            {
                var num1 = parsedInput[i][j];
                var num2 = parsedInput[i][j + 1];
                //Console.Write(parsedInput[i][j]);
                if (rules2D[num1, num2])
                {
                    continue;
                }
                else if (rules2D[num2, num1])
                {
                    exitedEarly = true;
                    break;
                }
            }

            if (!exitedEarly)
            {
                midCounter += parsedInput[i][parsedInput[i].Length / 2];
            }

            //Console.WriteLine();
        }


        //Console.WriteLine("midCounter: {0}", midCounter);
        return midCounter;
    }

    public static void InputParser(
       string[] rawInput,
       out ReadOnlySpan2D<bool> rules2D,
       out ReadOnlySpan<int[]> parsedInput)
    {
        int separatorIndex = Array.IndexOf(rawInput, string.Empty);

        Span2D<bool> rules = new bool[100, 100];
        for (int i = 0; i < separatorIndex; i++)
        {
            var test = rawInput[i].Split('|');
            rules[int.Parse(test[0]), int.Parse(test[1])] = true;
        }
        rules2D = rules;


        var input = rawInput.AsSpan(separatorIndex + 1);
        Span<int[]> parsedInpu = new int[input.Length][];
        for (int i = 0; i < input.Length; i++)
        {
            var print = input[i].Split(',').Select(int.Parse).ToArray();
            parsedInpu[i] = print;
            foreach (var number in print)
            {
                Console.Write($"{number} ");
            }
            Console.WriteLine();
        }
        parsedInput = parsedInpu;


    }
}