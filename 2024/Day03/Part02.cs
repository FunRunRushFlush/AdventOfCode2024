
using Microsoft.Diagnostics.Runtime.Utilities;
using System.Text.RegularExpressions;

namespace Day03;
public class Part02 : IPart
{

    public string Result(string input)
    {
        // https://learn.microsoft.com/de-de/dotnet/api/system.text.regularexpressions.regex.match?view=net-8.0
        string doPattern = @"do\(\)";
  
        Regex doReg = new Regex(doPattern);

        var test = doReg.Split(input);

        int counter = 0;
        for( int i=0; i<test.Length; i++)
        {
            var line = test[i].Split("don't()").FirstOrDefault();
            counter +=CalculateMul(line);

        }

        return $"{counter}";
    }

    private int CalculateMul(string stringInput)
    {
        string pattern = @"mul\(\d{1,3},\d{1,3}\)";
        Regex reg = new Regex(pattern);
        int result = 0;
        foreach (Match match in reg.Matches(stringInput))
        {
            var value = match.Value.Split(new[] { '(', ')', ',', }, StringSplitOptions.RemoveEmptyEntries);

            int num1 = int.Parse(value[1]);
            int num2 = int.Parse(value[2]);

            int multRes = num1 * num2;
            result += multRes;

        }
        return result;

    }
    //public static int Result_Improved(string input)
    //{

        


    //    return safeReports;
    //}
    private ReadOnlySpan<int> CreateSlicedSpan(ReadOnlySpan<int> span, int skipIndex)
    {
        if (skipIndex == 0)
            return span.Slice(1); 

        if (skipIndex == span.Length - 1)
            return span.Slice(0, span.Length - 1); 

   
        return CombineSlices(span.Slice(0, skipIndex), span.Slice(skipIndex + 1));
    }
    private int[] CombineSlices(ReadOnlySpan<int> part1, ReadOnlySpan<int> part2)
    {
        int[] result = new int[part1.Length + part2.Length];
        part1.CopyTo(result);
        part2.CopyTo(result.AsSpan(part1.Length));
        return result;
    }


}
