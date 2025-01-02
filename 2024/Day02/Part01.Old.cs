
namespace Day02;

public class Part01Old : IPart
{
    public string Result(Input input)
    {
        string[] lines = input.Text
        .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
        .ToArray();

        int safeReports = 0;
        foreach (string line in lines)
        {
            int[] numbers = line
                  .Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                  .Select(int.Parse)
                  .ToArray();

           bool safe = CheckIfSafe(numbers);

            if (safe)
            { 
                safeReports++;
            }
        }
        GlobalLog.LogLine($"safeReports {safeReports}");
        return $"{safeReports}";
    }


    public int Result_Improved(string input)
    {
        var data = InputParser(input);

        int safeReports = 0;
        foreach (int[] numbers in data)        
        {
            if (CheckIfSafe(numbers))
            {
                safeReports++;
            }
        }
        GlobalLog.LogLine($"safeReports {safeReports}");
        return safeReports;
    }

    public int[][] InputParser(string input)
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
    private int IntParser(ReadOnlySpan<Char> span)
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


    private bool CheckIfSafe(int[] intArray)
    {
        if(intArray.Length == 0) return false;

        if(intArray.Length == 1) return true;

        var direction = intArray[0] - intArray[1];
        if(direction == 0 ) return false;

        for (int i = 0; i < intArray.Length-1; i++)
        {
            if (direction > 0) 
            {
                var checking = intArray[i] - intArray[i + 1];
                if (checking <= 0) return false;
                if (Math.Abs(checking) > 3) return false;

            }
            if (direction < 0)
            {
                var checking = intArray[i] - intArray[i + 1];
                if (checking >= 0) return false;
                if (Math.Abs(checking) > 3) return false;
            }
        }
        return true;
    }
}
