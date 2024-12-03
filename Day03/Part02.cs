
using System.Text.RegularExpressions;

namespace Day03;
public static class Part02
{
    public static int Result(string input)
    {

        // https://learn.microsoft.com/de-de/dotnet/api/system.text.regularexpressions.regex.match?view=net-8.0
        string doPattern = @"do\(\)";
        string dontPattern = @"don't\(\)";

        List<int> dontIndex = new List<int>();
        Regex doReg = new Regex(doPattern);
        Regex dontReg = new Regex(dontPattern);
        foreach (Match match in dontReg.Matches(input))
        {

            bool doBool = true; 
            int start = match.Index;

            int result = 0;
        input.Substring(result, input.Length - result);
        foreach (string line in lines)
        {
            string pattern = @"mul\(\d{1,3},\d{1,3}\)";
            Regex mulReg = new Regex(pattern);

          

        }

        //sw.Stop();
        Console.WriteLine($"safeReports {safeReports}");
        return safeReports;
    }

    public static int Result_Improved(string input)
    {

        int safeReports = 0;

        Console.WriteLine($"safeReports {safeReports}");
        return safeReports;
    }
    private static ReadOnlySpan<int> CreateSlicedSpan(ReadOnlySpan<int> span, int skipIndex)
    {
        if (skipIndex == 0)
            return span.Slice(1); 

        if (skipIndex == span.Length - 1)
            return span.Slice(0, span.Length - 1); 

   
        return CombineSlices(span.Slice(0, skipIndex), span.Slice(skipIndex + 1));
    }
    private static int[] CombineSlices(ReadOnlySpan<int> part1, ReadOnlySpan<int> part2)
    {
        int[] result = new int[part1.Length + part2.Length];
        part1.CopyTo(result);
        part2.CopyTo(result.AsSpan(part1.Length));
        return result;
    }


}
