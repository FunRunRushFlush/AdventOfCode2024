
namespace Day02;
public static class Part02
{
    public static int Result(string input)
    {
        //Stopwatch sw = Stopwatch.StartNew();
        string[] lines = input
        .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
        .ToArray();

        int safeReports = 0;
        foreach (string line in lines)
        {
            int[] numbers = line
                  .Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                  .Select(int.Parse)
                  .ToArray();
         
            for (int i = 0; i < numbers.Length; i++)
            {
                //TODO: Reference vs ValueType Bug --> Doku
                var copyNumbers = numbers.ToList();
                copyNumbers.RemoveAt(i);
                int[] dumperedReports = copyNumbers.ToArray();
                bool safe = CheckIfSafe(dumperedReports);
                if (safe)
                {
                    safeReports++;
                    break;
                }
            }           

        }

        //sw.Stop();
        Console.WriteLine($"safeReports {safeReports}");
        return safeReports;
    }

    public static int Result_Improved(string input)
    {
        var data = InputParser(input);
        int safeReports = 0;
        foreach (int[] numbers in data)
        {
                ReadOnlySpan<int> span = new ReadOnlySpan<int>(numbers);
            for (int i = 0; i < numbers.Length; i++)
            {
                var slicedArray = CreateSlicedSpan(span,i);
                if (CheckIfSafeSpan(slicedArray))
                {
                    safeReports++;
                    break;
                }
            }
        }
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


    private static bool CheckIfSafe(int[] intArray)
    {
        
        if (intArray.Length == 0) return false;

        if (intArray.Length == 1) return true;

        var direction = intArray[0] - intArray[1];
        if (direction == 0) return false;

        for (int i = 0; i < intArray.Length - 1; i++)
        {
            if (direction > 0)
            {
                var checking = intArray[i] - intArray[i + 1];
                if (checking <= 0) return false;
                if (Math.Abs(checking) > 3) return false;

            }
            else if (direction < 0)
            {
                var checking = intArray[i] - intArray[i + 1];
                if (checking >= 0) return false;
                if (Math.Abs(checking) > 3) return false;
            }
        }
        return true;
    }
    private static bool CheckIfSafeSpan(ReadOnlySpan<int> intSpan)
    {

        if (intSpan.Length == 0) return false;

        if (intSpan.Length == 1) return true;

        var direction = intSpan[0] - intSpan[1];
        if (direction == 0) return false;

        for (int i = 0; i < intSpan.Length - 1; i++)
        {
            if (direction > 0)
            {
                var checking = intSpan[i] - intSpan[i + 1];
                if (checking <= 0) return false;
                if (Math.Abs(checking) > 3) return false;

            }
            else if (direction < 0)
            {
                var checking = intSpan[i] - intSpan[i + 1];
                if (checking >= 0) return false;
                if (Math.Abs(checking) > 3) return false;
            }
        }
        return true;
    }

}
