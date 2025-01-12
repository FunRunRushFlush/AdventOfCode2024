
using CommunityToolkit.HighPerformance;

namespace Year_2024.Days.Day05;

public class Part01 : IPart
{
    public string Result(Input rawInput)
    {
        InputParser(rawInput.Lines, out ReadOnlySpan2D<bool> rules2D,
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
        return midCounter.ToString();
    }

    public int Result_Improved01(ReadOnlySpan<string> rawInput)
    {
        InputParser(rawInput, out ReadOnlySpan2D<bool> rules2D,
       out ReadOnlySpan<int[]> parsedInput);
        int midCounter = 0;
        for (int i = 0; i < parsedInput.Length; i++)
        {
            bool exitedEarly = false;

            for (int j = 0; j < parsedInput[i].Length - 1; j++)
            {
                //var num1 = parsedInput[i][j];
                //var num2 = parsedInput[i][j + 1];
                //Console.Write(parsedInput[i][j]);
                if (rules2D[parsedInput[i][j], parsedInput[i][j + 1]])
                {
                    continue;
                }
                else if (rules2D[parsedInput[i][j + 1], parsedInput[i][j]])
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

    public void InputParser(
       ReadOnlySpan<string> rawInput,
       out ReadOnlySpan2D<bool> rules2D,
       out ReadOnlySpan<int[]> parsedInput)
    {
        int separatorIndex = Array.IndexOf(rawInput.ToArray(), string.Empty);

        Span2D<bool> rules = new bool[100, 100];
        for (int i = 0; i < separatorIndex; i++)
        {
            var test = rawInput[i].Split('|').AsSpan();
            rules[int.Parse(test[0]), int.Parse(test[1])] = true;
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
    public void InputParserNoOut(
   ReadOnlySpan<string> rawInput)
    {
        int separatorIndex = Array.IndexOf(rawInput.ToArray(), string.Empty);

        Span2D<bool> rules = new bool[100, 100];
        for (int i = 0; i < separatorIndex; i++)
        {
            var test = rawInput[i].Split('|').AsSpan();
            rules[int.Parse(test[0]), int.Parse(test[1])] = true;
        }



        var input = rawInput.Slice(separatorIndex + 1);
        Span<int[]> parsedInpu = new int[input.Length][];
        for (int i = 0; i < input.Length; i++)
        {
            parsedInpu[i] = input[i].Split(',').Select(int.Parse).ToArray();
        }

    }
}