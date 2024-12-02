using System.Diagnostics;
namespace Day02;
public static class Part02
{
    public static void Result(string input)
    {
        Stopwatch sw = Stopwatch.StartNew();
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

        sw.Stop();
        Console.WriteLine($"safeReports {safeReports}, Time {sw.ElapsedMilliseconds}");
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
