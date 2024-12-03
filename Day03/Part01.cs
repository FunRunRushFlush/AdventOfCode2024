
using System.Text.RegularExpressions;

namespace Day03;

public static class Part01
{
    public static int Result(string input)
    {
        // https://learn.microsoft.com/de-de/dotnet/api/system.text.regularexpressions.regex.match?view=net-8.0
        string pattern = @"mul\(\d{1,3},\d{1,3}\)";
        Regex reg = new Regex(pattern);
        int result = 0;
        foreach(Match match in reg.Matches(input))
        {
            var value = match.Value.Split(new[] { '(', ')', ',', }, StringSplitOptions.RemoveEmptyEntries);

            int num1 = int.Parse(value[1]);
            int num2 = int.Parse(value[2]);

            int multRes = num1 * num2;
            result += multRes;

        }
        Console.WriteLine(result);
        return result;


    }


    public static int Result_Improved(string input)
    {
        // https://learn.microsoft.com/de-de/dotnet/api/system.text.regularexpressions.regex.match?view=net-8.0
        string pattern = @"mul\(\d{1,3},\d{1,3}\)";
        Regex reg = new Regex(pattern);

        foreach (Match match in reg.Matches(input))
        {
            var value = match.ValueSpan;

            value.IndexOf(',');
        }
        return 0;
    }

    public static int[][] InputParser(string input)
    {
        List<int[]> data = new List<int[]>();

        foreach (var line in input.AsSpan().EnumerateLines())
        {
            var innerData = new List<int>();

            foreach (var range in line.Split(' '))
            {
                innerData.Add(IntParser(line[range]));
            }

            data.Add(innerData.ToArray());
        }

        return data.ToArray();
    }

    // Simpler IntParser der perfomanter ist als Int.Parse()
    // WARNUNG: Aufkosten von Robustheit(kein edgecases etc...)
    // https://youtu.be/EWmufbVF2A4?feature=shared&t=880 
    private static int IntParser(ReadOnlySpan<Char> span)
    {
        int temp = 0;
        for (int i = 0; i < span.Length; i++)
        {
            // Der ASCII-Wert des Zeichens (z. B. '3' → ASCII 51) wird von dem ASCII-Wert von '0' (ASCII 48) subtrahiert.
            // Dadurch wird der numerische Wert des Zeichens erhalten (z. B. '3' → 3).
            temp = temp * 10 + (span[i] - '0');
        }
        return temp;
    }


}
